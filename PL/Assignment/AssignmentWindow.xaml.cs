﻿using BO;
using DalApi;
using DO;
using MaterialDesignThemes.Wpf;
using PL.Worker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using static PL.Assignment.AssignmentWindow;

namespace PL.Assignment;

/// <summary>
/// Interaction logic for AssignmentsWindow.xaml
/// </summary>
public partial class AssignmentWindow : Window
{
    //List<CheckBox> AssignmentDetailsLCBox = new();
    // List<CheckBox> lCBox = new List<CheckBox>();

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;

    public ObservableCollection<BO.AssignmentInList> Assignments
    {
        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(List1AssignmentsProperty); }
        set { SetValue(List1AssignmentsProperty, value); }
    }

    private HashSet<BO.AssignmentInList> _assignmentSet => Assignments.ToHashSet();
    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
    public static readonly DependencyProperty List1AssignmentsProperty =
        DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));

    //public BO.Assignment currentAssignment
    //{
    //    get
    //    {
    //        var value = GetValue(AssignmentsListProperty);
    //        return value != null ? (BO.Assignment)value : new BO.Assignment() { IdAssignment = -1 };
    //    }
    //    set { SetValue(AssignmentsListProperty, value); }
    //}
    public BO.Assignment currentAssignment
    {
        get { return (BO.Assignment)GetValue(AssignmentsListProperty); }
        set { SetValue(AssignmentsListProperty, value); }
    }

    public static readonly DependencyProperty AssignmentsListProperty =
    DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow)*/




    public Visibility AssignmentDetailsVisibility
    {
        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(AssignmentWindow), new PropertyMetadata(Visibility.Collapsed));

    private List<BO.AssignmentInList> _selectedAssignments = new();







    //private bool isLinked;
    //public bool IsLinked
    //{
    //    get { return isLinked; }
    //    set
    //    {
    //        isLinked = value;
    //        OnPropertyChanged(nameof(IsLinked));
    //    }
    //}

    //// Add OnPropertyChanged method for INotifyPropertyChanged
    //public event PropertyChangedEventHandler PropertyChanged;
    //protected virtual void OnPropertyChanged(string propertyName)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}





    public AssignmentWindow(int id = 0)
    {
        InitializeComponent();
      
        CheckBox checkBox;
      
        try
        {
            currentAssignment = (id != 0) ? s_bl.Assignment.Read(id)! : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", LevelAssignment = DO.Level.None };
            if (currentAssignment != null && currentAssignment.IdAssignment != 0)
                txtId.IsEnabled = false;
           
            var allAssignments = s_bl.Assignment.ReadAll().ToList(); // קריאת כל המשימות
            Assignments = new ObservableCollection<AssignmentInList>(FilterAssignmentsToShow(allAssignments, currentAssignment!)); // יצירת ObservableCollection מהמשימות הסוננות
            
            Status s = Tools.GetProjectStatus();
            if (s == BO.Status.Unscheduled || s == BO.Status.Scheduled)
            {
                //sp.Visibility = Visibility.Visible;
                AssignmentDetailsVisibility = Visibility.Visible;

            }
            else
                AssignmentDetailsVisibility = Visibility.Collapsed;
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

    private IEnumerable<AssignmentInList> FilterAssignmentsToShow(IEnumerable<AssignmentInList> allAssignments, BO.Assignment currentAssignment)
    {

        //var links0 = s_bl.Assignment.Read(currentAssignment.IdAssignment).Links; // קריאת כל המשימות
        var links00 =  s_bl.Assignment.GetLinkedAssignments(currentAssignment);
        //List<AssignmentInList> links1 = currentAssignment.Links!;
        List<AssignmentInList> links2 = new List<AssignmentInList>();
        // אתה יכול לעבור על רשימת הלינקים ולמצוא את התז המתאים
        if (links00 != null)
        {
            foreach (var link in links00)
            {
                int i = link.Id;
                if (currentAssignment.IdAssignment != i)
                {
                    links2.Add(link);
                }
                // כאן תוכלי להשתמש בתז כרצונך
            }
        }


        // var filteredAssignments = allAssignments.Where(a =>
        //  a.Id != currentAssignment.IdAssignment
        //);
        var filteredAssignments = allAssignments
       .Where(a => a.Id != currentAssignment.IdAssignment)
       .Concat(links2);
        return filteredAssignments;
    }

    private bool anyPreviousAssignmemts(BO.Assignment assignment)
    {
        List<AssignmentInList>? Links = assignment.Links;
        if (Links == null)
            return true;
        //go through all the previous tasks and checks that they have already been completed
        foreach (AssignmentInList link in Links)
        {
            BO.Assignment currentAssignment = s_bl.Assignment.Read(link.Id)!;
            if (currentAssignment.DateFinish > s_bl.Clock)
                return false;
        }
        return true;
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
        try
        {
            currentAssignment.Links = _selectedAssignments;

            if ((sender as Button).Content.ToString() == "Update")
            {
                s_bl.Assignment.Update(currentAssignment);

                // Create a new HashSet to hold the remaining tasks
                //var remainingTasks = new HashSet<BO.AssignmentInList>(_assignmentSet);

                // Remove selected tasks from the set of remaining tasks
                foreach (var assignmentCheckBox in _selectedAssignments)
                {
                    Assignments.Remove(assignmentCheckBox);
                }

                // Update Assignments with the remaining tasks
                //Assignments = new ObservableCollection<AssignmentInList>(remainingTasks);

                MessageBox.Show("The Update was successful");
                //s_bl.Assignment.Update(currentAssignment);
                //foreach (var assignmentCheckBox in _selectedAssignments) _assignmentSet.Remove(assignmentCheckBox);
                //Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet); //    _selectedAssignments
                //MessageBox.Show("The Update was successfull");
                // Clear the Assignments collection
                //Assignments.Clear();

                // Add the remaining items from _assignmentSet to Assignments
                //foreach (var assignment in _assignmentSet)
                //{
                //    Assignments.Add(assignment);
                //}
            }
            else
            {
                int? id = s_bl.Assignment.Create(currentAssignment);
                foreach (var assignmentCheckBox in _selectedAssignments)
                    Assignments.Remove(assignmentCheckBox);
                //Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet); //_assignmentSet
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
        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את המזהה מה־Content שלה
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
        IEnumerable<BO.AssignmentInList> filteredList = Assignments.Where
            (assignment => assignment.AssignmentName.ToLower().Contains(searchText));

        lview.ItemsSource = filteredList;
    }
    private string GetAssignmentName(CheckBox checkBox)
    {
        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את השם מה־Content שלה
        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
        if (assignmentInList != null)
        {
            return assignmentInList.AssignmentName;
        }
        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
        return string.Empty; // ניתן לשנות את הערך לפי הצורך
    }

    private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }


    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var task = (sender as FrameworkElement)!.DataContext as BO.AssignmentInList;

        //if (checkBox?.IsChecked is var value) _selectedAssignments.Add(task!);
        //else
        //    _selectedAssignments.Remove(task);

        if (checkBox?.IsChecked is var value && task != null)
        {
            if (value!.Value)
            {
                _selectedAssignments.Add(task);

                // Update the visual appearance of the ListViewItem to indicate that the task is linked
                //UpdateListViewItemAppearance(checkBox!, true);
            }
            else
            {
                _selectedAssignments.Remove(task);
                // Update the visual appearance of the ListViewItem to indicate that the task is not linked
                //UpdateListViewItemAppearance(checkBox!, false);
            }
            //checkBox!.IsChecked = _selectedAssignments.Contains(task);

        }
    }

    private static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
    {
        while (obj != null)
        {
            if (obj is T parent)
            {
                return parent;
            }
            obj = VisualTreeHelper.GetParent(obj);
        }
        return null;
    }
}















//private void UpdateListViewItemAppearance(CheckBox checkBox, bool isLinked)
//{
//    // Find the parent ListViewItem of the CheckBox
//    ListViewItem item = FindVisualParent<ListViewItem>(checkBox);
//    if (item != null)
//    {
//        // Update the visual appearance based on whether the task is linked or not
//        if (isLinked)
//        {
//            item.Background = Brushes.LightGray;
//        }
//        else
//        {
//            // Restore the original background color of the ListViewItem
//            item.Background = Brushes.Transparent;
//        }
//    }
//}






















































































































//using BO;
//using DalApi;
//using DO;
//using MaterialDesignThemes.Wpf;
//using PL.Worker;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using static PL.Assignment.AssignmentWindow;

//namespace PL.Assignment;

///// <summary>
///// Interaction logic for AssignmentsWindow.xaml
///// </summary>
//public partial class AssignmentWindow : Window
//{
//    //List<CheckBox> AssignmentDetailsLCBox = new();
//    // List<CheckBox> lCBox = new List<CheckBox>();

//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public DO.Level level { get; set; } = DO.Level.None;




//    public BO.Assignment Assignment
//    {
//        get { return (BO.Assignment)GetValue(AssignmentProperty); }
//        set { SetValue(AssignmentProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty AssignmentProperty =
//        DependencyProperty.Register("Assignment", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));


//    public ObservableCollection<BO.AssignmentInList> Assignments
//    {
//        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(AssignmentsProperty); }
//        set { SetValue(AssignmentsProperty, value); }
//    }



//    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty AssignmentsProperty =
//        DependencyProperty.Register("Assignments", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));

//    private HashSet<BO.AssignmentInList> _assignmentSet => Assignments.ToHashSet();
//    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
//    //public static readonly DependencyProperty List1AssignmentsProperty =
//    //    DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));

//    //public BO.Assignment currentAssignment
//    //{
//    //    get
//    //    {
//    //        var value = GetValue(AssignmentsListProperty);
//    //        return value != null ? (BO.Assignment)value : new BO.Assignment() { IdAssignment = -1 };
//    //    }
//    //    set { SetValue(AssignmentsListProperty, value); }
//    //}
//    public BO.Assignment currentAssignment
//    {
//        get { return (BO.Assignment)GetValue(AssignmentsListProperty); }
//        set { SetValue(AssignmentsListProperty, value); }
//    }

//    public static readonly DependencyProperty AssignmentsListProperty =
//    DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow)*/




//    public Visibility AssignmentDetailsVisibility
//    {
//        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
//        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
//    }
//    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
//    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(AssignmentWindow), new PropertyMetadata(Visibility.Collapsed));

//    //private List<BO.AssignmentInList> _selectedAssignments = new();
//    private HashSet<int> _selectedAssignments
//    {
//        get { return (HashSet<int>)GetValue(SelectedAssignmentsProperty); }
//        set { SetValue(SelectedAssignmentsProperty, value); }
//    }










//    // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty SelectedAssignmentsProperty =
//        DependencyProperty.Register(nameof(_selectedAssignments), typeof(HashSet<int>), typeof(AssignmentWindow), new PropertyMetadata(null));

//    public IEnumerable<BO.Worker> Workers
//    {
//        get { return (IEnumerable<BO.Worker>)GetValue(WorkerInTProperty); }
//        set { SetValue(WorkerInTProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for Workers.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty WorkerInTProperty =
//        DependencyProperty.Register("Workers", typeof(IEnumerable<BO.Worker>), typeof(AssignmentWindow), new PropertyMetadata(null));






//    //private bool isLinked;
//    //public bool IsLinked
//    //{
//    //    get { return isLinked; }
//    //    set
//    //    {
//    //        isLinked = value;
//    //        OnPropertyChanged(nameof(IsLinked));
//    //    }
//    //}

//    //// Add OnPropertyChanged method for INotifyPropertyChanged
//    //public event PropertyChangedEventHandler PropertyChanged;
//    //protected virtual void OnPropertyChanged(string propertyName)
//    //{
//    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    //}
//    //private List<BO.AssignmentInList> _selectedAssignments = new();

//    public AssignmentWindow(int id = 0)
//    {
//        InitializeComponent();

//        try
//        {
//            //_selectedAssignments = new HashSet<int>();
//            _selectedAssignments = new HashSet<int>();
//            var isUpdate = id != 0;

//            Assignment = isUpdate ? s_bl!.Assignment.Read(id)! : new BO.Assignment() { Links = new() };

//            _selectedAssignments = Assignment.Links!.Select(task => task.Id).ToHashSet();

//            Assignments = new ObservableCollection<AssignmentInList>(s_bl?.Assignment.ReadAll(t => t.Id != id &&
//            !_selectedAssignments.Contains(t.Id))!);

//            _selectedAssignments.Clear();
//            //Workers = s_bl?.Worker.ReadAll(worker => s_bl.Task.IsTaskCanBeAssigntToWorker(worker, Task))!;
//        }
//        catch (BO.Exceptions.BlDoesNotExistException ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//            this.Close();
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//            this.Close();
//        }

//    }



//    //public AssignmentWindow(int id = 0)
//    //{
//    //    InitializeComponent();
//    //    //List<CheckBox> lCBox = new List<CheckBox>();

//    //    CheckBox checkBox;
//    //    //currentAssignment = s_bl.Assignment.Read(id)!;

//    //    //Assignments = new ObservableCollection<AssignmentInList>(s_bl.Assignment.ReadAll());

//    //    //lview.ItemsSource= lCBox;
//    //    try
//    //    {
//    //        //var A = currentAssignment.Links.ToList();
//    //        // נניח שיש לך אובייקט assignment מסוג AssignmentInList



//    //        //sp.Visibility = Visibility.Collapsed;
//    //        //currentAssignment = (id != 0) ? new List<BO.Assignment>() { s_bl.Assignment.Read(id)!} : new List<BO.Assignment>() { new BO.Assignment() {IdAssignment=0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null } };
//    //        currentAssignment = (id != 0) ? s_bl.Assignment.Read(id)! : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null, LevelAssignment = DO.Level.None };
//    //        if (currentAssignment != null && currentAssignment.IdAssignment != 0)
//    //            txtId.IsEnabled = false;
//    //        //var allAssignments = s_bl.Assignment.ReadAll().ToList(); // Read all assignments
//    //        //var assignmentsExceptCurrent = allAssignments.Where(a => a.Id != currentAssignment!.IdAssignment);
//    //        //Assignments = new ObservableCollection<AssignmentInList>(assignmentsExceptCurrent);
//    //        //// Exclude the current assignment

//    //        //Create ObservableCollection from the filtered assignments
//    //        var allAssignments = s_bl.Assignment.ReadAll().ToList(); // קריאת כל המשימות
//    //        Assignments = new ObservableCollection<AssignmentInList>(FilterAssignmentsToShow(allAssignments, currentAssignment!)); // יצירת ObservableCollection מהמשימות הסוננות

//    //        Status s = Tools.GetProjectStatus();
//    //        if (s == BO.Status.Unscheduled || s == BO.Status.Scheduled)
//    //        {
//    //            //sp.Visibility = Visibility.Visible;
//    //            AssignmentDetailsVisibility = Visibility.Visible;

//    //        }
//    //        else
//    //            AssignmentDetailsVisibility = Visibility.Collapsed;
//    //    }
//    //    catch (BO.Exceptions.BlDoesNotExistException ex)
//    //    {
//    //        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    //    }
//    //}

//    private IEnumerable<AssignmentInList> FilterAssignmentsToShow(IEnumerable<AssignmentInList> allAssignments, BO.Assignment currentAssignment)
//    {
//        List<AssignmentInList> links1 = currentAssignment.Links!;
//        List<AssignmentInList> links2 = new List<AssignmentInList>();
//        // אתה יכול לעבור על רשימת הלינקים ולמצוא את התז המתאים
//        if (links1 != null)
//        {
//            foreach (var link in links1)
//            {
//                int i = link.Id;
//                if (currentAssignment.IdAssignment != i)
//                {
//                    links2.Add(link);
//                }
//                // כאן תוכלי להשתמש בתז כרצונך
//            }
//        }


//        // var filteredAssignments = allAssignments.Where(a =>
//        //  a.Id != currentAssignment.IdAssignment
//        //);
//        var filteredAssignments = allAssignments
//       .Where(a => a.Id != currentAssignment.IdAssignment)
//       .Concat(links2);



//        return filteredAssignments;
//    }

//    public BO.Assignment currentAssignmentData
//    {
//        get { return currentAssignment; }
//        set { currentAssignment = value; }
//    }
//    private void txtId_TextChanged(object sender, TextChangedEventArgs e)
//    {

//    }

//    private void txtName_TextChanged(object sender, TextChangedEventArgs e)
//    {

//    }



//    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
//    {
//        //try
//        //{

//            //    currentAssignment.Links = _selectedAssignments;

//            //    if ((sender as Button).Content.ToString() == "Update")
//            //    {
//            //        s_bl.Assignment.Update(currentAssignment);

//            //        // Create a new HashSet to hold the remaining tasks
//            //        var remainingTasks = new HashSet<BO.AssignmentInList>(_assignmentSet);

//            //        // Remove selected tasks from the set of remaining tasks
//            //        foreach (var assignmentCheckBox in _selectedAssignments)
//            //        {
//            //            remainingTasks.Remove(assignmentCheckBox);
//            //        }

//            //        // Update Assignments with the remaining tasks
//            //        Assignments = new ObservableCollection<AssignmentInList>(remainingTasks);

//            //        MessageBox.Show("The Update was successful");
//            //        //s_bl.Assignment.Update(currentAssignment);
//            //        //foreach (var assignmentCheckBox in _selectedAssignments) _assignmentSet.Remove(assignmentCheckBox);
//            //        //Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet); //    _selectedAssignments
//            //        //MessageBox.Show("The Update was successfull");
//            //        // Clear the Assignments collection
//            //        //Assignments.Clear();

//            //        // Add the remaining items from _assignmentSet to Assignments
//            //        //foreach (var assignment in _assignmentSet)
//            //        //{
//            //        //    Assignments.Add(assignment);
//            //        //}
//            //    }
//            //    else
//            //    {
//            //        int? id = s_bl.Assignment.Create(currentAssignment);
//            //        foreach (var assignmentCheckBox in _selectedAssignments) _assignmentSet.Remove(assignmentCheckBox);
//            //        Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet); //_assignmentSet
//            //        MessageBox.Show("The Addition was made successfully");
//            //    }
//            //    this.Close();
//            //}
//            //catch (BO.Exceptions.BlDoesNotExistException ex)
//            //{
//            //    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//            //}
//            //catch (Exception ex)
//            //{
//            //    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//            //}

//            if ((sender as Button)!.Content.ToString() == "Add")
//            {
//                try
//                {
//                    int? id = s_bl.Assignment.Create(Assignment!);
//                    MessageBox.Show($"Task {id} was successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                    this.Close();
//                }
//                catch (BO.Exceptions.BlAlreadyExistsException ex)
//                {
//                    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                }

//            }
//            else//update
//            {
//                try
//                {
//                    s_bl.Assignment.Update(Assignment!);
//                    MessageBox.Show($"Task {Assignment?.IdAssignment} was successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                    this.Close();
//                }
//                catch (BO.Exceptions.BlDoesNotExistException ex)
//                {
//                    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//                }
//            }













































































































//    }
//    private void cmbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {
//        //Level l = enum.TryParse(((ComboBox) sender).Text;
//        //currentWorker = (level== DO.Level.None) ?
//        //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
//    }

//    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {

//    }


//    private int GetAssignmentId(CheckBox checkBox)
//    {
//        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את המזהה מה־Content שלה
//        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
//        if (assignmentInList != null)
//        {
//            return assignmentInList.Id;
//        }
//        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
//        return -1; // ניתן לשנות את הערך לפי הצורך
//    }
//    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
//    {
//        string searchText = txtSearch.Text.ToLower(); // Convert search text to lowercase for case-insensitive search
//        IEnumerable<BO.AssignmentInList> filteredList = Assignments.Where
//            (assignment => assignment.AssignmentName.ToLower().Contains(searchText));

//        lview.ItemsSource = filteredList;
//    }
//    private string GetAssignmentName(CheckBox checkBox)
//    {
//        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את השם מה־Content שלה
//        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
//        if (assignmentInList != null)
//        {
//            return assignmentInList.AssignmentName;
//        }
//        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
//        return string.Empty; // ניתן לשנות את הערך לפי הצורך
//    }

//    private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {

//    }


//    private void CheckBox_Checked(object sender, RoutedEventArgs e)
//    {
//        var checkBox = sender as CheckBox;
//        var task = (sender as FrameworkElement)!.DataContext as BO.AssignmentInList;

//        //if (checkBox?.IsChecked is var value) _selectedAssignments.Add(task!);
//        //else
//        //    _selectedAssignments.Remove(task);

//        if (checkBox?.IsChecked is var value && task != null)
//        {
//            if (value!.Value)
//            {
//                _selectedAssignments.Add(task.Id);

//                // Update the visual appearance of the ListViewItem to indicate that the task is linked
//                //UpdateListViewItemAppearance(checkBox!, true);
//            }
//            else
//            {
//                _selectedAssignments.Remove(task.Id);
//                // Update the visual appearance of the ListViewItem to indicate that the task is not linked
//                //UpdateListViewItemAppearance(checkBox!, false);
//            }
//            //checkBox!.IsChecked = _selectedAssignments.Contains(task);

//        }
//    }

//    //private void UpdateListViewItemAppearance(CheckBox checkBox, bool isLinked)
//    //{
//    //    // Find the parent ListViewItem of the CheckBox
//    //    ListViewItem item = FindVisualParent<ListViewItem>(checkBox);
//    //    if (item != null)
//    //    {
//    //        // Update the visual appearance based on whether the task is linked or not
//    //        if (isLinked)
//    //        {
//    //            item.Background = Brushes.LightGray;
//    //        }
//    //        else
//    //        {
//    //            // Restore the original background color of the ListViewItem
//    //            item.Background = Brushes.Transparent;
//    //        }
//    //    }
//    //}

//    private static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
//    {
//        while (obj != null)
//        {
//            if (obj is T parent)
//            {
//                return parent;
//            }
//            obj = VisualTreeHelper.GetParent(obj);
//        }
//        return null;
//    }
//}























/*public partial class WorkerWindow : Window{//רשימה שיודעת לדווח למי שמקושר אליה שנוספו אברים//לא יכול להיות  ליסט כי אז הוא יתקשר רק חדפ ואני רוצה שכל פעם זה יתעדכן מהגרפיקה לכאןstatic readonly BlApi.IBl s_bl = BlApi.Factory.Get();public BO.Worker currentWorker{    get { return (BO.Worker)GetValue(WorkerListProperty); }    set { SetValue(WorkerListProperty, value); }}//יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהוpublic static readonly DependencyProperty WorkerListProperty =    DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));//currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.Nonepublic WorkerWindow(int id = 0){    InitializeComponent();    try    {        currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None };        if (currentWorker!=null && currentWorker.Id != 0)            txtId.IsEnabled = false;    }    catch(BO.Exceptions.BlDoesNotExistException ex)    {        MessageBox.Show(ex.Message,"Operation Faild",MessageBoxButton.OK,MessageBoxImage.Exclamation);    }    catch(Exception ex)    {        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);    }}private void btnAddUpdate_Click(object sender, RoutedEventArgs e){    try    {        if ((sender as Button).Content.ToString() == "Update")        {            s_bl.Worker.Update(currentWorker);            MessageBox.Show("The Update was successfull");        }        else        {            int? id = s_bl.Worker.Create(currentWorker);            MessageBox.Show("The Addition was made successfully");        }        this.Close();    }    catch (BO.Exceptions.BlDoesNotExistException ex)    {        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);    }    catch (Exception ex)    {        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);    }}private void cmbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e){    //Level l= enum.TryParse(((ComboBox) sender).Text;    //currentWorker = (level== DO.Level.None) ?    //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;}private void txtId_TextChanged(object sender, TextChangedEventArgs e){    if(!int.TryParse(((TextBox)sender).Text, out int id))    {        ((TextBox)sender).IsEnabled = false;    }    }}* */




/*  public static readonly DependencyProperty ListAssignmentsProperty = DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));  */


//using BO;
//using DO;
//using MaterialDesignThemes.Wpf;
//using PL.Worker;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using static PL.Assignment.AssignmentWindow;

//namespace PL.Assignment;

///// <summary>
///// Interaction logic for AssignmentsWindow.xaml
///// </summary>
//public partial class AssignmentWindow : Window
//{
//    //List<CheckBox> AssignmentDetailsLCBox = new();
//   // List<CheckBox> lCBox = new List<CheckBox>();

//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public DO.Level level { get; set; } = DO.Level.None;

//    public ObservableCollection<BO.AssignmentInList> Assignments
//    {
//        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(List1AssignmentsProperty); }
//        set { SetValue(List1AssignmentsProperty, value); }
//    }

//    private HashSet<BO.AssignmentInList> _assignmentSet => Assignments.ToHashSet();
//    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
//    public static readonly DependencyProperty List1AssignmentsProperty =
//        DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));

//    public BO.Assignment currentAssignment
//    {
//        get { return (BO.Assignment)GetValue(AssignmentsListProperty); }
//        set { SetValue(AssignmentsListProperty, value); }
//    }

//    public static readonly DependencyProperty AssignmentsListProperty =
//    DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow)*/

//    public Visibility AssignmentDetailsVisibility
//    {
//        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
//        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
//    }
//    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
//    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(AssignmentWindow), new PropertyMetadata(Visibility.Collapsed));

//    private List<BO.AssignmentInList> _selectedAssignments = new();
//    public AssignmentWindow(int id = 0)
//    {
//        InitializeComponent();
//        //List<CheckBox> lCBox = new List<CheckBox>();

//        CheckBox checkBox;

//        Assignments = new ObservableCollection<AssignmentInList>(s_bl.Assignment.ReadAll());


//        //lview.ItemsSource= lCBox;
//        //

//        try
//        {
//            Status s = Tools.GetProjectStatus();
//            if (s == BO.Status.Unscheduled || s == BO.Status.Scheduled)
//            {
//                //sp.Visibility = Visibility.Visible;
//                AssignmentDetailsVisibility = Visibility.Visible;

//            }
//            else
//                AssignmentDetailsVisibility = Visibility.Collapsed;

//            //sp.Visibility = Visibility.Collapsed;

//            //currentAssignment = (id != 0) ? new List<BO.Assignment>() { s_bl.Assignment.Read(id)!} : new List<BO.Assignment>() { new BO.Assignment() {IdAssignment=0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null } };
//            currentAssignment = (id != 0) ? s_bl.Assignment.Read(id) : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null, LevelAssignment = DO.Level.None };
//            if (currentAssignment != null && currentAssignment.IdAssignment != 0)
//                txtId.IsEnabled = false;
//        }
//        catch (BO.Exceptions.BlDoesNotExistException ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//        }
//    }
//    public BO.Assignment currentAssignmentData
//    {
//        get { return currentAssignment; }
//        set { currentAssignment = value; }
//    }
//    private void txtId_TextChanged(object sender, TextChangedEventArgs e)
//    {

//    }

//    private void txtName_TextChanged(object sender, TextChangedEventArgs e)
//    {

//    }



//    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
//    {
//        try
//        {
//            currentAssignment.Links = _selectedAssignments;

//            if ((sender as Button).Content.ToString() == "Update")
//            {
//                s_bl.Assignment.Update(currentAssignment);

//                foreach (var assignmentCheckBox in _selectedAssignments) _assignmentSet.Remove(assignmentCheckBox);
//                Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet);// _selectedAssignments
//                MessageBox.Show("The Update was successfull");
//            }
//            else
//            {
//                int? id = s_bl.Assignment.Create(currentAssignment);
//                MessageBox.Show("The Addition was made successfully");
//            }
//            this.Close();
//        }
//        catch (BO.Exceptions.BlDoesNotExistException ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//        }
//    }

//    private void cmbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {
//        //Level l = enum.TryParse(((ComboBox) sender).Text;
//        //currentWorker = (level== DO.Level.None) ?
//        //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
//    }

//    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {

//    }


//    private int GetAssignmentId(CheckBox checkBox)
//    {
//        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את המזהה מה־Content שלה
//        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
//        if (assignmentInList != null)
//        {
//            return assignmentInList.Id;
//        }
//        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
//        return -1; // ניתן לשנות את הערך לפי הצורך
//    }
//    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
//    {
//        string searchText = txtSearch.Text.ToLower(); // Convert search text to lowercase for case-insensitive search
//        IEnumerable<BO.AssignmentInList> filteredList = Assignments.Where
//            (assignment => assignment.AssignmentName.ToLower().Contains(searchText));

//        lview.ItemsSource = filteredList;
//    }

//    private string GetAssignmentName(CheckBox checkBox)
//    {
//        // במידה והתיבה מכילה את המידע בפורמט Assignment, יש להוציא את השם מה־Content שלה
//        AssignmentInList assignmentInList = checkBox.Content as AssignmentInList;
//        if (assignmentInList != null)
//        {
//            return assignmentInList.AssignmentName;
//        }
//        // אם לא ניתן להשיג את המידע מהתיבה, יש להחזיר ערך ברירת המחדל
//        return string.Empty; // ניתן לשנות את הערך לפי הצורך
//    }

//    private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    {

//    }

//    private void CheckBox_Checked(object sender, RoutedEventArgs e)
//    {
//        var checkBox = sender as CheckBox;
//        var task = (sender as FrameworkElement)!.DataContext as BO.AssignmentInList;

//        if (checkBox?.IsChecked is var value) _selectedAssignments.Add(task!);
//        else _selectedAssignments.Remove(task);



//    }
//}























///*
//public partial class WorkerWindow : Window
//{
////רשימה שיודעת לדווח למי שמקושר אליה שנוספו אברים
////לא יכול להיות  ליסט כי אז הוא יתקשר רק חדפ ואני רוצה שכל פעם זה יתעדכן מהגרפיקה לכאן

//static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//public BO.Worker currentWorker
//{
//    get { return (BO.Worker)GetValue(WorkerListProperty); }
//    set { SetValue(WorkerListProperty, value); }
//}
////יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
//public static readonly DependencyProperty WorkerListProperty =
//    DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));


////currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None

//public WorkerWindow(int id = 0)
//{
//    InitializeComponent();
//    try
//    {
//        currentWorker = (id != 0) ? s_bl.Worker.Read(id) : new BO.Worker() { Id = 0, Name = " ", Email = " ", Experience = DO.Level.None };
//        if (currentWorker!=null && currentWorker.Id != 0)
//            txtId.IsEnabled = false;
//    }
//    catch(BO.Exceptions.BlDoesNotExistException ex)
//    {
//        MessageBox.Show(ex.Message,"Operation Faild",MessageBoxButton.OK,MessageBoxImage.Exclamation);
//    }
//    catch(Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Operation Faild", MessageBoxButton.OK, MessageBoxImage.Exclamation);
//    }
//}

//private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
//{
//    try
//    {
//        if ((sender as Button).Content.ToString() == "Update")
//        {
//            s_bl.Worker.Update(currentWorker);
//            MessageBox.Show("The Update was successfull");
//        }
//        else
//        {
//            int? id = s_bl.Worker.Create(currentWorker);
//            MessageBox.Show("The Addition was made successfully");
//        }
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

//private void cmbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    //Level l= enum.TryParse(((ComboBox) sender).Text;
//    //currentWorker = (level== DO.Level.None) ?
//    //    s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.Experience == level)!;
//}

//private void txtId_TextChanged(object sender, TextChangedEventArgs e)
//{
//    if(!int.TryParse(((TextBox)sender).Text, out int id))
//    {
//        ((TextBox)sender).IsEnabled = false;
//    }

//}
//}

//* */




///*//  public static readonly DependencyProperty ListAssignmentsProperty =
     // DependencyProperty.Register("currentWorker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));
     //  */

////public static readonly DependencyProperty ListAssignmentsProperty =
////   DependencyProperty.Register("currentAssignment", typeof(BO.Assignment), typeof(AssignmentsWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow)*/
////public DO.Level level { get; set; } = DO.Level.None;
////public IEnumerable<BO.Assignment> currentAssignment
////{
////    get { return (IEnumerable<BO.Assignment>)GetValue(ListAssignmentsProperty); }
////    set { SetValue(ListAssignmentsProperty, value); }
////} public DO.Level level { get; set; } = DO.Level.None;
///
/* *    private IEnumerable<AssignmentInList> FilterAssignmentsToShow(IEnumerable<AssignmentInList> allAssignments, BO.Assignment currentAssignment)    {        // סינון המשימות הנותרות לתצוגה        foreach (var ass in allAssignments)        {            BO.Assignment linkAss = s_bl.Assignment.Read(a => a.IdAssignment == ass.Id && a. != currentAssignment.Links);            var filteredAssignments = allAssignments.Where(a =>        a.Id != currentAssignment.IdAssignment &&        a.Links != currentAssignment.Links        //a.IdWorker == 0         );        }        return filteredAssignments;    } * */