using System;
using System.IO;
using System.Windows.Input;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;
using TimeTracker.ViewModels.Filter;
using TimeTracker.ViewModels.Read;

namespace TimeTracker.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============

    public string DatabaseFile
    {
        get => this._databaseFile;
        set => SetField(ref this._databaseFile, value);
    }

    public bool IsCategoriesViewShown
    {
        get => this._isCategoriesViewShown;
        set => SetField(ref this._isCategoriesViewShown, value);
    }

    public bool IsEntriesViewShown
    {
        get => this._isEntriesViewShown;
        set => SetField(ref this._isEntriesViewShown, value);
    }

    public ReadCategoriesViewModel ReadCategoriesViewModel { get; }
    public ReadEntriesViewModel ReadEntriesViewModel { get; }
    
    public FilterEntriesViewModel FilterEntriesViewModel { get; }
    
    // ==============
    // Commands
    // ==============

    public ICommand ShowCategoriesViewCommand { get; }
    public ICommand ShowEntriesViewCommand { get; }
    
    // ==============
    // Fields
    // ==============

    private string _databaseFile = null!;       // Not null after constructor.
    
    private bool _isCategoriesViewShown;
    private bool _isEntriesViewShown;

    // ==============
    // Initialization
    // ==============
    
    public MainWindowViewModel(string databasePath)
    {
        this.DatabaseFile = Path.GetFileName(databasePath);
        
        // Initialize all dependencies (especially Models and View Models).
        DependencyManager dependencyManager = new DependencyManager();
        dependencyManager.InitializeDependencies(databasePath);
        dependencyManager.LoadData();
        
        // Initialize the View Models for the main view.
        this.ReadCategoriesViewModel = dependencyManager.ReadCategoriesViewModel;
        this.ReadEntriesViewModel = dependencyManager.ReadEntriesViewModel;
        this.FilterEntriesViewModel = dependencyManager.FilterEntriesViewModel;
        
        // Initialize the main view that is shown on startup.
        this.IsCategoriesViewShown = false;
        this.IsEntriesViewShown = true;
        
        // Initialize the commands.
        this.ShowCategoriesViewCommand = new DelegateCommand(this.ShowCategoriesView);
        this.ShowEntriesViewCommand = new DelegateCommand(this.ShowEntriesView);
    }
    
    // ==============
    // Command actions
    // ==============

    public void ShowCategoriesView()
    {
        this.IsCategoriesViewShown = true;
        this.IsEntriesViewShown = false;
    }

    public void ShowEntriesView()
    {
        this.IsCategoriesViewShown = false;
        this.IsEntriesViewShown = true;
    }
}