using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

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

    public static void Play02()
    {
        // Func<string, string, string, string>
        var FormatName = (string last, string mid, string first) => $"{last} {mid} {first}";

        var SetFirstName = FormatName
                .Apply("LastName-Z")
                .Apply("Middle-Y");

        Console.WriteLine(SetFirstName("Tanapat"));
        Console.WriteLine(SetFirstName("Lawan"));

        // LastName-Z Middle-Y Tanapat
        // LastName-Z Middle-Y Lawan
    }

    public static void Play03()
    {
        var isAdult = (int a) => a switch {
            > 19 => Success<Error, int>(a),
            _ => Fail<Error, int>(Error.New("not adult"))
        };

        var isOdd = (int a) => (a % 2) switch {
            0 => Success<Error, int>(a),
            _ => Fail<Error, int>(Error.New("not odd"))
        };

        Func<int, Validation<Error, int>>[] validations = [
            isAdult,
            isOdd
        ];

        var age = 11;

        // Harvest errors
        var result = validations
            .Fold(
                state: new List<Validation<Error, int>>(),
                folder: (acc, validate) => 
                { 
                    _ = acc.Append(validate(age)); 
                    return acc; 
                }
            )
            .Where(v => v.IsFail);
    }

}

public static class FP
{
    public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this, T1 t1)
        => (t2, t3) => @this(t1, t2, t3);

    public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> @this, T1 t1)
        => t2 => @this(t1, t2);
}
