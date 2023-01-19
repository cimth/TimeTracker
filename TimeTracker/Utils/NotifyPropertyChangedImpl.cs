using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeTracker.Utils;

/// <summary>
/// This class can be used as base class for data type classes and similar which should provide the interface
/// <see cref="INotifyPropertyChanged"/>.
/// </summary>
public class NotifyPropertyChangedImpl: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// <para/>
    /// Should be placed inside the <c>set</c> method of each property whose changes should be noticed by the view.
    /// <para/>
    /// When called without a parameter, the property from the calling <c>set</c> method will automatically be
    /// taken as value.
    /// However, you can still give the property's name as string or via <c>nameof</c> method.
    /// <para/>
    /// In some circumstances it might be also necessary to call this method for other properties than only the changed
    /// on (e.g. a variable `fullName` will be updated, too, if the variable `firstName` has changed).
    /// </summary>
    /// <param name="propertyName">
    /// The name of the property that is changed. Set to the calling member name by default which usually is the
    /// property in whose <c>set</c> method the method is called.
    /// </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets the given value to the referenced field and raises a <see cref="PropertyChanged"/> event
    /// afterwards.
    /// <para/>
    /// Can be used for a one-line change and notification in a property's <c>set</c> method instead of first
    /// changing the value and then raising the notification event by calling the <see cref="OnPropertyChanged"/>
    /// method.
    /// <para/>
    /// For more information, see <see cref="OnPropertyChanged"/>.
    /// </summary>
    /// <param name="field">The (private!) field to be changed.</param>
    /// <param name="value">The (new) value to be set in the field.</param>
    /// <param name="propertyName">
    /// The name of the property that is changed. Set to the calling member name by default which usually is the
    /// property in whose <c>set</c> method the method is called.
    /// </param>
    /// <typeparam name="T">The type of the changed field and value.</typeparam>
    /// <returns>True if the field has been changed.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        // only continue if the field needs to be changed
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }
        
        // change the field and raise the PropertyChanged event
        field = value;
        OnPropertyChanged(propertyName);
        
        // return true because the field has been changed
        return true;
    }
}