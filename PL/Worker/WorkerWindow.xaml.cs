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

    ObservableCollection<BO.Worker> observeListWorker;
    public ObservableCollection<BO.Worker> observeListWorker
    {
        get { return (ObservableCollection<BO.Worker>)GetValue(observeListWorkerProperty); }
        set { SetValue(observeListWorkerProperty, value); }
    }
    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
    public static readonly DependencyProperty observeListWorkerProperty =
        DependencyProperty.Register("observeListWorker", typeof(ObservableCollection<BO.Worker>), typeof(WorkerWindow), new PropertyMetadata(null));



    BO.Worker curWrk;
    public WorkerWindow(int id = 0)
    {
        InitializeComponent();

        if (id != 0)//if it's update
        {
            curWrk=s_bl.Worker.Read(id);
        }
        else
        { 
            curWrk = new BO.Worker();
        }

    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        //if.....
        s_bl.Worker.Update(curWrk);
        s_bl.Worker.Create(curWrk);
    }


    // public object 

}
