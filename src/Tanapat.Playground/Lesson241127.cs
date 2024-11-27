using LanguageExt;
using static LanguageExt.Prelude;

namespace Tanapat.Playground.Lesson241127;

public static class Program
{

    public static Aff<Source[]> FetchData()
        => Aff(async () => 
        {
            await Task.Delay(1000);

            var result = new List<Source>() 
                { 
                    new("A"),
                    new("B"),
                };

            return result.ToArray();
        });

    public static Eff<Unit> WriteLine(string message)
        => Eff(() => 
        {
            Console.WriteLine(message);
            return Unit.Default;
        });

    public static async Task Main()
    {
        var prog = 
            from d in FetchData()
            from _ in WriteLine($"Fetch count := {d.Count()}")
            select unit;

        var result = await prog.Run();

        result.Match(
            Succ: _ => Console.WriteLine("Completed"),
            Fail: _ => Console.WriteLine("Failed")
        );
    }
    

    public record Source(string Message);
}