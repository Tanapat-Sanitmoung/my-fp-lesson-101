using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
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
            from _1 in WriteOutputOption(b)
            select _1;

        Console.WriteLine("MainX done");

        var computation2 =
            from a in Some(199).AsEff()
            from b in WriteOutputEff(a)
            select b;

        // Notice that the Eff will not be executed inside query expression
        computation2.Run(); 
        
        Console.WriteLine("MainX done 2");

        var computation3 =
            from a in ValidateLength("Hello, 1234567890, 1234567890, 1234567890, 1234567890, 1234567890")
            from b in WriteOutputValidation(a)
            select b;

        computation3.IfFail(err =>  { Console.WriteLine(err.First().Message); return unit; });

        Console.WriteLine("MainX done 3");

         var computation4 =
            from a in ValidateLength("Hello, 1234567890")
            from b in WriteOutputValidation(a)
            select b;

        computation4.IfFail(err =>  { Console.WriteLine(err.First().Message); return unit; });

        Console.WriteLine("MainX done 4");
    }

    public static Validation<Error, string> ValidateLength(string message)
        => message switch 
        {
            { Length: > 50 } => Fail<Error, string>(Error.New("Exceed 50 charactors")),
            var e when string.IsNullOrWhiteSpace(e) => Fail<Error, string>(Error.New("message must not be null or empty")),
            _ => Success<Error, string>(message)
        };

    public static Option<int> MoreThan50(int value) 
        => value > 50 ? value : None;

    public static Eff<Unit> WriteOutputEff(int value)
        => WriteOutputEff($"Value is {value}");   

    public static Eff<Unit> WriteOutputEff(string message)
    {
        Console.WriteLine(message);   
        return SuccessEff(unit);
    }

    public static Option<Unit> WriteOutputOption(int value)
        =>  WriteOutputEff(value).AsOption();

    public static Validation<Error, Unit> WriteOutputValidation(string message)
        => WriteOutputEff(message).AsValidation();
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

    public static Validation<Error, A> AsValidation<A>(this Eff<A> ma)
        => ma
            .Run()
            .Match(
                a   => Success<Error, A>(a), 
                err => Fail<Error, A>(err)
            );
}