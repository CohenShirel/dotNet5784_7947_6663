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

namespace PL.Assignment
{
    /// <summary>
    /// Interaction logic for AssignmentListWindow.xaml
    /// </summary>
    /// static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
  
    public partial class AssignmentListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty ListAssignmentsProperty =
           DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow));
        public DO.Level level { get; set; } = DO.Level.None;
        public IEnumerable<BO.AssignmentInList> ListAssignments
        {
            get { return (IEnumerable<BO.AssignmentInList>)GetValue(ListAssignmentsProperty); }
            set { SetValue(ListAssignmentsProperty, value); }
        }
        public AssignmentListWindow()
        {
            InitializeComponent();
        }

        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            ListAssignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AssignmentsWindow().ShowDialog();
            UpdateAssignmentsList();
            //we cant go back to previose window till we finish
        }

        private void ListView_UpdateClick(object sender, MouseButtonEventArgs e)
        {
            BO.AssignmentInList wrk = ((sender as ListView)?.SelectedItem as BO.AssignmentInList)!;
           new AssignmentsWindow(wrk!.Id).ShowDialog();
            UpdateAssignmentsList();
        }
        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            ListAssignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }
        //Activated="Window_Activated">

        private void UpdateAssignmentsList()
        {
            ListAssignments = (level == DO.Level.None) ?
             s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ListAssignments = (level == DO.Level.None) ?
             s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
        }
    }
}
