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

internal class Converters
{
    

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


//public class NoBooleanToVisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        bool b = (bool)value;
//        if (b)
//        {
//            return Visibility.Visible;
//        }
//        else
//            return Visibility.Collapsed;    
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}
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
//public class NoBooleanToVisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        bool b = (bool)value;
//        if (b)
//        {
//            return Visibility.Collapsed;
//        }
//        else
//            return Visibility.Visible;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}
//public class BooleanToVisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is bool isVisible)
//        {
//            return isVisible ? Visibility.Visible : Visibility.Collapsed;
//        }
//        return Visibility.Collapsed;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is Visibility visibility)
//        {
//            return visibility == Visibility.Visible;
//        }
//        return false;
//    }
//}
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

        // ביצוע בדיקת האימות על שם המועבר
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

  
    //=====================Visibility
   

    public Visibility AllAssignmentsDetailsVisibility
    {
        get { return (Visibility)GetValue(AllAssignmentsDetailsVisibilityProperty); }
        set { SetValue(AllAssignmentsDetailsVisibilityProperty, value); }
    }
    public static readonly DependencyProperty AllAssignmentsDetailsVisibilityProperty =
 DependencyProperty.Register("AllAssignmentsDetailsVisibility", typeof(Visibility), typeof(CurrentWorkerWindow), new PropertyMetadata(Visibility.Collapsed));

    public WorkerViewModel(int id)
    {
        //Assignments = BlApi.Factory.Get().Assignment.ReadAllAss(ass => ass.LevelAssignment <= currentWorker.Experience && ass.IdWorker == default(int));
    }
}
////        <local:VisibilityConverter x:Key="VisibilityConverterKey"/>

//public class VisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value == null || parameter == null)
//            return Visibility.Collapsed;

//        string expectedId = parameter.ToString()!;
//        string currentId = value.ToString()!;

//        return currentId.Equals(expectedId, StringComparison.InvariantCultureIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
