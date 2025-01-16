namespace FinanceTrackerDotNET.Services;

using FinanceTrackerDotNET.Models;
using System.Text.Json;

public static class DebtService
{
    #region Debt Data Management
    private static void SaveAll(List<DebtInfo> debts)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string debtFilePath = Utils.GetDebtFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(debts);
        File.WriteAllText(debtFilePath, json);
    }

    public static List<DebtInfo> GetAll()
    {
        string debtFilePath = Utils.GetDebtFilePath();
        if (!File.Exists(debtFilePath))
        {
            return new List<DebtInfo>();
        }

        var json = File.ReadAllText(debtFilePath);
        return JsonSerializer.Deserialize<List<DebtInfo>>(json);
    }
    #endregion

    #region Debt Operations
    public static List<DebtInfo> Create(int amount, string remarks, DateTime debtDate, Guid createdBy)
    {
        List<DebtInfo> debts = GetAll();

        debts.Add(
            new DebtInfo
            {
                Debt_Amount = amount,
                Debt_Remarks = remarks,
                Debt_Date = debtDate,
                Debt_CreatedBy = createdBy
            }
        );

        SaveAll(debts);
        return debts;
    }

    public static void PayOff(Guid debtId)
    {
        List<DebtInfo> debts = GetAll();
        var debtToRemove = debts.FirstOrDefault(x => x.Id == debtId);

        if (debtToRemove != null)
        {
            debts.Remove(debtToRemove);
            SaveAll(debts);
        }
    }

    public static List<DebtInfo> GetByUserId(Guid userId)
    {
        return GetAll().Where(x => x.Debt_CreatedBy == userId).ToList();
    }

    public static int GetTotalDebt(Guid userId)
    {
        var userDebts = GetAll().Where(x => x.Debt_CreatedBy == userId).ToList();
        return userDebts.Sum(x => x.Debt_Amount);
    }
    #endregion
}