
namespace Tanapat.Playground.Lesson241125v3;

public static class Example
{
    public static void Run()
        => CopyFile<LiveRuntime>("src", "desc")
            .Run(new LiveRuntime())
            .Match(
                Succ: _ => Console.WriteLine("success"), 
                Fail: err => Console.WriteLine(err.Message));

    // Define a runtime agnostic file copying function
    public static Eff<RT, Unit> CopyFile<RT>(string source, string dest) 
        where RT : struct, HasFile<RT> =>
            from text in default(RT).FileEff.Map(rt => rt.ReadAllText(source))
            from _    in default(RT).FileEff.Map(rt => rt.WriteAllText(dest, text))
            select unit;
}

public interface FileIO
{
    string ReadAllText(string path);
    Unit WriteAllText(string path, string text);
}

public interface HasFile<RT> 
    where RT : struct
{
    Eff<RT, FileIO> FileEff { get; }
}

public struct LiveRuntime : HasFile<LiveRuntime>
{
    public Eff<LiveRuntime, FileIO> FileEff => SuccessEff(LiveFileIO.Default);
}

public struct LiveFileIO : FileIO
{
    public static readonly FileIO Default = new LiveFileIO();

    public string ReadAllText(string path) => "test";

    public Unit WriteAllText(string path, string text)
    {
        Console.WriteLine($"{path} >> {text}"); 
        return unit; 
    }
}
