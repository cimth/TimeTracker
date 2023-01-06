using System;
using System.Collections.Generic;
using System.IO;

namespace TimeTracker.Models.DatabaseConfiguration;

public class DatabaseConfig
{
    // ==============
    // Constants
    // ==============
    
    // The app's config file is placed under "<App base directory>/dbconfig.json" (if existing)
    public static readonly string AppBaseDirectoryPath = Path.GetFullPath(AppContext.BaseDirectory);
    public static readonly string AppConfigFilePath = Path.GetFullPath(Path.Combine(AppBaseDirectoryPath, "dbconfig.json"));
    
    // ==============
    // Properties
    // ==============

    public List<string> DatabasePaths { get; set; } = new();
}