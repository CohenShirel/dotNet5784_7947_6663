using BO;
using DalApi;
using DO;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace PL.Assignment;
public partial class AssignmentWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;

    public ObservableCollection<BO.AssignmentInList> Assignments
    {
        get { return (ObservableCollection<BO.AssignmentInList>)GetValue(List1AssignmentsProperty); }
        set { SetValue(List1AssignmentsProperty, value); }
    }

    private HashSet<BO.AssignmentInList> _assignmentSet => Assignments.ToHashSet();
    public static readonly DependencyProperty List1AssignmentsProperty =
        DependencyProperty.Register("Assignments", typeof(ObservableCollection<BO.AssignmentInList>), typeof(AssignmentWindow), new PropertyMetadata(null));
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
        List<AssignmentInList> links2 = new List<AssignmentInList>();
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
            if (currentAssignment.DateFinish > s_bl.Clock.GetStartProject())
                return false;
        }
        return true;
    }
    public BO.Assignment currentAssignmentData
    {
        get { return currentAssignment; }
        set { currentAssignment = value; }
    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            currentAssignment.Links = _selectedAssignments;

            if ((sender as Button).Content.ToString() == "Update")
            {
                s_bl.Assignment.Update(currentAssignment);
                foreach (var assignmentCheckBox in _selectedAssignments)
                {
                    Assignments.Remove(assignmentCheckBox);
                }
                MessageBox.Show("The Update was successful");
            }
            else
            {
                int? id = s_bl.Assignment.Create(currentAssignment);
                foreach (var assignmentCheckBox in _selectedAssignments)
                    Assignments.Remove(assignmentCheckBox);
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
        // Convert the search text to lowercase for case-insensitive search
        string searchText = txtSearch.Text.ToLower();

        // Filter the list of assignments based on the search text
        IEnumerable<BO.AssignmentInList> filteredList = Assignments.Where
            (assignment => assignment.AssignmentName.ToLower().Contains(searchText));

        // Set the filtered list as the ItemsSource of the ListView
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
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var task = (sender as FrameworkElement)!.DataContext as BO.AssignmentInList;
        if (checkBox?.IsChecked is var value && task != null)
        {
            if (value!.Value)
                _selectedAssignments.Add(task);
            else 
                _selectedAssignments.Remove(task);
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