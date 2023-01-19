using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using TimeTracker.Models.Entities;
using TimeTracker.Utils;

namespace TimeTracker.Converters;

public class EntryGroupToTotalTimeSumConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Sum up the total time of each entry in the group.
        TimeSpan sum = TimeSpan.Zero;
        if (values[0] is ReadOnlyObservableCollection<Object> items)
        {
            foreach (var item in items)
            {
                if (item is Entry entry)
                {
                    sum = sum.Add(entry.TotalTime);
                }
            }
        }
        
        return TimeSpanStringFormatter.FormatTotalHourAndMinutes(sum);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
