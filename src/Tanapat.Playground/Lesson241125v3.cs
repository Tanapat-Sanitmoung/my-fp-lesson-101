
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
            from text in File<RT>.readAllText(source)
            from _    in File<RT>.writeAllText(dest, text)
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

public static class File<RT>
    where RT : struct, HasFile<RT>
{
    public static Eff<RT, string> readAllText(string path) =>
        default(RT).FileEff.Map(rt => rt.ReadAllText(path));

    public static Eff<RT, Unit> writeAllText(string path, string text) =>
        default(RT).FileEff.Map(rt => rt.WriteAllText(path, text));
}