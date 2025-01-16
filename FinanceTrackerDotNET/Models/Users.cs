namespace FinanceTrackerDotNET.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string userPasswordHash { get; set; }
    
    public int TotalAmt { get; set; } = 0;
    public string preCurrency {get; set;}
}