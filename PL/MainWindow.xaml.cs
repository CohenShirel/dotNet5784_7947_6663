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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public partial class MainWindow : Window
    {
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
           // new MainWorkerWindow().Show();
        }
    }
}