namespace FinanceTrackerDotNET.Services;

using FinanceTrackerDotNET.Models;
using System.Text.Json;

public static class CategoryService
{
    private static void SaveAll(List<CategoryModel> categories)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appCategoriesFilePath = Utils.GetCategoryFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(categories);
        File.WriteAllText(appCategoriesFilePath, json);
    }

    public static List<CategoryModel> GetAll()
    {
        string appCategoriesFilePath = Utils.GetCategoryFilePath();
        if (!File.Exists(appCategoriesFilePath))
        {
            // Load default categories and tags if the file doesn't exist
            // Load default categories and tags if the file doesn't exist
            var defaultCategories = new List<CategoryModel>
{
    // Default Income Categories
    new CategoryModel { CategoryName = "Salary", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Freelance", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Investment", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Bonus", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Rental Income", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Gifts", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Dividends", Type = "Income", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Side Hustle", Type = "Income", CreatedBy = Guid.NewGuid() },

    // Default Expense Categories
    new CategoryModel { CategoryName = "Vacation", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Food", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Clothes", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Rent", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Healthcare", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Utilities", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Transportation", Type = "Expense", CreatedBy = Guid.NewGuid() },
    new CategoryModel { CategoryName = "Entertainment", Type = "Expense", CreatedBy = Guid.NewGuid() }
};

            SaveAll(defaultCategories);
            return defaultCategories;
        }

        var json = File.ReadAllText(appCategoriesFilePath);
        return JsonSerializer.Deserialize<List<CategoryModel>>(json);
    }

    public static List<CategoryModel> Create(string categoryName, string type, Guid createdBy)
    {
        List<CategoryModel> categories = GetAll();
        bool categoryExists = categories.Any(x => x.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        if (categoryExists)
        {
            throw new InvalidOperationException("A category with this name already exists.");
        }

        categories.Add(
            new CategoryModel
            {
                CategoryName = categoryName,
                Type = type,
                CreatedBy = createdBy
            }
        );
        SaveAll(categories);
        return categories;
    }

    public static CategoryModel GetById(Guid id)
    {
        List<CategoryModel> categories = GetAll();
        return categories.FirstOrDefault(x => x.CategoryId == id);
    }

    public static async Task<List<CategoryModel>> GetCategoriesByUserIdAsync(Guid userId)
    {
        string categoriesFilePath = Utils.GetCategoryFilePath();
        if (!File.Exists(categoriesFilePath))
        {
            return new List<CategoryModel>();
        }

        var json = await File.ReadAllTextAsync(categoriesFilePath);
        var allCategories = JsonSerializer.Deserialize<List<CategoryModel>>(json);

        return allCategories
            .Where(c => c.CreatedBy == userId && c.Type == "Income")
            .ToList();
    }

    public static async Task<List<CategoryModel>> GetExpenseCategoriesByUserIdAsync(Guid userId)
    {
        string categoriesFilePath = Utils.GetCategoryFilePath();
        if (!File.Exists(categoriesFilePath))
        {
            return new List<CategoryModel>();
        }

        var json = await File.ReadAllTextAsync(categoriesFilePath);
        var allCategories = JsonSerializer.Deserialize<List<CategoryModel>>(json);

        return allCategories
            .Where(c => c.CreatedBy == userId && c.Type == "Expense")
            .ToList();
    }
}