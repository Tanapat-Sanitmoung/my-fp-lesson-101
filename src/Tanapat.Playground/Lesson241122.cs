namespace Tanapat.Playground.Lesson241122;

public readonly record struct AccountState
{
    public decimal Balance { get; }

    public AccountState(decimal balance) => Balance = balance;
}

public static class Account
{
    public static Option<AccountState> Debit(this AccountState state, decimal balance) 
        => state.Balance >= balance 
            ? new AccountState(state.Balance - balance)
            : None;
}