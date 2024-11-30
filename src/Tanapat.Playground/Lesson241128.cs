using LanguageExt;
using LanguageExt.SomeHelp;
using static LanguageExt.Prelude;

namespace Tanapat.Playground.Lesson241128;

public class Program
{
    public static async Task MainX()  
    {
        var computation =
            from a in Some(100)
            from b in MoreThan50(a)
            from _1 in WriteOutput(b)
            select _1;
            
        Console.WriteLine("MainX done");
    }

    public static Option<int> MoreThan50(int value) 
        => value > 50 ? value : None;

    public static Eff<Unit> WriteOutputEff(int value)
    {
        Console.WriteLine($"Value is {value}");   
        return SuccessEff(unit);
    }

    public static Option<Unit> WriteOutput(int value)
        =>  WriteOutputEff(value).AsOption();
}

public record Person();

public static class Extensions
{
    public static Option<Unit> AsOption<A>(this Eff<A> ma)
        =>  ma
            .Run()
            .Match(
                Succ: u => Some(unit),
                Fail: _ => None
            );
}