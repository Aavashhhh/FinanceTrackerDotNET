using FinanceTrackerDotNET.Models;
using System.Text.Json;

namespace FinanceTrackerDotNET.Services;

public class IncomeService
{
    #region Income Data Management
    private static void SaveAll(List<Incomeinfo> incomes)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string incomeFilePath = Utils.GetIncomeFilePath();
        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(incomes);
        File.WriteAllText(incomeFilePath, json);
    }

    public static List<Incomeinfo> GetAll()
    {
        string incomeFilePath = Utils.GetIncomeFilePath();
        if (!File.Exists(incomeFilePath))
        {
            return new List<Incomeinfo>();
        }

        var json = File.ReadAllText(incomeFilePath);
        return JsonSerializer.Deserialize<List<Incomeinfo>>(json) ?? new List<Incomeinfo>();
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

    #region Income Operations
    public static List<Incomeinfo> Create(int amount, string category, string remarks, Guid createdBy)
    {
        List<Incomeinfo> incomes = GetAll();

        incomes.Add(new Incomeinfo
        {
            income_Amount = amount,
            Category = category,
            Remarks = remarks,
            CreatedBy = createdBy,
        });

        List<User> users = GetUsers();
        User user = users.FirstOrDefault(x => x.Id == createdBy);

        if (user != null)
        {
            user.TotalAmt += amount;
            SaveUser(user);
        }

        SaveAll(incomes);
        return incomes;
    }

    public static Incomeinfo GetById(Guid id)
    {
        List<Incomeinfo> incomes = GetAll();
        return incomes.FirstOrDefault(x => x.Id == id);
    }

    public static bool Delete(Guid id)
    {
        List<Incomeinfo> incomes = GetAll();
        var incomeToDelete = incomes.FirstOrDefault(x => x.Id == id);

        if (incomeToDelete != null)
        {
            incomes.Remove(incomeToDelete);
            SaveAll(incomes);
            return true;
        }

        return false;
    }

    public static bool Update(Guid id, int amount, string category, string remarks)
    {
        List<Incomeinfo> incomes = GetAll();
        var incomeToUpdate = incomes.FirstOrDefault(x => x.Id == id);

        if (incomeToUpdate != null)
        {
            incomeToUpdate.income_Amount = amount;
            incomeToUpdate.Category = category;
            incomeToUpdate.Remarks = remarks;
            SaveAll(incomes);
            return true;
        }

        return false;
    }

    public static List<Incomeinfo> GetByUserId(Guid userId)
    {
        return GetAll().Where(t => t.CreatedBy == userId).ToList();
    }

    public static int GetTotalIncome(Guid userId)
    {
        var userIncomes = GetAll().Where(x => x.CreatedBy == userId).ToList();
        return userIncomes.Sum(x => x.income_Amount);
    }
    #endregion
}