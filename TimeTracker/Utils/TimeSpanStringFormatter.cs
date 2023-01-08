using System;

namespace TimeTracker.Utils;

public class TimeSpanStringFormatter
{
    /**
     * Return a string in "hh:mm" format but with "hh" as total hours and not only the hours of the TimeSpan
     * object. The "normal" hours attribute only accepts 0 to 23 and no values above.
     *
     * See: https://stackoverflow.com/questions/38587552/use-timespan-to-add-intervals-over-24-hours
     */
    public static string FormatTotalHourAndMinutes(TimeSpan timeSpan)
    {
        // Truncate the hours to get the correct rounded hours.
        // This especially applies when having set the time to 30 minutes or more since the default rounding
        // would round to the next hour then. E.g. 10:00 - 13:30 would return the hours 4 since the total hours
        // is 3.5 which will be rounded to "04" by formatting the "hh" part of the string.
        double hours = Math.Truncate(timeSpan.TotalHours);
        
        return $"{hours:00}:{timeSpan.Minutes:00}";
    }
}