namespace FinanceTrackerDotNET.Models;

public class Incomeinfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public int income_Amount { get; set; }
    
    public string Category { get; set; }
    
    public string Remarks { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}