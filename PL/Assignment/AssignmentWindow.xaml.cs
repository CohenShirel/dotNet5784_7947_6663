using BO;
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

namespace PL.Assignment;

/// <summary>
/// Interaction logic for AssignmentsWindow.xaml
/// </summary>
public partial class AssignmentWindow : Window
{
    //List<CheckBox> AssignmentDetailsLCBox = new();
   // List<CheckBox> lCBox = new List<CheckBox>();
    List<CheckBox> lCBox = new();

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;

    public IEnumerable<BO.AssignmentInList> ListAssignments
    {
        get { return (IEnumerable<BO.AssignmentInList>)GetValue(List1AssignmentsProperty); }
        set { SetValue(List1AssignmentsProperty, value); }
    }


    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
    public static readonly DependencyProperty List1AssignmentsProperty =
        DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));

   

    public BO.Assignment currentAssignment
    {
        get { return (BO.Assignment)GetValue(AssignmentsListProperty); }
        set { SetValue(AssignmentsListProperty, value); }
    }

    public static readonly DependencyProperty AssignmentsListProperty =
    DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow)*/

    public Visibility AssignmentDetailsVisibility
    {
        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(AssignmentWindow), new PropertyMetadata(Visibility.Collapsed));


    public CheckBox AssignmentDetailsLCBox
    {
        get { return (CheckBox)GetValue(AssignmentDetailsLCBoxProperty); }
        set { SetValue(AssignmentDetailsLCBoxProperty, value); }
    }
    public static readonly DependencyProperty AssignmentDetailsLCBoxProperty =
    DependencyProperty.Register("AssignmentDetailsLCBox", typeof(CheckBox), typeof(AssignmentWindow), new PropertyMetadata(null));

    public AssignmentWindow(int id = 0)
    {
        InitializeComponent();
        //List<CheckBox> lCBox = new List<CheckBox>();

        CheckBox checkBox;
        ListAssignments = s_bl.Assignment.ReadAll();
        foreach (var x in ListAssignments)
        {
            checkBox = new CheckBox() { Content = x, IsChecked = false };
            //AssignmentDetailsLCBox = checkBox;

            lCBox.Add(checkBox);
            //lCBox.Add(new CheckBox() { Content = x, IsChecked = false });

        }

        //lview.ItemsSource= lCBox;
        //

        try
        {
            Status s = Tools.GetProjectStatus();
            if (s == BO.Status.Unscheduled || s == BO.Status.Scheduled)
            {
                //sp.Visibility = Visibility.Visible;
                AssignmentDetailsVisibility = Visibility.Visible;

            }
            else
                AssignmentDetailsVisibility = Visibility.Collapsed;

            //sp.Visibility = Visibility.Collapsed;

            //currentAssignment = (id != 0) ? new List<BO.Assignment>() { s_bl.Assignment.Read(id)!} : new List<BO.Assignment>() { new BO.Assignment() {IdAssignment=0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null } };
            currentAssignment = (id != 0) ? s_bl.Assignment.Read(id) : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null, LevelAssignment = DO.Level.None };
            if (currentAssignment != null && currentAssignment.IdAssignment != 0)
                txtId.IsEnabled = false;
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
    public BO.Assignment currentAssignmentData
    {
        get { return currentAssignment; }
        set { currentAssignment = value; }
    }
    private void txtId_TextChanged(object sender, TextChangedEventArgs e)
    {
        
    }

    private void txtName_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        List<AssignmentInList> list = new List<AssignmentInList>();
        foreach (var x in lCBox)
        {
            if (x.IsChecked == true)
            {
                int id = GetAssignmentId(x);
                string name = GetAssignmentName(x);

                BO.AssignmentInList assl = new BO.AssignmentInList { Id = id, AssignmentName = name };
                list.Add(assl);

            }
        }
        try
        {
            if ((sender as Button).Content.ToString() == "Update")
            {
                s_bl.Assignment.Update(currentAssignment);
                MessageBox.Show("The Update was successfull");
            }
            else
            {
                int? id = s_bl.Assignment.Create(currentAssignment);
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
        //Level l = enum.TryParse(((ComboBox) sender).Text;
        //currentWorker = (level== DO.Level.None) ?
        //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private int GetAssignmentId(CheckBox checkBox)
    {
        // במידה והתיבה מכילה את המידע בפורמט AssignmentInList, יש להוציא את המזהה מה־Content שלה
        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
        if (assignmentInList != null)
        {
            return assignmentInList.Id;
        }
        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
        return -1; // ניתן לשנות את הערך לפי הצורך
    }
    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = txtSearch.Text.ToLower(); // Convert search text to lowercase for case-insensitive search
        List<AssignmentInList> filteredList = ListAssignments.Where(assignment => assignment.AssignmentName.ToLower().Contains(searchText)).ToList();
        lview.ItemsSource = filteredList;
    }

    private string GetAssignmentName(CheckBox checkBox)
    {
        // במידה והתיבה מכילה את המידע בפורמט AssignmentInList, יש להוציא את השם מה־Content שלה
        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
        if (assignmentInList != null)
        {
            return assignmentInList.AssignmentName;
        }
        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
        return string.Empty; // ניתן לשנות את הערך לפי הצורך
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




/*
  public static readonly DependencyProperty ListAssignmentsProperty =
 DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));
  */

//public static readonly DependencyProperty ListAssignmentsProperty =
//   DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentsWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow)*/
//public DO.Level level { get; set; } = DO.Level.None;
//public IEnumerable<BO.Assignment> currentAssignment
//{
//    get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
//    set { SetValue(ListAssignmentsProperty, value); }
//} public DO.Level level { get; set; } = DO.Level.None;