using static System.Console;

namespace Tanapat.Playground.Lesson241123;

public static class Play
{
    public static void Play02()
    {
        var events = new TransactionEvent[] {
            new (TransactionType.Debit, 1000m),
            new (TransactionType.Debit, 500),
            new (TransactionType.Credit, 2000m),
        };

        var initState = new AccountState(10_000m);

        var finalState = 
            events.Fold(initState, Replay);
    }

    public static AccountState Replay(AccountState initState, TransactionEvent @event)
    {
        var newState = @event switch 
            {
                { Type: TransactionType.Debit } => new AccountState(initState.Balance - @event.Amount),
                _                               => new AccountState(initState.Balance + @event.Amount),
            };
            
        WriteLine(newState);

        return newState;
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }

    public record TransactionEvent(
        TransactionType Type, 
        decimal Amount);

    public readonly record struct AccountState(decimal Balance); 
    
}
