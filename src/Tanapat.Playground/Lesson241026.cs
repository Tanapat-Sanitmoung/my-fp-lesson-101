using LanguageExt;
using static LanguageExt.Prelude;
using static Tanapat.Playground.Lesson241026_Methods;

namespace Tanapat.Playground;

public static class Lesson241026
{
    public static void Execute()
    {
        Console.WriteLine(" Solution 1");
        var result =
            from dir in GetFolders("D:\\folder1", "D:\\folder2")
            from f in GetFiles(dir, Seq("*.jpg", "*.png"))
            from c in GetContent(f)
            select c;

        result.Iter(r => 
        Console.WriteLine("Content := {0}", r));

        // =======

        Console.WriteLine(" Solution 2");
        var result2 = GetFolders("D:\\folder1", "D:\\folder2")
            .SelectMany(dir => GetFiles(dir, Seq("*.jpg", "*.png")))
            .SelectMany(f => GetContent(f))
            .ToSeq()
            .Iter(c => Console.WriteLine("Content := {0}", c));

        // Console.Read();
    }

}
public record FileName(string Value);

public record Folder(string Value);

public static class Lesson241026_Methods
{
    public static Seq<FileName> GetFiles(Folder dir, string searchPattern)
        => Mock(dir, searchPattern)
            .Map(d => new FileName(d))
            .ToSeq();

    public static Seq<FileName> GetFiles(Folder dir, Seq<string> searchPattern)
        => from p in searchPattern
           from f in GetFiles(dir, p)
           select f;

    public static Seq<string> Mock(Folder dir, string searchPattern)
        => (dir.Value, searchPattern) switch
        {
            (var d, "*.jpg") when d == "D:\\folder1" => Seq([$"{d}\\f1.jpg", $"{d}\\f2.jpg"]),
            (var d, "*.jpg") when d == "D:\\folder2" => Seq([$"{d}\\f3.jpg", $"{d}\\f4.jpg"]),
            (var d, "*.png") when d == "D:\\folder1" => Seq([$"{d}\\f5.jpg"]),
            (var d, "*.png") when d == "D:\\folder2" => Seq([$"{d}\\f6.jpg", $"{d}\\f7.jpg"]),
            _ => Empty
        };

    public static Seq<Folder> GetFolders(params string[] folderNames)
        => folderNames.Map(f => new Folder(f)).ToSeq();

    
    public static Option<string> GetContent(FileName name)
        => Mock(name);

    private static Option<string> Mock(FileName name)
         => $"Hello, from {name.Value}";
}
