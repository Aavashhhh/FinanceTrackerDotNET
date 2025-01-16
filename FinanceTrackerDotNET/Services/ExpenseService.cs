namespace FinanceTrackerDotNET.Services;

using FinanceTrackerDotNET.Models;
using System.Text.Json;

public static class ExpenseService
{
    #region Expense Data Management
    private static void SaveAll(List<ExpenseModel> expenses)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string expenseFilePath = Utils.GetExpenseFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(expenses);
        File.WriteAllText(expenseFilePath, json);
    }

    public static List<ExpenseModel> GetAll()
    {
        string expenseFilePath = Utils.GetExpenseFilePath();
        if (!File.Exists(expenseFilePath))
        {
            return new List<ExpenseModel>();
        }

        var json = File.ReadAllText(expenseFilePath);
        return JsonSerializer.Deserialize<List<ExpenseModel>>(json);
    }
    #endregion

    #region User Data Management
    public static List<User> GetUsers()
    {
        string usersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(usersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(usersFilePath);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    private static void SaveUser(User user)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string usersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        List<User> users = GetUsers();
        var existingUser = users.FirstOrDefault(x => x.Id == user.Id);

        if (existingUser != null)
        {
            existingUser.TotalAmt = user.TotalAmt;
        }
        else
        {
            users.Add(user);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(usersFilePath, json);
    }
    #endregion

    #region Expense Operations
    public static List<ExpenseModel> Create(int amount, string category, string remarks, Guid createdBy)
    {
        List<ExpenseModel> expenses = GetAll();

        expenses.Add(
            new ExpenseModel
            {
                Amount = amount,
                Category = category,
                Remarks = remarks,
                CreatedBy = createdBy,
            }
        );

        List<User> users = GetUsers();
        User user = users.FirstOrDefault(x => x.Id == createdBy);

        if (user != null)
        {
            user.TotalAmt -= amount;
            SaveUser(user);
        }

        SaveAll(expenses);
        return expenses;
    }

    public static List<ExpenseModel> GetByUserId(Guid userId)
    {
        return GetAll().Where(x => x.CreatedBy == userId).ToList();
    }

    public static int GetTotalExpense(Guid userId)
    {
        var userExpenses = GetAll().Where(x => x.CreatedBy == userId).ToList();
        return userExpenses.Sum(x => x.Amount);
    }
    #endregion
}