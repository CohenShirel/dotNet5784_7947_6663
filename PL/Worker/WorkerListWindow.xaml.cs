using BO;
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

namespace PL.Worker
{
    public partial class WorkerListWindow : Window
    {
        public WorkerListWindow()
        {
            InitializeComponent();
            WorkerList = s_bl?.Worker.ReadAll()!;
            //ComboBox.ItemsSource=Enums.GetValues(typeof(DO.Level));
        }
        public DO.Level level { get; set; } = DO.Level.None;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.WorkerInList> WorkerList
        {
            get { return (IEnumerable<BO.WorkerInList>)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }

        public static readonly DependencyProperty WorkerListProperty =
            DependencyProperty.Register("WorkerList", typeof(IEnumerable<BO.WorkerInList>), typeof(WorkerListWindow), new PropertyMetadata(null));
       

        private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
        {
            WorkerList = (level == DO.Level.None) ?
            s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;

        }
    }
}
