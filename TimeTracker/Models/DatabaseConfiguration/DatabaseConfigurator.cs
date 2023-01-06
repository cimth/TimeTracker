using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TimeTracker.Models.DatabaseConfiguration;

public class DatabaseConfigurator
{
    // ==============
    // Properties
    // ==============

    public DatabaseConfig DatabaseConfig { get; }
    
    // ==============
    // Initialization
    // ==============

    public DatabaseConfigurator()
    {
        this.DatabaseConfig = this.LoadDatabaseConfig();
    }
    
    private DatabaseConfig LoadDatabaseConfig()
    {
        // Load the app config if existing.
        // => The config file (if existing) is placed inside the app's base directory.
        DatabaseConfig? appConfig = null;        
        if (File.Exists(DatabaseConfig.AppConfigFilePath))
        {
            string configJson = File.ReadAllText(DatabaseConfig.AppConfigFilePath);
            appConfig = JsonSerializer.Deserialize<DatabaseConfig>(configJson) ?? throw new InvalidOperationException();
        }

        // Return the loaded config or a new (empty) config if no config is existing yet.
        return appConfig ?? new DatabaseConfig();
    }
    
    // ==============
    // Save changes
    // ==============
    
    private void UpdateConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            // Pretty print
            WriteIndented = true
        };
        string configJson = JsonSerializer.Serialize(this.DatabaseConfig, options);
        File.WriteAllText(DatabaseConfig.AppConfigFilePath, configJson, Encoding.UTF8);
    }
    
    // ==============
    // Change the single config elements
    // ==============
    
    public void AddDatabasePath(string databasePath)
    {
        this.DatabaseConfig.DatabasePaths.Add(databasePath);
        this.UpdateConfigFile();
    }
    
    public void RemoveDatabasePath(string databasePath)
    {
        this.DatabaseConfig.DatabasePaths.Remove(databasePath);
        this.UpdateConfigFile();
    }
}