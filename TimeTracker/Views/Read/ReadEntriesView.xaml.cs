using System.Windows.Controls;
using System.Windows.Data;

namespace TimeTracker.Views.Read;

public partial class ReadEntriesView : UserControl
{
    private CollectionViewSource? _gridGroups;
    
    public ReadEntriesView()
    {
        InitializeComponent();

       this._gridGroups = this.FindResource("GridGroups") as CollectionViewSource;
    }

    private void FrameworkElement_OnSourceUpdated(object? sender, DataTransferEventArgs e)
    {
        this._gridGroups?.GroupDescriptions.Clear();
        this._gridGroups?.View.Refresh();
    }
}