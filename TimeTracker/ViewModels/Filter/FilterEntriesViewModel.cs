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

    // ==============
    // Commands
    // ==============
    
    public ICommand ApplyFiltersCommand { get; }
    
    public ICommand SelectAllCategoriesCommand { get; }
    public ICommand UnselectAllCategoriesCommand { get; }
    
    public ICommand ClearNotesCommand { get; }

    // ==============
    // Fields
    // ==============

    private readonly CategoryService _categoryService;
    private readonly EntryService _entryService;
    
    private string _inputNotes;

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
        this.FilterByCategories();
        this.FilterByNotes();
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
    
    // ==============
    // The actual filter methods
    // ==============
    
    private void FilterByCategories()
    {
        List<Category> selected = new List<Category>();
        foreach (ObservableKeyValuePair<Category, bool> categorySelection in this.CategorySelections)
        {
            if (categorySelection.Value)
            {
                selected.Add(categorySelection.Key);
            }
        }
        
        this._entryService.ReadAllByCategories(selected);
    }

    private void FilterByNotes()
    {
        this._entryService.ReadAllByNotes(this.InputNotes);
    }
}