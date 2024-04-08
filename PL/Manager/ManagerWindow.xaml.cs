using BlImplementation;
using PL.Assignment;
using PL.Gantt;
using PL.Worker;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty lstAssignmentsProperty =
          DependencyProperty.Register("lstAssignments", typeof(List<BO.Assignment>), typeof(ManagerWindow), new PropertyMetadata(null));
        public DateTime? CurrentTime
        {
            get { return (DateTime?)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
          DependencyProperty.Register("CurrentTime", typeof(DateTime?), typeof(ManagerWindow), new PropertyMetadata(null));
        public Visibility VisibilityNewMessages
        {
            get { return (Visibility)GetValue(VisibilityNewMessagesProperty); }
            set { SetValue(VisibilityNewMessagesProperty, value); }
        }


        public static readonly DependencyProperty VisibilityNewMessagesProperty =
         DependencyProperty.Register("VisibilityNewMessages", typeof(Visibility), typeof(ManagerWindow), new PropertyMetadata(Visibility.Hidden));
       
        public ManagerWindow()
        {
            InitializeComponent();

            CurrentTime = s_bl.Clock.GetStartProject();
        }
        private void BtnWorkersListWindow_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }

        private void BtnTasksListWindow_Click(object sender, RoutedEventArgs e)
        {
            new AssignmentListWindow().Show();
        }

        private void BtnGanntWindow_Click(object sender, RoutedEventArgs e)
        {
            new GanttWindow().Show();
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to update dates for all assignments?",
              "Schedule", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    foreach (var assignment in s_bl.Assignment.ReadAllAss())
                        BO.Tools.ScheduleProject(assignment);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to reset the Data?",
               "RESET", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    Bl.reset();//קראנו כאן לאבא...ןצריך לעדכן גם באקסמל
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void BtnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to initialize the Data?",
               "initializition", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    Bl.InitializeDB();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void BtnResetClock_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to reset the Clock?",
              "RESET", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (mbResult)
            {
                case MessageBoxResult.OK:
                    s_bl.Clock.resetClock();
                    CurrentTime = s_bl.Clock.GetStartProject();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        private void BtnHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Clock.FormatClockOneHour();
            CurrentTime = s_bl.Clock.GetStartProject();
        }
        private void BtnDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Clock.FormatClockOneDay();
            CurrentTime = s_bl.Clock.GetStartProject();
        }

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Clock.FormatClockOneMonth();
            CurrentTime = s_bl.Clock.GetStartProject();
        }

        private void BtnYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Clock.FormatClockOneYear();
            CurrentTime = s_bl.Clock.GetStartProject();
        }
        private void UpdateCurrentTimeLabel(object sender, RoutedEventArgs e)
        {
            if (sender is Label)
            {
                (sender as Label)!.Content = CurrentTime.ToString(); //"dd/MM/yyyy HH:mm:ss"
            }
        }

        //private void BtnNewMessages_Click(object sender, RoutedEventArgs e)
        //{
        //    if (VisibilityNewMessages == Visibility.Hidden && s_bl.lstAssignments.Count != 0)
        //        VisibilityNewMessages = Visibility.Visible;
        //    else if (VisibilityNewMessages == Visibility.Visible && s_bl.lstAssignments.Count != 0)
        //        VisibilityNewMessages = Visibility.Hidden;
        //    else if (s_bl.lstAssignments.Count == 0)
        //        MessageBox.Show("There aren't completed tasks yet!", "No New Messages!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //}

        //private void BtnClearMessages_Click(object sender, RoutedEventArgs e)
        //{
        //    s_bl.lstAssignments?.Clear();
        //    LstAssignments = s_bl.lstAssignments!;
        //    VisibilityNewMessages = Visibility.Hidden;
        //}
    }
}