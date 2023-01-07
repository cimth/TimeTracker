using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using TimeTracker.Models.DatabaseConfiguration;
using TimeTracker.Utils;
using TimeTracker.ViewModels.Command;

namespace TimeTracker.ViewModels.DatabaseConfiguration;

public class SelectDatabaseFileViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Events
    // ==============

    public event EventHandler? OnRequestClose;
    
    // ==============
    // Properties
    // ==============

    public ObservableCollection<string> LastOpenedDatabaseFiles { get; } = new();
    
    public string? SelectedDatabaseFile { get; set; }

    public bool ShowGrid
    {
        get => this._showGrid;
        set => SetField(ref this._showGrid, value);
    }

    // ==============
    // Fields
    // ==============
    
    private readonly DatabaseConfigurator _databaseConfigurator;
    
    private bool _showGrid;

    // ==============
    // Commands
    // ==============
    
    public ICommand OpenFileFromFilesystemCommand { get; }
    public ICommand CreateFileCommand { get; }
    public ICommand OpenSelectedFileCommand { get; }
    public ICommand RemoveSelectedFileFromListCommand { get; }
    
    // ==============
    // Initialization
    // ==============

    public SelectDatabaseFileViewModel(DatabaseConfigurator databaseConfigurator)
    {
        this._databaseConfigurator = databaseConfigurator;
        
        this.LastOpenedDatabaseFiles.CollectionChanged += this.UpdateShowGrid;
        
        this.OpenFileFromFilesystemCommand = new DelegateCommand(this.OpenFileFromFilesystem);
        this.CreateFileCommand = new DelegateCommand(this.CreateFile);
        this.OpenSelectedFileCommand = new DelegateCommand(this.OpenSelectedFile);
        this.RemoveSelectedFileFromListCommand = new DelegateCommand(this.RemoveSelectedFileFromList);

        this.LoadLastOpenedDatabaseFilesFromConfiguration();
    }

    private void LoadLastOpenedDatabaseFilesFromConfiguration()
    {
        List<string> filesInConfig = this._databaseConfigurator.DatabaseConfig.DatabasePaths;
        foreach (var file in filesInConfig)
        {
            this.LastOpenedDatabaseFiles.Add(file);
        }
    }
    
    // ==============
    // Show / hide grid
    // ==============

    private void UpdateShowGrid(object? sender, NotifyCollectionChangedEventArgs eventArgs)
    {
        this.ShowGrid = this.LastOpenedDatabaseFiles.Count > 0;
    }
    
    // ==============
    // Update the configuration file
    // ==============

    private void SaveAsFirstDatabasePathInConfiguration(string filePath)
    {
        this._databaseConfigurator.SaveAsFirstDatabasePath(filePath);
    }

    private void RemoveDatabasePathFromConfiguration(string filePath)
    {
        this._databaseConfigurator.RemoveDatabasePath(filePath);
    }
    
    // ==============
    // Command actions
    // ==============
    
    private void OpenFileFromFilesystem()
    {
        // Open dialog to select the database path.
        OpenFileDialog dialog = new OpenFileDialog()
        {
            Filter = "*.db|*.db"
        };

        bool? shouldOpen = dialog.ShowDialog();

        if (shouldOpen != null && shouldOpen.Value && File.Exists(dialog.FileName))
        {
            this.SelectedDatabaseFile = dialog.FileName;
            this.SaveAsFirstDatabasePathInConfiguration(dialog.FileName);
            this.CloseDialog();
        }
    }
    
    private void CreateFile()
    {
        // Open dialog to select the database path.
        SaveFileDialog dialog = new SaveFileDialog()
        {
            Filter = "*.db|*.db"
        };

        bool? shouldCreate = dialog.ShowDialog();

        if (shouldCreate != null && shouldCreate.Value)
        {
            // The file does NOT have to be saved because this is done automatically when initializing
            // the DbContext.
            this.SelectedDatabaseFile = dialog.FileName;
            this.SaveAsFirstDatabasePathInConfiguration(dialog.FileName);
            this.CloseDialog();
        }
    }

    private void OpenSelectedFile()
    {
        if (this.SelectedDatabaseFile != null)
        {
            // The file does NOT have to be opened because this is done automatically when initializing
            // the DbContext.
            this.SaveAsFirstDatabasePathInConfiguration(this.SelectedDatabaseFile);
            this.CloseDialog();
        }
    }

    private void RemoveSelectedFileFromList()
    {
        if (this.SelectedDatabaseFile != null)
        {
            this.RemoveDatabasePathFromConfiguration(this.SelectedDatabaseFile);
            this.LastOpenedDatabaseFiles.Remove(this.SelectedDatabaseFile);
        }
    }

    // ==============
    // Close the dialog
    // ==============

    private void CloseDialog()
    {
        this.OnRequestClose?.Invoke(this, EventArgs.Empty);
    }
}