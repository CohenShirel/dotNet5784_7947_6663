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
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        public static readonly DependencyProperty CurrentTimeProperty =
          DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(ManagerWindow), new PropertyMetadata(null));
        public ManagerWindow()
        {
            InitializeComponent();
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
                (sender as Label).Content = CurrentTime.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
    }
}