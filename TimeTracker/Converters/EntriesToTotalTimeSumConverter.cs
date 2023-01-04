﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using TimeTracker.Models.Entities;

namespace TimeTracker.Converters;

public class EntryToTotalTimeSumConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        TimeSpan sum = TimeSpan.Zero;
        
        // Add the total time of each entry to the sum.
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

        return sum.ToString("hh\\:mm");
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}