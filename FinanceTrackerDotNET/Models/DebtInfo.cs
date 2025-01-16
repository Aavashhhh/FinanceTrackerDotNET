namespace FinanceTrackerDotNET.Models;

public class DebtInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public int Debt_Amount { get; set; }
    
    public DateTime Debt_Date { get; set; } = DateTime.Now;
    
    public string Debt_Remarks { get; set; }
    
    public Guid Debt_CreatedBy { get; set; }
}