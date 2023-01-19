using System;
using System.Globalization;
using System.Windows.Data;
using TimeTracker.Utils;

namespace TimeTracker.Converters;

public class TimeSpanToTotalHoursAndMinutesConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return TimeSpanStringFormatter.FormatTotalHourAndMinutes(timeSpan);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}