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
