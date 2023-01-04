using System;
using System.Globalization;
using TimeTracker.Models.Entities;

namespace TimeTracker.Utils;

public static class WeekGroupUtil
{
    public static string GetWeekGroupName(Entry entry)
    {
        // Get the date for whose group should be computed.
        DateTime dateInWeek = entry.Start;
        DayOfWeek dayInWeek = dateInWeek.DayOfWeek;
        
        // Get the first day of week according to the current culture.
        DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        // Get the first and the last date of the week.
        DateTime startOfWeek = WeekGroupUtil.GetStartOfWeek(dateInWeek, firstDayOfWeek, dayInWeek);
        DateTime endOfWeek = WeekGroupUtil.GetEndOfWeek(startOfWeek);
        
        // Return the formatted string for the computed week.
        return $"{startOfWeek.ToShortDateString()} - {endOfWeek.ToShortDateString()}";
    }

    private static DateTime GetStartOfWeek(DateTime dateInWeek, DayOfWeek firstDayOfWeek, DayOfWeek dayInWeek)
    {
        int daysToSubtract = dayInWeek - firstDayOfWeek;
        return dateInWeek.AddDays(-daysToSubtract).Date;
    }
    
    private static DateTime GetEndOfWeek(DateTime startOfWeek)
    {
        return startOfWeek.AddDays(6).Date;
    }
}