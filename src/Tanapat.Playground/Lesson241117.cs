using static Tanapat.Playground.Lesson241117.X;
using static System.Console;
using System.Diagnostics.Contracts;
using LanguageExt.SomeHelp;

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

        var a1 = Age.Of(10);

        a1.Match(
            (a) => WriteLine($"Age is {a.Value}"),
            () => WriteLine("None"));

        var a2 = Age.Of(2000);

        a2.Match(
            (a) => WriteLine($"Age is {a.Value}"),
            () => WriteLine("None"));

        Risk.OfAge(a1)
            .IfSome(r => WriteLine($"Your risk is {r.ToSome().Value}"));

        var value = ParseInt("10");
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

    public static bool Between(this int value, int min, int max)
        => min <= value && value <= max;

    public static Option<int> ParseInt(string t)
        => int.TryParse(t, out var val) ? Some(val) : None;
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

public readonly record struct Age
{
    public int Value { get;}

    private Age(int value) => Value = value;

    public static Option<Age> Of(int value) => IsValid(value) ? new Age(value) : None;

    public static bool IsValid(int value) => value.Between(0, 120);

    public static implicit operator int(Age value) => value.Value;

    public static bool operator <(Age left, Age right) => left.Value < right.Value;

    public static bool operator >(Age left, Age right) => left.Value > right.Value;

    public static bool operator <(Age left, int right) => left.Value < right;

    public static bool operator >(Age left, int right) => left.Value > right;
}

public enum RiskRanges { Low, Mid, High }

public readonly record struct Risk 
{
    public static Option<RiskRanges> OfAge(Option<Age> age)
        => age.Map(a => CalculateRisk(a));
    
    public static RiskRanges CalculateRisk(int age)
        => age switch {
             < 10           => RiskRanges.Low,
            >= 10 and <= 30 => RiskRanges.Mid,
                          _ => RiskRanges.High
        };
}