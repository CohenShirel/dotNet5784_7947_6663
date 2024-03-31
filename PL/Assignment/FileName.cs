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
    public AssignmentWindow(int id = 0)
    {
        InitializeComponent();
        //List<CheckBox> lCBox = new List<CheckBox>();

        CheckBox checkBox;
        //currentAssignment = s_bl.Assignment.Read(id)!;

        //Assignments = new ObservableCollection<AssignmentInList>(s_bl.Assignment.ReadAll());

        //lview.ItemsSource= lCBox;
        //

        try
        {

//sp.Visibility = Visibility.Collapsed;
            //currentAssignment = (id != 0) ? new List<BO.Assignment>() { s_bl.Assignment.Read(id)!} : new List<BO.Assignment>() { new BO.Assignment() {IdAssignment=0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null } };
            currentAssignment = (id != 0) ? s_bl.Assignment.Read(id)! : new BO.Assignment() { IdAssignment = 0, DurationAssignment = 0, Name = " ", Description = " ", Remarks = " ", ResultProduct = " ", Links = null, LevelAssignment = DO.Level.None };
            if (currentAssignment != null && currentAssignment.IdAssignment != 0)
                txtId.IsEnabled = false;
            //var allAssignments = s_bl.Assignment.ReadAll().ToList(); // Read all assignments
            //var assignmentsExceptCurrent = allAssignments.Where(a => a.Id != currentAssignment.IdAssignment); // Exclude the current assignment
            //Assignments = new ObservableCollection<AssignmentInList>(assignmentsExceptCurrent); // Create ObservableCollection from the filtered assignments
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
        // סינון המשימות הנותרות לתצוגה
        var filteredAssignments = allAssignments.Where(a =>
        a.Id != currentAssignment.IdAssignment &&
        //a.Link != currentAssignment.Links
         a.IdWorker == 0

        );
        return filteredAssignments;
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
                var remainingTasks = new HashSet<BO.AssignmentInList>(_assignmentSet);

                // Remove selected tasks from the set of remaining tasks
                foreach (var assignmentCheckBox in _selectedAssignments)
                {
                    remainingTasks.Remove(assignmentCheckBox);
                }

                // Update Assignments with the remaining tasks
                Assignments = new ObservableCollection<AssignmentInList>(remainingTasks);

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

//Neomi Golkin טאבלט😌, [31/03/2024 14:16]
//XAML

//Neomi Golkin טאבלט😌, [31/03/2024 14:16]
//<StackPanel Visibility = "{Binding AssignmentDetailsVisibility, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType={x:Type local:AssignmentWindow}}}" Grid.Row="1">
//    <Label Content = "Search:" Grid.Column="0" Grid.Row="0" HorizontalAlignment="Left" Margin="3" VerticalAlignment="Top"/>

//    <TextBox x:Name="txtSearch" TextChanged="txtSearch_TextChanged" Margin="50,-24,0,0" VerticalAlignment="Top" HorizontalAlignment="Left" Width="100" Grid.Row="1"/>
//    <ListView MaxHeight = "330" x:Name="lview" ItemsSource="{Binding Assignments}" HorizontalContentAlignment="Stretch" Width="300">
//        <!--<ListView.ItemContainerStyle>
//    <Style TargetType = "ListViewItem" >
//        < Style.Triggers >
//            < DataTrigger Binding="{Binding IsLinked}" Value="True">
//                <Setter Property = "Background" Value="LightGray"/>
//            </DataTrigger>
//        </Style.Triggers>
//    </Style>
//</ListView.ItemContainerStyle>-->
//        <ListView.View>

//            <GridView>
//                <GridViewColumn>
//                    <GridViewColumn.CellTemplate>
//                        <DataTemplate>
//                            <StackPanel Orientation = "Horizontal" >
//                                < !--Check if the task is already linked, and if so, display "V" -->
//                                <!-- Display the task name -->
//                                <!-- Display the checkbox for selecting the task -->
//                                <CheckBox Checked = "CheckBox_Checked" />
//                            </ StackPanel >
//                            < !--< CheckBox Checked="CheckBox_Checked"></CheckBox>-->
//                        </DataTemplate>
//                    </GridViewColumn.CellTemplate>
//                </GridViewColumn>

//                <GridViewColumn>
//                    <GridViewColumn.CellTemplate>
//                        <DataTemplate>
//                            <TextBlock Text = "{Binding AssignmentName}" />
//                        </ DataTemplate >
//                    </ GridViewColumn.CellTemplate >
//                </ GridViewColumn >
//            </ GridView >
//        </ ListView.View >

//    </ ListView >
//</ StackPanel >