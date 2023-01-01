using System.Collections.ObjectModel;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;

namespace TimeTracker.ViewModels.Read;

public class ReadCategoriesViewModel
{
    // ==============
    // Properties
    // ==============
    
    public ObservableCollection<Category> Categories { get; private set; } = null!;
    
    // ==============
    // Fields
    // ==============
    
    private readonly CategoryService _categoryService;
    
    // ==============
    // Initialization
    // ==============

    public ReadCategoriesViewModel(CategoryService categoryService)
    {
        this._categoryService = categoryService;
        
        this.InitializeCategories();
    }
    
    private void InitializeCategories()
    {
        this.Categories = new ObservableCollection<Category>(this._categoryService.ReadAll());
    }
}