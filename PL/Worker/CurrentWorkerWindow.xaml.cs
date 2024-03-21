using BlImplementation;
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
    public IEnumerable<BO.Assignment> ListAssignments
    {
        get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
        set { SetValue(ListAssignmentsProperty, value); }
    }
    public static readonly DependencyProperty ListAssignmentsProperty =
       DependencyProperty.Register("Assignments", typeof(IEnumerable<BO.Assignment>), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
    //=================currentAssignment
    public BO.Assignment currentAssignment
    {
        get { return (BO.Assignment)GetValue(AssignmentListProperty); }
        set { SetValue(AssignmentListProperty, value); }
    }
    public static readonly DependencyProperty AssignmentListProperty =
        DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(CurrentWorkerWindow), new PropertyMetadata(null));
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
        //bool isVisible = false;
        BO.Worker wrk=s_bl.Worker.Read(id);
        currentAssignment = s_bl.Assignment.Read(ass => ass.WorkerId == id && ass.DateFinish<=s_bl.Clock.Date);
        ListAssignments = s_bl.Assignment.ReadAllAss(ass=>ass.LevelAssignment<=wrk.Experience && ass.IdWorker== default(int));
        //אין משימות קודמות שלא הסתיימ
    }

    private void btnMyTask_Click(object sender, RoutedEventArgs e)
    {
        if (VisibilityAssignment == Visibility.Hidden)
            VisibilityAssignment = Visibility.Visible;
        else
            VisibilityAssignment = Visibility.Hidden;
    }

    private void BtnAllMyTasks_Click(object sender, RoutedEventArgs e)
    {
        if (VisibilityListAssignment == Visibility.Hidden)
            VisibilityListAssignment = Visibility.Visible;
        else
            VisibilityListAssignment = Visibility.Hidden;
    }

    private void btnEndAssignment_Click(object sender, RoutedEventArgs e)
    {
        currentAssignment.DateFinish=s_bl.Clock.Date;
        //להודיע כאן למנהל על סיום משמימה
    }
    //המהנדס הזה אחראי עליה ושאין לה תאריך סיום
}

