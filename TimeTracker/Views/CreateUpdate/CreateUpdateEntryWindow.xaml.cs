using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace TimeTracker.Views.CreateUpdate;

public partial class CreateUpdateEntryWindow : Window
{
    // ==============
    // Initialization
    // ==============
    
    public CreateUpdateEntryWindow()
    {
        InitializeComponent();
    }
    
    // ==============
    // Focus text boxes
    // ==============

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        // Select complete input text when focused
        if (sender is TextBox tb)
        {
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }
    }
    
    // ==============
    // Select start date
    // ==============

    private void DtpStart_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Reset the blackout dates (= dates that are disallowed to select) so that a date in the past can be selected.
        // If not reset here, a possible disabled date would be selected which leads to an Exception.
        this.DtpEnd.BlackoutDates.Clear();
        
        // Get the selected start date.
        DateTime start = this.DtpStart.SelectedDate ?? DateTime.Today;

        // Set the same date as end date.
        this.DtpEnd.SelectedDate = this.DtpStart.SelectedDate;

        // Set the 1st of the current month as first displayed date.
        DateTime firstDayOfMonth = this.GetFirstDayOfMonth(start);
        this.DtpEnd.DisplayDateStart = firstDayOfMonth;

        // Do not allow to select a date before the start date.
        if (!start.Date.Equals(firstDayOfMonth.Date))
        {
            CalendarDateRange blackoutDates = this.GetDateRangeExcludingTo(firstDayOfMonth, start);
            this.DtpEnd.BlackoutDates.Add(blackoutDates);
        }
    }

    private DateTime GetFirstDayOfMonth(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
    }

    private CalendarDateRange GetDateRangeExcludingTo(DateTime from, DateTime to)
    {
        DateTime oneDayBeforeTo = to.Subtract(new TimeSpan(1, 0, 0, 0));
        CalendarDateRange range = new CalendarDateRange(from, oneDayBeforeTo);
        return range;
    }
}