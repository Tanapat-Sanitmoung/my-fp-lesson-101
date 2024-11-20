namespace Tanapat.Playground.Lesson241120;

using static Console;

public static class Practice
{
    public static void Take01()
    {
        // different between Map and Bind

        Option<int> a = 1;
        Option<string> b = "Hello,";

        // Map signature: (C<T>, (T -> R)) -> C<R>
    
        // wrapped 
        Option<Option<string>> result1 = a.Map(i => b);

        // Bind signature: (C<T>, (T -> C<R>)) -> C<R>

        // not wrapped
        Option<string>         result2 = a.Bind(i => b);
    }

    public static void Take02()
    {
        var f1 = (string s) => PromptForInput("Please enter number")
            .Bind(ParseInt)
            .Bind(ParseIfNotZero);

        var r1 = f1("1");
        var r2 = f1("0");
    }

    public static void Take03()
    {
        WriteLine($"Your age is {ReadAge()}");
    }

    public static void Take04()
    {
        // use Bind with Map
        Option<string> result = Some("5") // Some: (string -> Option<string>) // type lifting
            .Bind(ParseInt) // ParseInt: (string -> Option<int>)
            .Bind(ParseIfNotZero) // ParseIfNotZero: (int -> Option<string>)
            .Map(AppendFoo); // AppendFoo: (string -> string)
    }

    public static void Take05()
    {
        var IsAdult = (int age) => age >= 18;

        var result1 = Some(10).Where(IsAdult); // None
        var result2 = Some(18).Where(IsAdult); // Some(18)
    }

    public static void Take06()
    {
        var IsHighRisk = (string risk) => risk == "3";

        var result = Some("3").Where(IsHighRisk)
            .Match(s => "High Risk Customer", () => "Low Risk Customer");

    }

    public static void Take07()
    {
        var array = new[] { 1, 2};

        // SelectMany is indicated as Bind function ,
        // So, the IEnumerable is a monadic
        var result = array.SelectMany(i => new [] { i + 1, i + 2, i + 3});
        // output
        // [2, 3, 4, 3, 4, 5]
    }

    public static void Take08()
    {
        var peopleAges = new string[] { "10", "", "30" };

        var result = peopleAges
            .Bind(s => ParseInt(s)) // ParseInt: (string -> Option<int>) // ** the "" will be filtered here
            .Bind(a => Age.Of(a)) // Age.Of: (int -> Option<Age>)
            .Average(a => a.Value); // (10 + 30) / 2 = 20

        // output:
        // result = 20
    }

    public readonly record struct Age
    {
        public int Value { get;  }

        private Age(int value)
        {
            Value = value;
        }

        public static Option<Age> Of(int value)
            => value >= 0 ? new Age(value) : None;
    }

    // recursive call until get correct age
    public static int ReadAge()
        => PromptForInput("Please enter your age:")
            .Bind(ParseInt)
            .Match(
                s => s,
                () =>ReadAge()
            );

    public static Option<string> PromptForInput(string prompt)
    {
        WriteLine(prompt);
        return ReadLine() is string inputStr ? inputStr : None;;
    }

    public static Option<int> ParseInt(string str) 
        => int.TryParse(str, out var val) ? val : None;
    
    public static Option<string> ParseIfNotZero(int val)
        => val == 0 ? val.ToString() : None;

    public static string AppendFoo(string str) => $"{str} Foo";
}