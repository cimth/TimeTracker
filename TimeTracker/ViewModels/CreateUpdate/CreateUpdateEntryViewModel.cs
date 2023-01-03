using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TimeTracker.Dialog;
using TimeTracker.Models;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;

namespace TimeTracker.ViewModels.CreateUpdate;

public class CreateUpdateEntryViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============
    
    public string WindowTitle
    {
        get => this._windowTitle;
        set => SetField(ref this._windowTitle, value);
    }

    public string SubmitButtonText
    {
        get => this._submitButtonText;
        set => SetField(ref this._submitButtonText, value);
    }

    public List<Category> Categories { get; private set; } = null!;     // Not null after Initialize().
    
    /*
     * Input values.
     *
     * Note:
     * Use raw strings for time input to avoid a weird user experience when entering a time.
     */

    public Category? InputCategory
    {
        get => this._inputCategory;
        set => SetField(ref this._inputCategory, value);
    }

    public DateTime InputStartDate
    {
        get => this._inputStartDate;
        set => SetField(ref this._inputStartDate, value);
    }

    public DateTime InputEndDate
    {
        get => this._inputEndDate;
        set => SetField(ref this._inputEndDate, value);
    }

    public string InputStartTime
    {
        get => this._inputStartTime;
        set => SetField(ref this._inputStartTime, value);
    }

    public string InputEndTime
    {
        get => this._inputEndTime;
        set => SetField(ref this._inputEndTime, value);
    }

    public string InputPauseTime
    {
        get => this._inputTimePauseTime;
        set => SetField(ref this._inputTimePauseTime, value);
    }

    public string InputNotes
    {
        get => this._inputNotes;
        set => SetField(ref this._inputNotes, value);
    }

    /*
     * Input validation
     */
    
    public bool IsInputValid
    {
        get => this._isInputValid;
        set => SetField(ref this._isInputValid, value);
    }

    public bool IsInputStartTimeValid
    {
        get => this._isInputStartTimeValid;
        private set => SetField(ref this._isInputStartTimeValid, value);
    }
    
    public bool IsInputEndTimeValid
    {
        get => this._isInputEndTimeValid;
        private set => SetField(ref this._isInputEndTimeValid, value);
    }
    
    public bool IsInputPauseTimeValid
    {
        get => this._isInputPauseTimeValid;
        private set => SetField(ref this._isInputPauseTimeValid, value);
    }
    
    /*
     * The total time (readonly from the GUI)!
     */

    public string TotalTime
    {
        get => this._totalTime;
        private set => SetField(ref this._totalTime, value);
    }

    // ==============
    // Commands
    // ==============
    
    public ICommand SubmitCommand { get; }
    
    public ICommand UpdateStateAfterInputCommand { get; }
    
    // ==============
    // Fields
    // ==============

    private readonly EntryService _entryService;
    private readonly CategoryService _categoryService;
    private readonly DialogService _dialogService;
    
    private string _windowTitle = null!;            // Not null after Initialize().
    private string _submitButtonText = null!;       // Not null after Initialize().

    private Entry? _originalEntry;

    // Input values
    private Category? _inputCategory;
    
    private DateTime _inputStartDate;
    private DateTime _inputEndDate;

    private string _inputStartTime = null!;         // Not null after Initialize().
    private string _inputEndTime = null!;           // Not null after Initialize().
    private string _inputTimePauseTime = null!;     // Not null after Initialize().

    private string _inputNotes = null!;             // Not null after Initialize().

    // Input validation
    private bool _isInputValid;
    
    private bool _isInputStartTimeValid;
    private bool _isInputEndTimeValid;
    private bool _isInputPauseTimeValid;
    
    private string _totalTime = null!;              // Not null after Initialize().

    // ==============
    // Initialization
    // ==============

    public CreateUpdateEntryViewModel(DependencyManager dependencyManager)
    {
        this._entryService = dependencyManager.EntryService;
        this._categoryService = dependencyManager.CategoryService;
        this._dialogService = dependencyManager.DialogService;

        this.SubmitCommand = new DelegateCommand(this.Submit);
        this.UpdateStateAfterInputCommand = new DelegateCommand(this.UpdateStateAfterInput);
    }

    public void Initialize(Entry? entry = null)
    {
        // Save the original entry (might be null if a new entry should be created).
        // The GUI does not bind to the original entry but to separate variables for each input field
        // to avoid changing the original data even if the changes are not saved.
        this._originalEntry = entry;
        
        // Adjust the GUI texts depending on whether creating or updating an entry.
        this.WindowTitle = this._originalEntry == null ? "Create Entry" : "Update Entry";
        this.SubmitButtonText = this._originalEntry == null ? "Create" : "Save";
        
        // Initialize the data bound to the GUI.
        this.InitializeInput();
        this.InitializeCategories();
        this.UpdateStateAfterInput();
    }

    private void InitializeInput()
    {
        if (this._originalEntry == null)
        {
            this.InitializeCreateInput();
        }
        else
        {
            this.InitializeUpdateInput(this._originalEntry);
        }
    }
    
    private void InitializeCreateInput()
    {
        this.InputCategory = null;
            
        this.InputStartDate = DateTime.Today;
        this.InputStartTime = "00:00";
            
        this.InputEndDate = DateTime.Today;
        this.InputEndTime = "00:00";
            
        this.InputPauseTime = "00:00";

        this.InputNotes = "";
    }

    private void InitializeUpdateInput(Entry entry)
    {
        this.InputCategory = entry.Category;
            
        this.InputStartDate = entry.Start.Date;
        this.InputStartTime = entry.Start.TimeOfDay.ToString("hh\\:mm");

        this.InputEndDate = entry.End.Date;
        this.InputEndTime = entry.End.TimeOfDay.ToString("hh\\:mm");
            
        this.InputPauseTime = entry.Pause.ToString("hh\\:mm");

        this.InputNotes = entry.Notes;
    }

    private void InitializeCategories()
    {
        this.Categories = this._categoryService.ReadAll();
    }
    
    // ==============
    // Command actions
    // ==============

    private void UpdateStateAfterInput()
    {
        bool allTimeInputIsValid = this.ValidateTimeInput();
        this.UpdateTotalTime(allTimeInputIsValid);

        this.IsInputValid = this.InputCategory != null
                            && allTimeInputIsValid;
    }

    private void UpdateTotalTime(bool allTimeInputIsValid)
    {
        string newTotal = "--:--";
        
        if (allTimeInputIsValid)
        {
            TimeSpan start = this.GetTimeFromRawInput(this.InputStartTime);
            TimeSpan end = this.GetTimeFromRawInput(this.InputEndTime);
            TimeSpan pause = this.GetTimeFromRawInput(this.InputPauseTime);

            TimeSpan total = end.Subtract(start).Subtract(pause);
            
            newTotal = total.ToString("hh\\:mm");
        }

        this.TotalTime = newTotal;
    }

    private void Submit()
    {
        // Stop if the input is invalid.
        if (!this.IsInputValid)
        {
            return;
        }

        // Create / update the entry.
        if (this._originalEntry == null)
        {
            this.CreateEntry();
        }
        else
        {
            this.UpdateEntry();
        }
        
        // Close the dialog.
        this._dialogService.CloseCurrentDialog();
    }

    private void CreateEntry()
    {
        Entry entry = this.GetEntryFromInput();
        this._entryService.Create(entry);
    }

    private void UpdateEntry()
    {
        // Get the updated data and save them into the original entry.
        Entry updatedEntry = this.GetEntryFromInput();

        this._originalEntry!.Category = updatedEntry.Category;
        this._originalEntry!.Start = updatedEntry.Start;
        this._originalEntry!.End = updatedEntry.End;
        this._originalEntry!.Pause = updatedEntry.Pause;
        this._originalEntry!.Notes = updatedEntry.Notes;
        
        // Save the changes into the database.
        this._entryService.Update();
    }

    private Entry GetEntryFromInput()
    {
        // Convert the raw time input to actual time objects.
        TimeSpan startTime = GetTimeFromRawInput(this.InputStartTime);
        TimeSpan endTime = GetTimeFromRawInput(this.InputEndTime);
        TimeSpan pauseTime = GetTimeFromRawInput(this.InputPauseTime);
        
        // Create the DateTime objects for the entry.
        DateTime start = this.InputStartDate + startTime;
        DateTime end = this.InputEndDate + endTime;
        
        // Create and return the entry.
        return new Entry(this.InputCategory, start, end, pauseTime, this.InputNotes);
    }
    
    // ==============
    // Helping methods
    // ==============

    private bool ValidateTimeInput()
    {
        // Check the raw inputs.
        this.IsInputStartTimeValid = this.IsValidTime(this.InputStartTime);
        this.IsInputEndTimeValid = this.IsValidTime(this.InputEndTime);
        this.IsInputPauseTimeValid = this.IsValidTime(this.InputPauseTime);
        
        // Check if the end time is after the start time.
        // If not so, mark both as invalid.
        if (this.IsInputStartTimeValid && this.IsInputEndTimeValid)
        {
            bool isEndTimeAfterStartTime = this.IsEndTimeAfterStart();
            this.IsInputEndTimeValid = isEndTimeAfterStartTime;
            this.IsInputStartTimeValid = isEndTimeAfterStartTime;
        }
        
        // Check if the pause time is not higher than the difference between the end and start time.
        if (this.IsInputStartTimeValid && this.IsInputEndTimeValid && this.IsInputPauseTimeValid)
        {
            this.IsInputPauseTimeValid = this.IsPauseTimeLessThanDifferenceBetweenStartAndEnd();
        }
        
        // Return true if all time input is valid.
        return this.IsInputStartTimeValid && this.IsInputEndTimeValid && this.IsInputPauseTimeValid;
    }
    
    private bool IsValidTime(string? input)
    {
        if (input != null)
        {
            Regex timeRegex = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return timeRegex.IsMatch(input);
        }

        return false;
    }

    private bool IsEndTimeAfterStart()
    {
        TimeSpan start = this.GetTimeFromRawInput(this.InputStartTime);
        TimeSpan end = this.GetTimeFromRawInput(this.InputEndTime);
        
        return start.CompareTo(end) < 0;
    }
    
    private bool IsPauseTimeLessThanDifferenceBetweenStartAndEnd()
    {
        TimeSpan start = this.GetTimeFromRawInput(this.InputStartTime);
        TimeSpan end = this.GetTimeFromRawInput(this.InputEndTime);
        TimeSpan pause = this.GetTimeFromRawInput(this.InputPauseTime);
        
        return end.Subtract(start) > pause;
    }

    private TimeSpan GetTimeFromRawInput(string input)
    {
        string[] split = input.Split(":");
        
        int hours = int.Parse(split[0]);
        int minutes = int.Parse(split[1]);
        
        return new TimeSpan(hours, minutes, 0);
    }
}