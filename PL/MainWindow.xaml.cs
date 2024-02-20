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
using BL;
using BlApi;

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
        //WorkerListWindow
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new WorkerListWindow().Show();
        }
        //reset Data
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult =MessageBox.Show("Do you want to reset the Data?",
                "RESET",MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch(mbResult) 
            {
                case MessageBoxResult.OK:
                    IBl.reset();//קראנו כאן לאבא...ןצריך לעדכן גם באקסמל
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new WorkerWindow().Show();
        }
    }
}