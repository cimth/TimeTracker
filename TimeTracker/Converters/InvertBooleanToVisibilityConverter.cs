using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeTracker.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
public class InvertBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = (bool) value;
        bool invertedBool = !boolValue;
        return invertedBool ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}