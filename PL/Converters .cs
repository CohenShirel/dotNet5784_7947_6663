using PL.Worker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PL;

internal class Converters{}
public class AssignmentSelectionConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return false;

        // השאילתא המקבלת את רשימת המשימות שנבחרו ומשימת המשימות בחלון
        var selectedTasks = values[0] as IEnumerable<BO.AssignmentInList>;
        var allTasks = values[1] as IEnumerable<BO.AssignmentInList>;

        // ודא ששתי הרשימות אינן ריקות
        if (selectedTasks == null || allTasks == null)
            return false;

        // לולאה שתבדוק האם כל משימה מרשימת המשימות קיימת ברשימת המשימות שנבחרו
        foreach (var task in allTasks)
        {
            if (selectedTasks.Any(selectedTask => selectedTask.Id == task.Id))
                return true;
        }

        return false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class BooleanToSymbolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // קבלת ערך בוליאני מהמודל
        bool isLinked = (bool)value;

        // אם המשימה קשורה, החזר את הסימון שבחרת (לדוגמה "V")
        // אחרת, החזר מחרוזת ריקה
        return isLinked ? "V" : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
public class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class VisibilityContent : DependencyObject
{
    public Visibility AssignmentDetailsVisibility
    {
        get { return (Visibility)GetValue(AssignmentDetailsVisibilityProperty); }
        set { SetValue(AssignmentDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AssignmentDetailsVisibilityProperty =
    DependencyProperty.Register("AssignmentDetailsVisibility", typeof(Visibility), typeof(CurrentWorkerWindow), new PropertyMetadata(Visibility.Collapsed));
}

public class NameValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string name = value as string;
        if (string.IsNullOrEmpty(name))
        {
            return new ValidationResult(false, "שדה זה הינו שדה חובה.");
        }
        else if (name.Length < 2)
        {
            return new ValidationResult(false, "השם חייב להכיל לפחות שני תווים.");
        }
        else
        {
            return ValidationResult.ValidResult;
        }
    }
}
public class WorkerViewModel : DependencyObject
{
    public Visibility AllAssignmentsDetailsVisibility
    {
        get { return (Visibility)GetValue(AllAssignmentsDetailsVisibilityProperty); }
        set { SetValue(AllAssignmentsDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AllAssignmentsDetailsVisibilityProperty =
 DependencyProperty.Register("AllAssignmentsDetailsVisibility", typeof(Visibility), typeof(CurrentWorkerWindow), new PropertyMetadata(Visibility.Collapsed));

    public WorkerViewModel(int id){}
}