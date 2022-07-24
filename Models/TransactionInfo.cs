namespace TestTask.Models;
public class Transaction
{
    public Guid Id { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime TransactionTime { get; set; }
}

public enum TransactionStatus
{
    Created, 
    Running, 
    Finished
}
