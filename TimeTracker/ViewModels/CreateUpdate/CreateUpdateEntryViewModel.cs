using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TimeTracker.Dialog;
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

    public Entry Entry
    {
        get => this._entry;
        set
        {
            SetField(ref this._entry, value);
            this.UpdateStateAfterInput();
        }
    }

    public List<Category> Categories { get; private set; } = null!;
    
    /*
     * Input validation
     */
    
    public bool IsInputValid
    {
        get => this._isInputValid;
        set => SetField(ref this._isInputValid, value);
    }

    public string InputRawStart { get; set; } = "00:00";
    public string InputRawEnd { get; set; } = "00:00";
    public string InputRawPause { get; set; } = "00:00";

    public bool IsInputRawStartValid
    {
        get => this._isInputRawStartValid;
        private set => SetField(ref this._isInputRawStartValid, value);
    }
    
    public bool IsInputRawEndValid
    {
        get => this._isInputRawEndValid;
        private set => SetField(ref this._isInputRawEndValid, value);
    }
    
    public bool IsInputRawPauseValid
    {
        get => this._isInputRawPauseValid;
        private set => SetField(ref this._isInputRawPauseValid, value);
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

    private Entry _entry = null!;
    private bool _isInputValid;
    
    private bool _isInputRawStartValid;
    private bool _isInputRawEndValid;
    private bool _isInputRawPauseValid;

    // ==============
    // Initialization
    // ==============

    public CreateUpdateEntryViewModel(EntryService entryService, CategoryService categoryService, DialogService dialogService)
    {
        this._entryService = entryService;
        this._categoryService = categoryService;
        this._dialogService = dialogService;

        this.SubmitCommand = new DelegateCommand(this.Submit);
        this.UpdateStateAfterInputCommand = new DelegateCommand(this.UpdateStateAfterInput);
        
        this.InitializeCategories();
        this.InitializeEntry();
        this.UpdateStateAfterInput();
    }

    private void InitializeEntry()
    {
        this.Entry = new Entry(null, DateTime.Now.Date, DateTime.Now.Date, TimeSpan.Zero, "");
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
        this.UpdateTimeValues();
        
        this.IsInputValid = this.Entry.Category != null
                            && allTimeInputIsValid;
    }

    private void Submit()
    {
        // Stop if the input is invalid.
        if (!this.IsInputValid)
        {
            return;
        }
        
        // Create / update the entry.
        if (this.Entry.Id == null)
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
        this._entryService.Create(this.Entry);
    }

    private void UpdateEntry()
    {
        this._entryService.Update();
    }
    
    // ==============
    // Helping methods
    // ==============

    private bool ValidateTimeInput()
    {
        // Check the raw inputs.
        this.IsInputRawStartValid = this.IsValidTime(this.InputRawStart);
        this.IsInputRawEndValid = this.IsValidTime(this.InputRawEnd);
        this.IsInputRawPauseValid = this.IsValidTime(this.InputRawPause);
        
        // Check if the end time is after the start time.
        // If not so, mark both as invalid.
        if (this.IsInputRawStartValid && this.IsInputRawEndValid)
        {
            bool isEndTimeAfterStartTime = this.IsEndTimeAfterStart();
            this.IsInputRawEndValid = isEndTimeAfterStartTime;
            this.IsInputRawStartValid = isEndTimeAfterStartTime;
        }
        
        // Check if the pause time is not higher than the difference between the end and start time.
        if (this.IsInputRawStartValid && this.IsInputRawEndValid && this.IsInputRawPauseValid)
        {
            this.IsInputRawPauseValid = this.IsPauseTimeLessThanDifferenceBetweenStartAndEnd();
        }
        
        // Return true if all time input is valid.
        return this.IsInputRawStartValid && this.IsInputRawEndValid && this.IsInputRawPauseValid;
    }
    
    private bool IsValidTime(string input)
    {
        Regex timeRegex = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
        return timeRegex.IsMatch(input);
    }

    private bool IsEndTimeAfterStart()
    {
        TimeSpan start = this.GetTimeFromRawInput(this.InputRawStart);
        TimeSpan end = this.GetTimeFromRawInput(this.InputRawEnd);
        
        return start.CompareTo(end) < 0;
    }
    
    private bool IsPauseTimeLessThanDifferenceBetweenStartAndEnd()
    {
        TimeSpan start = this.GetTimeFromRawInput(this.InputRawStart);
        TimeSpan end = this.GetTimeFromRawInput(this.InputRawEnd);
        TimeSpan pause = this.GetTimeFromRawInput(this.InputRawPause);
        
        return end.Subtract(start) > pause;
    }

    private void UpdateTimeValues()
    {
        // Start
        if (this.IsInputRawStartValid)
        {
            TimeSpan time = this.GetTimeFromRawInput(this.InputRawStart);
            this.Entry.Start = this.Entry.Start.Date + time;
        }
        
        // End
        if (this.IsInputRawEndValid)
        {
            TimeSpan time = this.GetTimeFromRawInput(this.InputRawEnd);
            this.Entry.End = this.Entry.End.Date + time;
        }
        
        // Pause
        if (this.IsInputRawPauseValid)
        {
            TimeSpan pause = this.GetTimeFromRawInput(this.InputRawPause);
            this.Entry.Pause = pause;
        }

        // Update the displayed entry.
        OnPropertyChanged(nameof(this.Entry));
    }

    private TimeSpan GetTimeFromRawInput(string input)
    {
        string[] split = input.Split(":");
        
        int hours = int.Parse(split[0]);
        int minutes = int.Parse(split[1]);
        
        return new TimeSpan(hours, minutes, 0);
    }
}