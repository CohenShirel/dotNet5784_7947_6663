using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.Assignment;

public partial class AssignmentListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DO.Level level { get; set; } = DO.Level.None;
    public IEnumerable<BO.AssignmentInList> ListAssignments
    {
        get { return (IEnumerable<BO.AssignmentInList>)GetValue(ListAssignmentsProperty); }
        set { SetValue(ListAssignmentsProperty, value); }
    }
    //יודעת לדווח על הגרפיקה על קיומה...מדווחת על שינויים על כל הוספה או מחיקה של מישהו
    public static readonly DependencyProperty ListAssignmentsProperty =
        DependencyProperty.Register("ListAssignments", typeof(IEnumerable<BO.AssignmentInList>), typeof(AssignmentListWindow), new PropertyMetadata(null));
    public AssignmentListWindow()
    {
        InitializeComponent();
        ListAssignments = s_bl.Assignment.ReadAll();
    }
    private void Cmb_Levels(object sender, SelectionChangedEventArgs e)
    {
        ListAssignments = (level == DO.Level.None) ?
          s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
    }
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        new AssignmentWindow().ShowDialog();
        UpdateAssignmentsList();
        //we cant go back to previose window till we finish
    }

    private void ListView_UpdateClick(object sender, MouseButtonEventArgs e)
    {
        var ass = ((sender as ListView)?.SelectedItem as BO.AssignmentInList)!;
        new AssignmentWindow(ass!.Id).ShowDialog();
        UpdateAssignmentsList();
    }
    private void ListView_SelectionChanged(object sender, EventArgs e)
    {
        ListAssignments = (level == DO.Level.None) ?
          s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
    }
    private void UpdateAssignmentsList()
    {
        ListAssignments = (level == DO.Level.None) ?
         s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level).OrderBy(e => e.Id);
    }
    private void ComboBox_Selected(object sender, RoutedEventArgs e)
    {
        ListAssignments = (level == DO.Level.None) ?
         s_bl?.Assignment.ReadAll() : s_bl?.Assignment.ReadAll(item => item.LevelAssignment == level);
    }
}