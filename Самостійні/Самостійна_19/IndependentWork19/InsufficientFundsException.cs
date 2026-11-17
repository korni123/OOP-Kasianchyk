namespace IndependentWork19;

public class InsufficientFundsException : Exception
{
    public decimal RequestedAmount { get; }
    public decimal CurrentBalance { get; }

    public InsufficientFundsException()
    {
    }

    public InsufficientFundsException(string message) : base(message)
    {
    }

    public InsufficientFundsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InsufficientFundsException(string message, decimal requestedAmount, decimal currentBalance)
        : base(message)
    {
        RequestedAmount = requestedAmount;
        CurrentBalance = currentBalance;
    }
}
