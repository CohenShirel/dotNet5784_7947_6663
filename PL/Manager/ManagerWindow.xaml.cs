using BlApi;
using BlImplementation;
using PL.Assignment;
using PL.Gantt;
using PL.Manager;
using PL.Worker;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace PL
{
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //public List<BO.Assignment> lstAssignments { get; set; }
        public List<BO.Assignment> lstAssignments
        {
            get { return (List<BO.Assignment>)GetValue(lstAssignmentsProperty); }
            set { SetValue(lstAssignmentsProperty, value); }
        }
        public static readonly DependencyProperty lstAssignmentsProperty =
          DependencyProperty.Register("lstAssignments", typeof(List<BO.Assignment>), typeof(ManagerWindow), new PropertyMetadata(null));
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
          DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(ManagerWindow), new PropertyMetadata(null));
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
            lstAssignments = s_bl.lstAssignments;
            CurrentTime = s_bl.Clock;
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
                    s_bl.ResetClock();
                    CurrentTime = s_bl.Clock;
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        private void BtnHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.FormatClockOneHour();
            CurrentTime = s_bl.Clock;
        }
        private void BtnDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.FormatClockOneDay();
            CurrentTime = s_bl.Clock;
        }

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.FormatClockOneMonth();
            CurrentTime = s_bl.Clock;
        }

        private void BtnYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.FormatClockOneYear();
            CurrentTime = s_bl.Clock;
        }
        private void SaveDateButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Label_Initialized(object sender, EventArgs e)
        {
            //(sender as Label).Content = CurrentTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void UpdateCurrentTimeLabel(object sender, RoutedEventArgs e)
        {
            if (sender is Label)
            {
                (sender as Label)!.Content = CurrentTime.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        private void BtnNewMessages_Click(object sender, RoutedEventArgs e)
        {
            if (VisibilityNewMessages == Visibility.Hidden && s_bl.lstAssignments.Count !=0)
                VisibilityNewMessages = Visibility.Visible;
            else if (VisibilityNewMessages == Visibility.Visible && s_bl.lstAssignments.Count != 0)
                VisibilityNewMessages = Visibility.Hidden;
            else if (s_bl.lstAssignments.Count == 0)
                MessageBox.Show("There aren't completed tasks yet!", "No New Messages!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void BtnClearMessages_Click(object sender, RoutedEventArgs e)
        {
            s_bl.lstAssignments?.Clear();
            lstAssignments = s_bl.lstAssignments!;
            VisibilityNewMessages = Visibility.Hidden;
        }
    }
}