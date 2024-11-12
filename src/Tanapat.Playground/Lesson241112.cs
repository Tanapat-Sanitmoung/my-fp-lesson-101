
// to enable call "Inital" rather than "Extensions.Initial"
using static Tanapat.Playground.Lesson241112.Extensions;

namespace Tanapat.Playground.Lesson241112;

// from this link: https://weblogs.asp.net/dixin/functional-csharp-higher-order-function-currying-and-first-class-function

public static class Extensions
{
    public static TNextResult PipeTo<TCurrResult, TNextResult>(this TCurrResult result, Func<TCurrResult, TNextResult> next) 
        => next(result);

    public static TCurrResult Initial<TCurrResult>(Func<TCurrResult> init) => init();
    
    public static TCurrResult Initial<TCurrResult>(TCurrResult initValue) => initValue;
}

public static class UseCase
{
    public static void UseCase1()
    {
        // just design pipeline
        _ = Initial(2)
            .PipeTo(s1 => s1 + 2)
            .PipeTo(s2 => s2 * 2)
            .PipeTo(s3 => $"Result is {s3}") 
            switch {
                var x when x == "8" => HandleSuccess(),
                _ => HandleSuccess(),
            };
    }

    public static Unit HandleSuccess() => Unit.Default;
    public static Unit HandleFail() => Unit.Default;
}