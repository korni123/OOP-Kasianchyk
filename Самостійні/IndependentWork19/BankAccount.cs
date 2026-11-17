namespace IndependentWork19;

public class BankAccount
{
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal balance)
    {
        Owner = owner;
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
        }
        if (amount > Balance)
        {
            throw new InsufficientFundsException(
                $"Not enough money on the account of {Owner}.", amount, Balance);
        }
        Balance -= amount;
    }

    public bool TryWithdraw(decimal amount, out string error)
    {
        if (amount <= 0)
        {
            error = "Amount must be positive.";
            return false;
        }
        if (amount > Balance)
        {
            error = $"Not enough money: requested {amount}, balance {Balance}.";
            return false;
        }
        Balance -= amount;
        error = "";
        return true;
    }
}
