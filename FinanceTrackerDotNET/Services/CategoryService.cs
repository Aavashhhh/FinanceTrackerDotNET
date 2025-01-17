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
            // Initialize the file with an empty JSON array if it doesn't exist
            SaveAll(new List<CategoryModel>());
            return new List<CategoryModel>();
        }

        var json = File.ReadAllText(appCategoriesFilePath);

        // Handle empty or invalid JSON files
        if (string.IsNullOrWhiteSpace(json))
        {
            SaveAll(new List<CategoryModel>());
            return new List<CategoryModel>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<CategoryModel>>(json);
        }
        catch (JsonException)
        {
            // If the JSON is invalid, reset the file with an empty list
            SaveAll(new List<CategoryModel>());
            return new List<CategoryModel>();
        }
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