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
    /// Interaction logic for AssignmentsListWindow.xaml
    /// </summary>
    public partial class AssignmentListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //public static readonly DependencyProperty ListAssignmentsProperty =
        //        DependencyProperty.Register("currentAssignments", typeof(IEnumerable<BO.Assignment>), typeof(AssignmentsListWindow));
        public DO.Level level { get; set; } = DO.Level.None;
        public IEnumerable<BO.AssignmentInList> ListAssignments
        {
            get { return (IEnumerable<BO.AssignmentInList>)GetValue(ListAssignmentsProperty); }
            set { SetValue(ListAssignmentsProperty, value); }
        }


        //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
        public static readonly DependencyProperty ListAssignmentsProperty =
            DependencyProperty.Register("Assignments", typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow), new PropertyMetadata(null));

        

        public AssignmentListWindow()
        {
            InitializeComponent();
            ListAssignments = s_bl.Assignment.ReadAll();

        }

        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            ListAssignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new AssignmentWindow().ShowDialog();
            UpdateAssignmentsList();
            //we cant go back to previose window till we finish
        }

        private void ListView_UpdateClick(object sender, MouseButtonEventArgs e)
        {
            var ass = ((sender as ListView)?.SelectedItem as BO.AssignmentInList)!;
            new AssignmentWindow(ass!.Id).ShowDialog();
            UpdateAssignmentsList();
        }
        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            ListAssignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }
        //?????????????????
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


/*


        //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
        public static readonly DependencyProperty ListAssignmentsProperty =
            DependencyProperty.Register("Assignments", typeof(IEnumerable<BO.WorkerInList>), typeof(AssignmentsListWindow), new PropertyMetadata(null));

        public AssignmentsListWindow()
        {
            InitializeComponent();
            Assignments = s_bl.Assignment.ReadAll();

        }
        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            Assignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new WorkerWindow().ShowDialog();
            UpdateWorkerList();
            //we cant go back to previose window till we finish
        }

        private void ListView_UpdateClick(object sender, MouseButtonEventArgs e)
        {
            BO.Worker wrk = ((sender as ListView)?.SelectedItem as BO.Worker)!;
            new WorkerWindow(wrk!.Id).ShowDialog();
            UpdateWorkerList();
        }
        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            Assignments = (level == DO.Level.None) ?
              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
        }
        //Activated="Window_Activated">

        //whennnn
        private void UpdateWorkerList()
        {
            Assignments = (level == DO.Level.None) ?
             s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.l == level).OrderBy(e => e.Id);
        }

//whennnn
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            Assignments = (level == DO.Level.None) ?
             s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
        }
        //private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        //{
        //    if (Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString(), out DO.Level currentLevel))
        //    {
        //        var wkr = (currentLevel == DO.Level.None) ?
        //           s_bl?.Worker.ReadAll()?.Select(x => x)?.ToList() :
        //           s_bl?.Worker.ReadAll(item => item.Experience == currentLevel)?.ToList();
        //        if (wkr != null)
        //        {
        //            ObserveListWorker = new ObservableCollection<BO.WorkerInList>(wkr);
        //        }
        //        else
        //        {
        //            // אם המשתמש לא בחר כלום, תציג את כל העובדים
        //            var workers = s_bl?.Worker.ReadAll();
        //            if (workers != null)
        //            {
        //                var observableWorkers = new ObservableCollection<BO.WorkerInList>(workers);
        //                ObserveListWorker.Clear();
        //                foreach (var worker in observableWorkers)
        //                {
        //                    if (workers != null)
        //                    {
        //                        var observableWorkers2 = new ObservableCollection<BO.WorkerInList>(workers);
        //                        ObserveListWorker.Clear();
        //                        ObserveListWorker.Add(observableWorkers2);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}
        //ObserveListWorker = (currentLevel == DO.Level.None) ?
        //    s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == currentLevel);

        //DO.Level currentLevel = Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString());
        //ObserveListWorker = (currentLevel == DO.Level.None) ?
        //s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == currentLevel)!;



        //if (Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString(), out DO.Level currentLevel))
        //{
        //    ObserveListWorker = s_bl.Worker.ReadAll(w=>w.Experience==currentLevel);
        //}
        //else
        //    ObserveListWorker =s_bl.Worker.ReadAll();
        //ObserveListWorker = s_bl?.Worker.ReadAll()!;

        // ComboBox.ItemsSource=Enums.GetValues(typeof(DO.Level));
    }
}









*/














//using PL.Worker;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL.Assignment
//{
//    /// <summary>
//    /// Interaction logic for AssignmentListWindow.xaml
//    /// </summary>
//    /// static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//    public partial class AssignmentsListWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public static readonly DependencyProperty ListAssignmentsProperty =
//        DependencyProperty.Register("currentAssignments", typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow));
//        public DO.Level level { get; set; } = DO.Level.None;
//        public IEnumerable<BO.Assignment> currentAssignments
//        {
//            get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
//            set { SetValue(ListAssignmentsProperty, value); }
//        }
//        public AssignmentsListWindow()
//        {
//            InitializeComponent();
//        }

//        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
//        {
//            currentAssignments = (level == DO.Level.None) ?
//              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
//        }

//        private void BtnAdd_Click(object sender, RoutedEventArgs e)
//        {
//            new AssignmentsWindow().ShowDialog();
//            UpdateAssignmentsList();
//            //we cant go back to previose window till we finish
//        }

//        private void ListView_UpdateClick(object sender, MouseButtonEventArgs e)
//        {
//            BO.Assignment wrk = ((sender as ListView)?.SelectedItem as BO.Assignment)!;
//           new AssignmentsWindow(wrk!.Id).ShowDialog();
//            UpdateAssignmentsList();
//        }
//        private void ListView_SelectionChanged(object sender, EventArgs e)
//        {
//            currentAssignments = (level == DO.Level.None) ?
//              s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
//        }
//        //Activated="Window_Activated">

//        private void UpdateAssignmentsList()
//        {
//            currentAssignments = (level == DO.Level.None) ?
//             s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
//        }

//        private void ComboBox_Selected(object sender, RoutedEventArgs e)
//        {
//            currentAssignments = (level == DO.Level.None) ?
//             s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
//        }
//    }
//}