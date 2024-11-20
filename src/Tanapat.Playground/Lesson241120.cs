namespace Tanapat.Playground.Lesson241120;

using static Console;

public static class Practice
{
    public static void Take01()
    {
        // different between Map and Bind

        Option<int> a = 1;
        Option<string> b = "Hello,";
    
        // wrapped 
        Option<Option<string>> result1 = a.Map(i => b);

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
}