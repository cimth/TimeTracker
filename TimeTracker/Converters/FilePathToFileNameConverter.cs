using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace TimeTracker.Converters;

public class FilePathToFileNameConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string path)
        {
            return Path.GetFileName(path);
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}