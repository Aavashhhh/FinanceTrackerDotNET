namespace FinanceTrackerDotNET.Models;

public class ExpenseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Amount { get; set; }
    public string Category { get; set; }
    public string Remarks {get; set;}
    public Guid CreatedBy { get; set; }
}