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
        public IEnumerable<BO.WorkerInList> ListWorker
        {
            get { return (IEnumerable<BO.WorkerInList>)GetValue(ListWorkerProperty); }
            set { SetValue(ListWorkerProperty, value); }
        }
        public static readonly DependencyProperty ListWorkerProperty =
            DependencyProperty.Register("ListWorker", typeof(IEnumerable<BO.WorkerInList>), typeof(WorkerListWindow), new PropertyMetadata(null));
        public WorkerListWindow()
        {
            InitializeComponent();
            ListWorker = s_bl.Worker.ReadAll();
        }
        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            ListWorker = (level == DO.Level.None) ?
              s_bl?.Worker.ReadAll() : s_bl?.Worker.ReadAll(item => item.Experience == level).OrderBy(e => e.Id);
        }
        private void BtnAdd_Click(object sender,RoutedEventArgs e)
        {
            new WorkerWindow().ShowDialog();
            UpdateWorkerList();
        }

        private void ListView_UpdateClick(object sender,MouseButtonEventArgs e)
        {
            var wrk=(sender as ListView)?.SelectedItem as BO.WorkerInList;
            new WorkerWindow(wrk.Id).ShowDialog();
            UpdateWorkerList();
        }
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
    }
}
