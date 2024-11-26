
namespace Tanapat.Playground.Lesson241126;

public static class Example
{
    public static void Execute()
    {
        AccountState a = new AccountState();

        var AddBalance = (AccountState a) => a with { Balance = a.Balance + 100 };

        var c = a 
            | AddBalance;
    }
}

public record AccountState
{
    public static AccountState operator | 
        (AccountState a, Func<AccountState, AccountState> f) 
        => f(a);

    public int Balance { get; init; }
}