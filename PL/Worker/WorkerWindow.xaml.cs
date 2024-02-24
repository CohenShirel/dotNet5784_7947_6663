using BO;
using DO;
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
//DataContext = "{Binding CurrentWorker, RelativeSource={RelativeSource Self}, Mode=TwoWay}" >

namespace PL.Worker;
//שימי לב כאן הקומבובוקס חייב להיות דפנדסי פרןפרטי כי אני רוצה לדווח על שינויים ולכן זה צריך להיות דו צדדי
//בשונה מהחלון שמראה לי את רשימת העובדים שם זה לא צריך להיות דפנדסי פרופרטי
/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
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
                MessageBox.Show("The Update was successful");
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

//ObservableCollection<BO.Worker> observeListWorker;
//public ObservableCollection<BO.Worker> ObserveListWorker
//{
//    get { return (ObservableCollection<BO.Worker>)GetValue(observeListWorkerProperty); }
//    set { SetValue(observeListWorkerProperty, value); }
//}
////יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
//public static readonly DependencyProperty observeListWorkerProperty =
//    DependencyProperty.Register("ObserveListWorker", typeof(ObservableCollection<BO.Worker>), typeof(WorkerWindow), new PropertyMetadata(null));

//if ((sender as Button).DataContext.ToString() == "Update")
//{
//    try
//    {
//        s_bl.Worker.Update(curWrk);
//        MessageBox.Show("The Update was successful");
//        this.Close();
//    }
//    catch (BO.Exceptions.BlDoesNotExistException ex)
//    {
//        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    }
//}
//else//Adding
//{
//    try
//    {
//        int? id = s_bl.Worker.Create(curWrk);
//        MessageBox.Show("The Addition was made successfully");
//        this.Close();
//    }
//    catch (BO.Exceptions.BlDoesNotExistException ex)
//    {
//        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    }
//}