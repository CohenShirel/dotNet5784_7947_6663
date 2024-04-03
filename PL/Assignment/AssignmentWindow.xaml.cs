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
    //List<CheckBox> AssignmentDetailsLCBox = new();
    // List<CheckBox> lCBox = new List<CheckBox>();

    static readonly BlApi.IBl _bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;

    public ObservableCollection<BO.AssignmentInList> Assignmentslst
    {
        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(LstAssignmentsProperty); }
        set { SetValue(LstAssignmentsProperty, value); }
    }
    public static readonly DependencyProperty LstAssignmentsProperty =
      DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));
    private HashSet<BO.AssignmentInList> assignmentSet => Assignmentslst.ToHashSet();
    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
 
    public BO.Assignment currentAss
    {
        get { return (BO.Assignment)GetValue(AssListProperty); }
        set { SetValue(AssListProperty, value); }
    }

    public static readonly DependencyProperty AssListProperty =
    DependencyProperty.Register("currentAss", typeof(BO.Assignment), typeof(AssignmentWindow), new PropertyMetadata(null));/*typeof(IEnumerable<BO.Assignment>), typeof(AssignmentListWindow)*/
    public Visibility AssDetailsVisibility
    {
        get { return (Visibility)GetValue(AssDetailsVisibilityProperty); }
        set { SetValue(AssDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssDetailsVisibilityProperty =
    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(AssignmentWindow), new PropertyMetadata(Visibility.Collapsed));

    private List<BO.AssignmentInList> selectedAssignments = new();
    public AssignmentWindow(int id = 0)
    {
        InitializeComponent();
        CheckBox checkBox;
        try
        {
            currentAss = (id != 0) ? _bl.Assignment.Read(id)! : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null, LevelAssignment = DO.Level.None };
            if (currentAss != null && currentAss.IdAssignment != 0)
                txtId.IsEnabled = false;
            var allAssignments = _bl.Assignment.ReadAll().ToList(); // Read all assignments
            var assignmentsExceptCurrent = allAssignments.Where(a => a.Id != currentAss.IdAssignment); // Exclude the current assignment
            Assignmentslst = new ObservableCollection<AssignmentInList>(assignmentsExceptCurrent); // Create ObservableCollection from the filtered assignments
            //var allAssignments = s_bl.Assignment.ReadAll().ToList(); // קריאת כל המשימות
            //Assignments = new ObservableCollection<AssignmentInList>(FilterAssignmentsToShow(allAssignments, currentAssignment!)); // יצירת ObservableCollection מהמשימות הסוננות

            Status s = Tools.GetProjectStatus();
            if (s == BO.Status.Unscheduled || s == BO.Status.Scheduled)
            {
                //sp.Visibility = Visibility.Visible;
                AssDetailsVisibility = Visibility.Visible;

            }
            else
                AssDetailsVisibility = Visibility.Collapsed;
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
        get { return currentAss; }
        set { currentAss = value; }
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
            currentAss.Links = selectedAssignments;

            if ((sender as Button).Content.ToString() == "Update")
            {
                _bl.Assignment.Update(currentAss);

                // Create a new HashSet to hold the remaining tasks
                var remainingTasks = new HashSet<BO.AssignmentInList>(assignmentSet);

                // Remove selected tasks from the set of remaining tasks
                foreach (var assignmentCheckBox in selectedAssignments)
                {
                    remainingTasks.Remove(assignmentCheckBox);
                }

                // Update Assignments with the remaining tasks
                Assignmentslst = new ObservableCollection<AssignmentInList>(remainingTasks);

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
                int? id = _bl.Assignment.Create(currentAss);
                foreach (var assignmentCheckBox in selectedAssignments) assignmentSet.Remove(assignmentCheckBox);
                Assignmentslst = new ObservableCollection<AssignmentInList>(assignmentSet); //_assignmentSet
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
        IEnumerable<BO.AssignmentInList> filteredList = Assignmentslst.Where
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
                selectedAssignments.Add(task);
                // Update the visual appearance of the ListViewItem to indicate that the task is linked
                //UpdateListViewItemAppearance(checkBox!, true);
            }
            else
            {
                selectedAssignments.Remove(task);
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