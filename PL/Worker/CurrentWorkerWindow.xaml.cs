using BlImplementation;
using BO;
using DO;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Worker;

public partial class CurrentWorkerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public int ID { get; set; }
    //public IEnumerable<BO.Assignment> ListAssignments
    //{
    //    get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
    //    set { SetValue(ListAssignmentsProperty, value); }
    //}
    //public static readonly DependencyProperty ListAssignmentsProperty =
    //   DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.Assignment>), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
    public IEnumerable<BO.Assignment> ListAssignments
    {
        get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
        set { SetValue(ListAssignmentsProperty, value); }
    }
    public static readonly DependencyProperty ListAssignmentsProperty =
       DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.Assignment>), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
    //=================currentAssignment
    public BO.Assignment currentAssignment
    {
        get { return (BO.Assignment)GetValue(AssignmentListProperty); }
        set { SetValue(AssignmentListProperty, value); }
    }
    public static readonly DependencyProperty AssignmentListProperty =
        DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
    public BO.Worker currentWorker
    {
        get { return (BO.Worker)GetValue(WorkerListProperty); }
        set { SetValue(WorkerListProperty, value); }
    }
    public static readonly DependencyProperty WorkerListProperty =
        DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
    public Visibility VisibilityAssignment
    {
        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
     DependencyProperty.Register("VisibilityAssignment", typeof(Visibility), typeof(CurrentWorkerWindow), new PropertyMetadata(Visibility.Hidden));
    public Visibility VisibilityListAssignment
    {
        get { return (Visibility)GetValue(AssignmentListDetailsVisibilityProperty); }
        set { SetValue(AssignmentListDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssignmentListDetailsVisibilityProperty =
     DependencyProperty.Register("VisibilityListAssignment", typeof(Visibility), typeof(CurrentWorkerWindow), new PropertyMetadata(Visibility.Hidden));
    public CurrentWorkerWindow(int id)
    {
        InitializeComponent();
        currentWorker = s_bl.Worker.Read(id);
        BO.Worker wrk = s_bl.Worker.Read(id);
        currentAssignment = s_bl.Assignment.Read(ass => ass.WorkerId == id /*&& ass.DeadLine<=s_bl.Clock.Date*/);
        ListAssignments = s_bl.Assignment.ReadAllAss(ass => ass.LevelAssignment <= wrk.Experience && ass.IdWorker == default(int) && anyPreviousAssignmemts(ass) == true);
        //ListAssignments = s_bl.Assignment.ReadAllAss(ass => ass.LevelAssignment == wrk.Experience /*&& ass.IdWorker== default(int)*/);
        //אין משימות קודמות שלא הסתיימ
    }

    private void btnMyTask_Click(object sender, RoutedEventArgs e)
    {
        if (VisibilityAssignment == Visibility.Hidden && currentAssignment != null)
            VisibilityAssignment = Visibility.Visible;
        else if (VisibilityAssignment == Visibility.Visible && currentAssignment != null)
            VisibilityAssignment = Visibility.Hidden;
        else if (currentAssignment == null)
            MessageBox.Show("You are not currently working on a task...you need to select a task from the list", "Pay Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

    }
    //פונקציה שבודקת שכל המשימות הקודמות של אותה המשימה שהתקבלה הסתיימו כבר
    private void BtnAllMyTasks_Click(object sender, RoutedEventArgs e)
    {
        if (VisibilityListAssignment == Visibility.Hidden)
            VisibilityListAssignment = Visibility.Visible;
        else
            VisibilityListAssignment = Visibility.Hidden;
    }
    private bool anyPreviousAssignmemts(BO.Assignment assignment)
    {
        List<AssignmentInList>? Links = assignment.Links;
        if (Links == null)
            return true;
        //go through all the previous tasks and checks that they have already been completed
        foreach (AssignmentInList link in Links)
        {
            BO.Assignment currentAssignment = s_bl.Assignment.Read(link.Id)!;
            if (currentAssignment.DateFinish > s_bl.Clock)
                return false;
        }
        return true;
    }
    private void btnEndAssignment_Click(object sender, RoutedEventArgs e)
    {
        currentAssignment.DeadLine = s_bl.Clock.Date;
        //להודיע כאן למנהל על סיום משמימה
    }
    private void UpdateWindow()
    {
        BO.Worker wrk = s_bl.Worker.Read(ID);
        currentAssignment = s_bl.Assignment.Read(ass => ass.WorkerId == ID /*&& ass.DeadLine<=s_bl.Clock.Date*/);
        ListAssignments = s_bl.Assignment.ReadAllAss(ass => ass.LevelAssignment <= wrk.Experience && ass.IdWorker == default(int) && anyPreviousAssignmemts(ass) == true);
    }
    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (currentAssignment == null)
        {
            var ass = ((sender as ListView)?.SelectedItem as BO.Assignment)!;
            MessageBoxResult mbResult = MessageBox.Show("Do you want to work on {0}", ass.Name, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    {
                        currentWorker.currentAssignment = new BO.WorkerInAssignment(currentWorker.Id, ass.IdAssignment);
                        ass.IdWorker = currentWorker.Id;
                        ass.dateSrart = s_bl.Clock.Date;
                        //Assignment = ass;
                        UpdateWindow();
                    }
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        else
            MessageBox.Show("You can't select new task, till you will finish your current task!", "Pay Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //public WorkerInAssignment currentAssignment { get; set; }
        //public class WorkerInAssignment
        //{
        //    public int WorkerId { get; init; }
        //    public required int AssignmentNumber { get; init; }

        //}
        //המהנדס הזה אחראי עליה ושאין לה תאריך סיום
    }
}

