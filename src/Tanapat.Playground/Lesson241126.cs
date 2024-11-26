
namespace Tanapat.Playground.Lesson241126;

public static class Example
{
    public static void Execute()
    {
        AccountState a = new AccountState();

        var AddBalance = (AccountState a) => a with { Balance = a.Balance + 100 };
        var DeductBalance = (AccountState a) => a with { Balance = a.Balance - 50 };

        var c = a 
            >> AddBalance
            >> DeductBalance;

        Console.WriteLine(c);

        var computation = 
            from __1 in WriteLine("Please enter your name:")
            from def in DefaultName("John Doe")
            from nam in ReadLine(def)
            from __2 in Greeting(nam)
            select unit;

        _ = computation.Run()
            .Match(
                Succ: u => Console.WriteLine("Success"),
                Fail: f => Console.WriteLine($"Failed := {f.Message}"));
    }

    static Eff<Unit> Greeting(string name)
        => Eff(() => 
            { 
                Console.WriteLine($"Hello, {name}");
                return Unit.Default;
            });

    static Eff<string> ReadLine(string defaultName) 
        => Eff(() => Console.ReadLine() ?? defaultName);

    static Eff<string> DefaultName(string name) 
        => SuccessEff(name);

    static Eff<Unit> WriteLine(string prompt) 
        => Eff(() => { Console.WriteLine(prompt); return Unit.Default; });
}

public record AccountState
{
    public static AccountState operator >> 
        (AccountState a, Func<AccountState, AccountState> f) 
        => f(a);

    public int Balance { get; init; }
}