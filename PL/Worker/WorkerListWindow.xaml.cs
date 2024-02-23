using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Worker
{
    public partial class WorkerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DO.Level level { get; set; } = DO.Level.None;
        //public DO.Level level
        //{
        //    get { return (DO.Level)GetValue(LevelProperty); }
        //    set { SetValue(LevelProperty, value); }
        //}

        //public static readonly DependencyProperty LevelProperty =
        //    DependencyProperty.Register("Level", typeof(DO.Level), typeof(WorkerWindow), new PropertyMetadata(null));


        //ObservableCollection <BO.WorkerInList> lsttWorker;
        public IEnumerable<BO.WorkerInList> ListWorker
        {
            get { return (IEnumerable<BO.WorkerInList>)GetValue(ListWorkerProperty); }
            set { SetValue(ListWorkerProperty, value); }
        }
        //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
        public static readonly DependencyProperty ListWorkerProperty =
            DependencyProperty.Register("ObserveListWorker", typeof(IEnumerable<BO.WorkerInList>), typeof(WorkerListWindow), new PropertyMetadata(null));
        //public IEnumerable<BO.WorkerInList> WorkerList
        //{
        //    get { return (IEnumerable<BO.WorkerInList>)GetValue(WorkerListProperty); }
        //    set { SetValue(WorkerListProperty, value); }
        //}

        //public static readonly DependencyProperty WorkerListProperty =
        //    DependencyProperty.Register("WorkerList", typeof(IEnumerable<BO.WorkerInList>), typeof(WorkerListWindow), new PropertyMetadata(null));
        public WorkerListWindow()
        {
            InitializeComponent();
           // ListWorker = s_bl.Worker.ReadAll();
        }
        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            ListWorker = (level == DO.Level.None) ?
              s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == level);
            
        }
        private void BtnAdd_Click(object sender,RoutedEventArgs e)
        {
            new WorkerWindow().ShowDialog();
            UpdateWorkerList();
            //we cant go back to previose window till we finish
        }

        private void ListView_UpdateClick(object sender,System.Windows.Input.MouseButtonEventArgs e)
        {
            BO.WorkerInList wrk=((sender as ListView)?.SelectedItem as BO.WorkerInList)!;
            new WorkerWindow(wrk!.Id).ShowDialog();
            UpdateWorkerList();
        }
        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            ListWorker = (level == DO.Level.None) ?
              s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == level).OrderBy(e => e.Id);
        }
        //Activated="Window_Activated">

        private void UpdateWorkerList()
        {
            ListWorker = (level == DO.Level.None) ?
             s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == level).OrderBy(e => e.Id);
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ListWorker = (level == DO.Level.None) ?
             s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == level);
        }
        //private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString(), out DO.Level currentLevel))
        //    //{
        //    //    var wkr = (currentLevel == DO.Level.None) ?
        //    //       s_bl?.Worker.ReadAll()?.Select(x => x)?.ToList() :
        //    //       s_bl?.Worker.ReadAll(item => item.Experience == currentLevel)?.ToList();
        //    //    if (wkr != null)
        //    //    {
        //    //        ObserveListWorker = new ObservableCollection<BO.WorkerInList>(wkr);
        //    //    }
        //    //    else
        //    //    {
        //    //        // אם המשתמש לא בחר כלום, תציג את כל העובדים
        //    //        var workers = s_bl?.Worker.ReadAll();
        //    //        if (workers != null)
        //    //        {
        //    //            var observableWorkers = new ObservableCollection<BO.WorkerInList>(workers);
        //    //            ObserveListWorker.Clear();
        //    //            foreach (var worker in observableWorkers)
        //    //            {
        //    //                if (workers != null)
        //    //                {
        //    //                    var observableWorkers2 = new ObservableCollection<BO.WorkerInList>(workers);
        //    //                    ObserveListWorker.Clear();
        //    //                    ObserveListWorker.Add(observableWorkers2);
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //}
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
