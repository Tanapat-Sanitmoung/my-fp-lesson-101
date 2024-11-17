using static Tanapat.Playground.Lesson241117.X;
using static System.Console;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace Tanapat.Playground.Lesson241117;

public static class Practices
{
    public static void One()
    {
        new string[] { "Apple", "Orange", "Banana" }
            .Select(FillSentence)
            .ToArray()
            .ForEach(WriteLine);

        RunIf(1 == 1, () => WriteLine("Do something"));

        RunIf(Not(1 == 1), () => WriteLine("Do another thing"));

        RunIfElse(2 == 2, 
            () => WriteLine("2 is equal to 2"), 
            () => WriteLine("2 is not equal to 2"));

        RunIf(GetGeneration(1999) == Generation.GenX, () => WriteLine("You are Generation X"));

        // var WhenHighRisk = (int i) => (b) => i > 10;

        // Switch(10, 
        //     WhenHighRisk,
        //     WhenMidRigk,
        //     WhenLowRisk);

        var dict = new Dictionary<int, string>() {
            [1] = "MON",
            [2] = "TUE",
            [3] = "WED",
            [4] = "THU",
            [5] = "FRI",
            [6] = "SAT",
            [7] = "SUN"
        };

        dict.Lookup(8)
            .Match(
                s => WriteLine($"Hello, {s}"), 
                () => WriteLine("Hello, ?"));
    }

    public static string FillSentence(string fruitName) => $"{fruitName} is fruit";
}

public static class X
{
    [Pure]
    public static bool Not(bool value) => !value;

    public static bool Not(Func<bool> predocate) => Not(predocate());

    public static void RunIf(bool condition, Action act) 
    { 
        if (condition) act(); 
    }

    public static void RunIfElse(bool condition, Action act, Action elseAct)
    {
        if (condition) act();
        else elseAct();
    }

    public static void RunIf(Func<bool> predicate, Action act) => RunIf(predicate(), act);


    public static void RunIfElse(Func<bool> predicate, Action act, Action elseAct)
        => RunIfElse(predicate(), act, elseAct);

    public static void ForEach<T>(this T[] values, Action<T> action)
    {
        foreach (var v in values) action(v);
    }

    public static Generation GetGeneration(int birthYear)
        => birthYear switch {
                        <= 1945 => Generation.Builder,
            >= 1946 and <= 1961 => Generation.Boomer,
            >= 1962 and <= 1979 => Generation.GenX,
            >= 1980 and <= 1994 => Generation.GenY,
            >= 1995 and <= 2001 => Generation.GenY,
            _                   => Generation.Children
        };

    // public static void Switch<T>(T value, params Func<T, bool>[] eachBlocks)
    // {
    //     foreach(var handle in eachBlocks)
    //     {
    //         if (handle(value)(value)) break;
    //     }
    // }

    public static Option<TValue> Lookup<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        => dict.TryGetValue(key, out var value) ? value : None;

}

public enum Generation
{
    Builder,
    Boomer,
    GenX,
    GenY,
    GetZ,
    Children
}