using System;
using System.Windows.Input;

namespace TimeTracker.ViewModels.Command;

/// <summary>
/// Can be used for parameterless commands by providing the method to execute and an method for checking if the
/// command currently can be executed.
/// </summary>
public class DelegateCommand : ICommand
{
    // ==============
    // FIELDS
    // ==============

    public event EventHandler? CanExecuteChanged;
    
    private readonly Predicate<object?>? _canExecute;
    private readonly Action _execute;

    // ==============
    // INITIALIZATION
    // ==============

    public DelegateCommand(Action execute, Predicate<object?>? canExecute = null)
    {
        this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this._canExecute = canExecute;
    }

    // ==============
    // CAN EXECUTE
    // ==============

    public bool CanExecute(object? parameter)
    {
        // always can be executed
        if (this._canExecute == null)
        {
            return true;
        }
        
        // can only be executed on certain conditions
        return this._canExecute.Invoke(parameter);
    }

    /// <summary>
    /// Raises <see cref="CanExecuteChanged"/> to re-check the condition if the command can be
    /// executed.
    /// <para/>
    /// Should be called each time after any variable of the <see cref="CanExecute"/> check is changed.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    // ==============
    // EXECUTE
    // ==============
    
    public void Execute(object? parameter)
    {
        this._execute?.Invoke();
    }
}