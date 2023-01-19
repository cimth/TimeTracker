using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;

namespace TimeTracker.ViewModels.Filter;

public class FilterEntriesViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public ObservableCollection<ObservableKeyValuePair<Category, bool>> CategorySelections { get; } = new();
    
    public string InputNotes
    {
        get => this._inputNotes;
        set => SetField(ref this._inputNotes, value);
    }

    public DateTime? InputDateMin
    {
        get => this._inputDateMin;
        set => SetField(ref this._inputDateMin, value);
    }
    
    public DateTime? InputDateMax
    {
        get => this._inputDateMax;
        set => SetField(ref this._inputDateMax, value);
    }

    // ==============
    // Commands
    // ==============
    
    public ICommand ApplyFiltersCommand { get; }
    
    public ICommand SelectAllCategoriesCommand { get; }
    public ICommand UnselectAllCategoriesCommand { get; }
    
    public ICommand ClearNotesCommand { get; }
    public ICommand ClearInputDateMinCommand { get; }
    public ICommand ClearInputDateMaxCommand { get; }

    // ==============
    // Fields
    // ==============

    private readonly CategoryService _categoryService;
    private readonly EntryService _entryService;
    
    private string _inputNotes = "";
    private DateTime? _inputDateMin;
    private DateTime? _inputDateMax;

    // ==============
    // Initialization
    // ==============

    public FilterEntriesViewModel(DependencyManager dependencyManager)
    {
        this._categoryService = dependencyManager.CategoryService;
        this._entryService = dependencyManager.EntryService;

        this.ApplyFiltersCommand = new DelegateCommand(this.ApplyFilters);
        this.SelectAllCategoriesCommand = new DelegateCommand(this.SelectAllCategories);
        this.UnselectAllCategoriesCommand = new DelegateCommand(this.UnselectAllCategories);
        this.ClearNotesCommand = new DelegateCommand(this.ClearNotes);
        this.ClearInputDateMinCommand = new DelegateCommand(this.ClearInputDateMin);
        this.ClearInputDateMaxCommand = new DelegateCommand(this.ClearInputDateMax);

        this._categoryService.Categories.CollectionChanged += Categories_CollectionChanged;
        this.ReinitializeCategorySelections(true);
    }

    private void Categories_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        this.ReinitializeCategorySelections(true);
    }

    private void ReinitializeCategorySelections(bool isSelected)
    {
        this.CategorySelections.Clear();
        foreach (Category category in this._categoryService.Categories)
        {
            this.CategorySelections.Add(new ObservableKeyValuePair<Category, bool>(category, isSelected));
        }
    }
    
    // ==============
    // Command actions
    // ==============

    private void ApplyFilters()
    {
        this._entryService.ReadWithFilters(this.GetSelectedCategories(), this.InputNotes, this.InputDateMin, this.InputDateMax);
    }

    private void SelectAllCategories()
    {
        this.ReinitializeCategorySelections(true);
        this.ApplyFilters();
    }

    private void UnselectAllCategories()
    {
        this.ReinitializeCategorySelections(false);
        this.ApplyFilters();
    }

    private void ClearNotes()
    {
        this.InputNotes = "";
    }
    
    private void ClearInputDateMin()
    {
        this.InputDateMin = null;
    }
    
    private void ClearInputDateMax()
    {
        this.InputDateMax = null;
    }
    
    // ==============
    // Helping methods for filtering
    // ==============
    
    private List<Category> GetSelectedCategories()
    {
        List<Category> selected = new List<Category>();
        foreach (ObservableKeyValuePair<Category, bool> categorySelection in this.CategorySelections)
        {
            // A category is selected when the value of the (key, value) pairs is true.
            if (categorySelection.Value)
            {
                selected.Add(categorySelection.Key);
            }
        }
        return selected;
    }
}