using PL.Worker;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BlImplementation;
using DO;

namespace PL
{
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int ID { get; set; }
        public BO.Worker currentWorker
        {
            get { return (BO.Worker)GetValue(WorkerListProperty); }
            set { SetValue(WorkerListProperty, value); }
        }
        public static readonly DependencyProperty WorkerListProperty =
            DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(MainWindow), new PropertyMetadata(null));
        public Visibility WorkerDetailsVisibility
        {
            get { return (Visibility)GetValue(WorkerDetailsVisibilityProperty); }
            set { SetValue(WorkerDetailsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty WorkerDetailsVisibilityProperty =
         DependencyProperty.Register("WorkerDetailsVisibility", typeof(Visibility), typeof(MainWindow), new PropertyMetadata(Visibility.Collapsed));
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnManagerWindow_Click(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
        }

        private void BtnWorkerWindow_Click(object sender, RoutedEventArgs e)
        {
            if (WorkerDetailsVisibility == Visibility.Collapsed)
                WorkerDetailsVisibility = Visibility.Visible;
            else
                WorkerDetailsVisibility = Visibility.Collapsed;
        }

        private void BtnAtackPanelWorker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentWorker = s_bl.Worker.Read(ID);
                new CurrentWorkerWindow(ID).Show();
            }
            catch (BO.Exceptions.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } 
        }
    }
}