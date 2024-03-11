using BlApi;
using BlImplementation;
using PL.Assignment;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void BtnWorkersListWindow_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }

        private void BtnTasksListWindow_Click(object sender, RoutedEventArgs e)
        {
            new AssignmentsListWindow().Show();
        }

        private void BtnGanntWindow_Click(object sender, RoutedEventArgs e)
        {
            new GanntWindow().Show();
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

        private void BtnHour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMonth_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnYear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
