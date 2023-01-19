using TimeTracker.Utils;

namespace TimeTracker.ViewModels.Dialog;

public class ConfirmDialogViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============
    
    public string Request { get; }
    
    // ==============
    // Initialization
    // ==============
    
    public ConfirmDialogViewModel(string requestResourceName)
    {
        this.Request = LanguageUtil.GiveLocalizedString(requestResourceName);
    }
}