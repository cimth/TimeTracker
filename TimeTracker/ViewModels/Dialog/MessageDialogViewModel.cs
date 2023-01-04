using TimeTracker.Utils;

namespace TimeTracker.ViewModels.Dialog;

public class MessageDialogViewModel : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============
    
    public string DialogTitle { get; }
    
    public string DialogMessage { get; }
    
    // ==============
    // Initialization
    // ==============
    
    public MessageDialogViewModel(string titleResourceName, string messageResourceName)
    {
        this.DialogTitle = LanguageUtil.GiveLocalizedString(titleResourceName);
        this.DialogMessage = LanguageUtil.GiveLocalizedString(messageResourceName);
    }
}