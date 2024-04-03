using PL.Gantt.GanttChart;
using PL.Gantt.PeriodSplitter;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable

namespace PL.Gantt;

/// <summary>
/// Interaction logic for GantWindow.xaml
/// </summary>
public partial class GanttWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private BackgroundWorker _backgroundWorker;
    private System.Timers.Timer _gridLinesTimelineBackgroundTimer;

    public GanttControl GanttControl
    {
        get { return (GanttControl)GetValue(GanttControlProperty); }
        set { SetValue(GanttControlProperty, value); }
    }

    // Using a DependencyProperty as the backing store for GanttChartData.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GanttControlProperty =
        DependencyProperty.Register(nameof(GanttControl), typeof(GanttControl), typeof(GanttWindow));

    public DateTime SelectedDate
    {
        get { return (DateTime)GetValue(SelectedDateProperty); }
        set { SetValue(SelectedDateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register("SelectedDate", typeof(DateTime),
            typeof(GanttWindow), new PropertyMetadata(DateTime.Now));

    public ObservableCollection<BO.Assignment> Assignments
    {
        get { return (ObservableCollection<BO.Assignment>)GetValue(AssignmentsProperty); }
        set { SetValue(AssignmentsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Assignments.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AssignmentsProperty =
        DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.Assignment>),
            typeof(GanttWindow), new PropertyMetadata(new ObservableCollection<BO.Assignment>(
               s_bl.Assignment.ReadAllAss()
             )));

    public GanttWindow()
    {
        GanttControl = new GanttControl();
        GantLenght = 50;
        initBackgroundWorker();
        Assignments = new ObservableCollection<BO.Assignment>(
               s_bl.Assignment.ReadAllAss());
        // Set selection -mode
        GanttControl.TaskSelectionMode = GanttControl.SelectionMode.Single;

        // Enable GanttAssignments to be selected
        GanttControl.AllowUserSelection = true;

        // listen to the GanttRowAreaSelected event
        GanttControl.GanttRowAreaSelected += ganttControl_GanttRowAreaSelected!;

        // define ganttAssignment context menu and action when each item is clicked
        ganttAssignmentContextMenuItems.Add(new ContextMenuItem(ViewClicked, "View..."));
        ganttAssignmentContextMenuItems.Add(new ContextMenuItem(EditClicked, "Edit..."));
        ganttAssignmentContextMenuItems.Add(new ContextMenuItem(DeleteClicked, "Delete..."));
        GanttControl.GanttTaskContextMenuItems = ganttAssignmentContextMenuItems;

        // define selection context menu and action when each item is clicked
        selectionContextMenuItems.Add(new SelectionContextMenuItem(NewClicked, "New..."));
        GanttControl.SelectionContextMenuItems = selectionContextMenuItems;
        InitializeComponent();
    }

    private int GantLenght { get; set; }
    private ObservableCollection<ContextMenuItem> ganttAssignmentContextMenuItems = new ObservableCollection<ContextMenuItem>();
    private ObservableCollection<SelectionContextMenuItem> selectionContextMenuItems = new ObservableCollection<SelectionContextMenuItem>();
    private TimeLine _gridLineTimeLine;
    private const int _interval = 2000;

    //private void initTimer()
    //{
    //    _gridLinesTimelineBackgroundTimer = new System.Timers.Timer()
    //    {
    //        Interval = 1000000,
    //        AutoReset = true
    //    };
    //    _gridLinesTimelineBackgroundTimer.Elapsed += _gridLinesTimelineBackgroundTimer_Elapsed!;
    //}

    private void initBackgroundWorker()
    {
        _backgroundWorker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        _backgroundWorker.DoWork += _backgroundWorker_DoWork;
        _backgroundWorker.ProgressChanged += _backgroundWorker_ProgressChanged;
    }

    private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (_gridLineTimeLine is not null) GanttControl.SetGridLinesTimeline(_gridLineTimeLine, DetermineBackground);
    }

    private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        while (true)
        {
            Thread.Sleep(_interval);
            _backgroundWorker.ReportProgress(0);
        }
        //  _gridLinesTimelineBackgroundTimer.Start();
    }

    private void setAssignmentsOnGantt()
    {
        var allAssignments = Assignments.ToDictionary(t => t.IdAssignment, t => t);
        foreach (var task in Assignments)
        {
            var rowGroup = GanttControl.CreateGanttRowGroup(task.Name, true);

            if (task.Links is not null)
            {
                var dependensAssignmentsSet = task.Links.Select(task => task.Id).ToHashSet();

                var dependensAssignments = Assignments.Where(taskItem => dependensAssignmentsSet
                                          .Contains(taskItem.IdAssignment))
                                          .OrderBy(task => task.DateBegin/*ScheduledDate*/);

                foreach (var dependsAssignment in dependensAssignments) addGanttRow(dependsAssignment, rowGroup);
            }

            addGanttRow(task, rowGroup);
        }
    }
    private void addGanttRow(BO.Assignment task, ExpandableGanttRowGroup rowGroup)
    {
        var row = GanttControl.CreateGanttRow(rowGroup, task.Name);
        GanttControl.AddGanttTask(row, taskToGanttAssignment(task));
    }

    private static GanttTask taskToGanttAssignment(BO.Assignment task) =>
        new GanttTask()
        {
            Start = task.DateBegin/*ScheduledDate*/!.Value,
            End = task.DeadLine/*EstimatedCompleteDate*/!.Value,
            Name = task.Name!,
            TaskProgressVisibility = Visibility.Visible
        };

    private void NewClicked(Period selectionPeriod)
    {
        MessageBox.Show("New clicked for task " + selectionPeriod.Start.ToString() + " -> " + selectionPeriod.End.ToString());
    }

    private void ViewClicked(GanttTask ganttAssignment)
    {
        MessageBox.Show("New clicked for task " + ganttAssignment.Name);
    }

    private void EditClicked(GanttTask ganttAssignment)
    {
        MessageBox.Show("Edit clicked for task " + ganttAssignment.Name);
    }

    private void DeleteClicked(GanttTask ganttAssignment)
    {
        MessageBox.Show("Delete clicked for task " + ganttAssignment.Name);
    }

    void ganttControl_GanttRowAreaSelected(object sender, PeriodEventArgs e)
    {
        MessageBox.Show(e.SelectionStart.ToString() + " -> " + e.SelectionEnd.ToString());
    }

    private Brush DetermineBackground(TimeLineItem timeLineItem, Color color = default)
    {
        if (timeLineItem.End.Date.DayOfWeek == DayOfWeek.Saturday || timeLineItem.End.Date.DayOfWeek == DayOfWeek.Sunday)
            return new SolidColorBrush(Colors.LightBlue);
        else
            return new SolidColorBrush(color);
    }

    private void _gridLinesTimelineBackgroundTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // Set the timeline to atatch gridlines to
        GanttControl.SetGridLinesTimeline(_gridLineTimeLine, DetermineBackground);
    }

    private void CreateData(DateTime minDate, DateTime maxDate)
    {
        // Set max and min dates
        GanttControl.Initialize(minDate, maxDate);

        // Create timelines and define how they should be presented
        GanttControl.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear);
        GanttControl.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth);
        GanttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDayName);
        _gridLineTimeLine = GanttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay);
        setAssignmentsOnGantt();

        if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync();

    }

    private string FormatYear(Period period)
    {
        return period.Start.Year.ToString();
    }

    private string FormatMonth(Period period)
    {
        return period.Start.Month.ToString();
    }

    private string FormatDay(Period period)
    {
        return period.Start.Day.ToString();
    }

    private string FormatDayName(Period period)
    {
        return period.Start.DayOfWeek.ToString();
    }

    private void buttonPrevious_Click(object sender, EventArgs e)
    {
        dateTimePicker.SelectedDate = GanttControl.GanttData.MinDate.AddDays(-GantLenght);
    }

    private void buttonNext_Click(object sender, EventArgs e)
    {
        dateTimePicker.SelectedDate = GanttControl.GanttData.MaxDate;
    }

    private void dateTimePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        DateTime maxDate = SelectedDate.AddDays(GantLenght);
        GanttControl.ClearGantt();
        CreateData(SelectedDate, maxDate);
    }
}
