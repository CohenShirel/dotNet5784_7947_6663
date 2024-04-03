using BO;
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
public partial class AssignmentWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;
    private HashSet<BO.AssignmentInList> _assignmentSet => Assignments.ToHashSet();

    public ObservableCollection<BO.AssignmentInList> Assignments
    {
        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(List1AssignmentsProperty); }
        set { SetValue(List1AssignmentsProperty, value); }
    }

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
    public AssignmentWindow(int id = 0)
    {
        InitializeComponent();
        CheckBox checkBox;
        //currentAssignment = s_bl.Assignment.Read(id)!;

        //Assignments = new ObservableCollection<AssignmentInList>(s_bl.Assignment.ReadAll());
        
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

    //private IEnumerable<AssignmentInList> FilterAssignmentsToShow(IEnumerable<AssignmentInList> allAssignments, BO.Assignment currentAssignment)
    //{
    //    // סינון המשימות הנותרות לתצוגה
    //    foreach (var ass in allAssignments)
    //    {

    //        BO.Assignment linkAss = s_bl.Assignment.Read(a=>a.IdAssignment==ass.Id && a. != currentAssignment.Links);
                    

    //        var filteredAssignments = allAssignments.Where(a =>
    //    a.Id != currentAssignment.IdAssignment &&
    //    a.Links!= currentAssignment.Links
    //    //a.IdWorker == 0 

    //    );
    //    }
        
    //    return filteredAssignments;
    //}

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
                var remainingTasks = new HashSet<BO.AssignmentInList>(_assignmentSet);

                // Remove selected tasks from the set of remaining tasks
                foreach (var assignmentCheckBox in _selectedAssignments)
                {
                    remainingTasks.Remove(assignmentCheckBox);
                }

                // Update Assignments with the remaining tasks
                Assignments = new ObservableCollection<AssignmentInList>(remainingTasks);
                MessageBox.Show("The Update was successful");
            }
            else
            {
                int? id = s_bl.Assignment.Create(currentAssignment);
                foreach (var assignmentCheckBox in _selectedAssignments) _assignmentSet.Remove(assignmentCheckBox);
                Assignments = new ObservableCollection<AssignmentInList>(_assignmentSet); //_assignmentSet
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

    private void UpdateListViewItemAppearance(CheckBox checkBox, bool isLinked)
    {
        // Find the parent ListViewItem of the CheckBox
        ListViewItem item = FindVisualParent<ListViewItem>(checkBox);
        if (item != null)
        {
            // Update the visual appearance based on whether the task is linked or not
            if (isLinked)
            {
                item.Background = Brushes.LightGray;
            }
            else
            {
                // Restore the original background color of the ListViewItem
                item.Background = Brushes.Transparent;
            }
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