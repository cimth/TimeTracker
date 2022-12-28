namespace TimeTracker.Models.Entities;

public class Category
{
    // ==============
    // Properties
    // ==============
    
    public int Id { get; set; }
    public string Name { get; set; }
    
    // ==============
    // Initialization
    // ==============

    public Category(string name)
    {
        this.Name = name;
    }
}