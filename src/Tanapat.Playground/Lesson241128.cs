using LanguageExt;
using LanguageExt.Common;
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

        var computation2 =
            from a in Some(199).AsEff()
            from b in WriteOutputEff(a)
            select b;

        // Notice that the Eff will not be executed inside query expression
        computation2.Run(); 
        
        Console.WriteLine("MainX done 2");
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
    
    public static Eff<A> AsEff<A>(this Option<A> ma)
        => ma.Match(
            a => SuccessEff(a), 
            () => FailEff<A>(Error.New("None")));
}