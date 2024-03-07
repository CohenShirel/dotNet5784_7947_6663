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
    /// Interaction logic for AssignmentsWindow.xaml
    /// </summary>
    public partial class AssignmentsWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        /*
        public static readonly DependencyProperty ListAssignmentsProperty =
       DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));
        */
        public static readonly DependencyProperty ListAssignmentsProperty =
           DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow));
        public DO.Level level { get; set; } = DO.Level.None;
        public IEnumerable<BO.AssignmentInList> ListAssignments
        {
            get { return (IEnumerable<BO.AssignmentInList>)GetValue(ListAssignmentsProperty); }
            set { SetValue(ListAssignmentsProperty, value); }
        }
        public AssignmentsWindow()
        {
            InitializeComponent();
        }

        public AssignmentsWindow(int id = 0)
        {
            InitializeComponent();
            try
            {
                /*
                currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None };
                if (currentWorker != null && currentWorker.Id != 0)
                    txtId.IsEnabled = false;*/
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

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as Button).Content.ToString() == "Update")
                {
                    //s_bl.Worker.Update(currentWorker);
                    MessageBox.Show("The Update was successfull");
                }
                else
                {
                    //int? id = s_bl.Worker.Create(currentWorker);
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
            //Level l= enum.TryParse(((ComboBox) sender).Text;
            //currentWorker = (level== DO.Level.None) ?
            //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
        }
    }
}

/*

public partial class WorkerWindow : Window
{
    //רשימה שיודעת לדווח למי שמקושר אליה שנוספו אברים
    //לא יכול להיות  ליסט כי אז הוא יתקשר רק חדפ ואני רוצה שכל פעם זה יתעדכן מהגרפיקה לכאן
    
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Worker currentWorker
    {
        get { return (BO.Worker)GetValue(WorkerListProperty); }
        set { SetValue(WorkerListProperty, value); }
    }
    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
    public static readonly DependencyProperty WorkerListProperty =
        DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));


    //currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None

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
        //Level l= enum.TryParse(((ComboBox) sender).Text;
        //currentWorker = (level== DO.Level.None) ?
        //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
    }

    private void txtId_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(!int.TryParse(((TextBox)sender).Text, out int id))
        {
            ((TextBox)sender).IsEnabled = false;
        }
        
    }
}

 * */