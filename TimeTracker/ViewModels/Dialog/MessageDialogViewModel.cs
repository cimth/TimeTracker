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
    
    public MessageDialogViewModel(string titleText, string messageText)
    {
        this.DialogTitle = titleText;
        this.DialogMessage = messageText;
    }
}