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


        //ObservableCollection <BO.WorkerInList> lsttWorker;
        public ObservableCollection<IEnumerable<BO.WorkerInList>> ObserveListWorker
        {
            get { return (ObservableCollection<IEnumerable<BO.WorkerInList>>)GetValue(observeListWorkerProperty); }
            set { SetValue(observeListWorkerProperty, value); }
        }
        //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
        public static readonly DependencyProperty observeListWorkerProperty =
            DependencyProperty.Register("ObserveListWorker", typeof(ObservableCollection<IEnumerable<BO.WorkerInList>>), typeof(WorkerListWindow), new PropertyMetadata(null));
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
           // ObserveListWorker = s_bl?.Worker.ReadAll()!;
           // ComboBox.ItemsSource=Enums.GetValues(typeof(DO.Level));
        }
        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            DO.Level currentLevel = Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString());
            ObserveListWorker = (currentLevel == DO.Level.None) ?
            s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == currentLevel)!;
        }
        //if (Enum.TryParse<DO.Level>(((ComboBox)sender).SelectedItem?.ToString(), out DO.Level currentLevel))
        //{
        //    ObserveListWorker = s_bl.Worker.ReadAll(w=>w.Experience==currentLevel);
        //}
        //else
        //    ObserveListWorker =s_bl.Worker.ReadAll();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new WorkerWindow().ShowDialog();
            //we cant go back to previose window till we finish
        }
    }
}
