using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using TimeTracker.Views.Templates;

namespace TimeTracker.Views.Read;

public partial class ReadEntriesView : UserControl
{
    // ==============
    // Initialization
    // ==============
    
    public ReadEntriesView()
    {
        InitializeComponent();
    }

    // ==============
    // Update DataGrid
    // ==============
    
    private void DataGridEntries_OnLayoutUpdated(object sender, EventArgs e)
    {
        // Compute how much space is available for the DataGrid column "Notes".
        //
        // Note:
        // It is not possible to just set the "x:Name" attribute for the column "Notes" to reference it
        // because the column element is placed inside the UserControl "BasicDataGrid" and you cannot set the
        // attribute "x:Name" for sub elements of UserControls.
        DataGridColumn columnNotes = this.DataGridEntries!.Columns[7];
        
        double remainingWidth = this.DataGridEntries!.ActualWidth;
        foreach (DataGridColumn column in this.DataGridEntries.Columns)
        {
            if (column != columnNotes)
            {
                remainingWidth -= column.ActualWidth;
            }
        }

        // Set the remaining width as width for the column "Notes".
        columnNotes.Width = remainingWidth;
    }
}