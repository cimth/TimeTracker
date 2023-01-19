using System;
using System.Globalization;
using System.Windows.Data;
using TimeTracker.Utils;

namespace TimeTracker.Converters;

public class WeekGroupToIsExpandedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return WeekGroupUtil.IsCurrentWeek(str);
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}