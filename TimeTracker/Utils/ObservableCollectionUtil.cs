using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TimeTracker.Utils;

public static class ObservableCollectionUtil
{
    /**
     * This method should be called when an ObservableCollection should be reassigned but the observers would
     * not notice it since they still observe the old ObservableCollection and not the new one.
     */
    public static void ChangeObservableCollection<T>(ObservableCollection<T> old, List<T> newCollection)
    {
        old.Clear();
        foreach (var item in newCollection)
        {
            old.Add(item);
        }
    }
}