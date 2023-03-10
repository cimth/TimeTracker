using System;
using System.Globalization;
using System.Runtime.Serialization;
using TimeTracker.Models.Entities;

namespace TimeTracker.Utils;

public static class WeekGroupUtil
{
    public static bool IsCurrentWeek(string weekGroupName)
    {
        // Get the first day of the given week group.
        string rawWeekGroupStart = weekGroupName.Substring(0, DateTimeFormatInfo.CurrentInfo.ShortDatePattern.Length);
        DateTime weekGroupStart = DateTime.Parse(rawWeekGroupStart);

        // Get the first day of the current week.
        DateTime today = DateTime.Today;
        DayOfWeek firstDayOfCurrentWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        DateTime currentWeekStart = WeekGroupUtil.GetStartOfWeek(today, firstDayOfCurrentWeek, today.DayOfWeek);
        
        // Return true if the week group is the current week.
        return weekGroupStart.Equals(currentWeekStart);
    }
    
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
        // Use modulo to get the correct days also if Monday and not Sunday is the first day of the week.
        int daysToSubtract = (dayInWeek - firstDayOfWeek + 7) % 7;
        return dateInWeek.AddDays(-daysToSubtract).Date;
    }
    
    private static DateTime GetEndOfWeek(DateTime startOfWeek)
    {
        return startOfWeek.AddDays(6).Date;
    }
}