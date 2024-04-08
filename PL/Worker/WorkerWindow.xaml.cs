using System.Windows;
using System.Windows.Controls;
namespace PL.Worker;
public partial class WorkerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Worker currentWorker
    {
        get { return (BO.Worker)GetValue(WorkerListProperty); }
        set { SetValue(WorkerListProperty, value); }
    }
    public static readonly DependencyProperty WorkerListProperty =
        DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));
    public WorkerWindow(int id = 0)
    {
        InitializeComponent();
        try
        {
            currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None };
            if (currentWorker!=null && currentWorker.Id != 0)
                txtId.IsEnabled = false;
        }
        catch(BO.Exceptions.BlDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message,"Operation Faild",MessageBoxButton.OK,MessageBoxImage.Exclamation);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if ((sender as Button).Content.ToString() == "Update")
            {
                s_bl.Worker.Update(currentWorker);
                MessageBox.Show("The Update was successfull");
            }
            else
            {
                int? id = s_bl.Worker.Create(currentWorker);
                MessageBox.Show("The Addition was made successfully");
            }
            this.Close();
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

    private void cmbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       
    }

    private void txtId_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(!int.TryParse(((TextBox)sender).Text, out int id))
        {
            ((TextBox)sender).IsEnabled = false;
        }
        
    }
}