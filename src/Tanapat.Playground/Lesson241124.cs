namespace Tanapat.Playground.Lesson241124;

public static class Playing
{
    public static void Play01()
    {
        // Func<string, string, string, string>
        var FormatName = (string last, string mid, string first) => $"{last} {mid} {first}";

        // Func<string, Func<string, string, string>>
        var SetLastName = (string last) => (string middle, string first) => FormatName(last, middle, first);

        // Func<string, Func<string, string>>
        var SetMidName = (string mid) => (string first) => SetLastName("LastName-X")(mid, first);

        // Func<string, string>
        var SetFirstName = SetMidName("Middle-Y");

        Console.WriteLine(SetFirstName("Tanapat"));
        Console.WriteLine(SetFirstName("Lawan"));

        // LastName-XMiddle-YTanapat
        // LastName-XMiddle-YLawan
    }
}