using static System.Console;

namespace Tanapat.Playground;

public static class Lesson241027
{
    public static void Execute()
    {
        var result = 
            from r1 in GetFilesDirectories(["folder", "folder2"], ["*.jpg", "*.png"])
            from r2 in GetFileContent(r1)
            select r2;

        _ = result.Do(ctx => ctx.Outputs.Iter(WriteLine))
            .IfFail(ex => WriteLine("Error := {0}", ex.Message));
    }

    static Try<Context> GetFilesDirectories(string[] directories, string[] searchPatterns)
        =>  Try(() => 
            {
                var result = 
                    from d in directories
                    from s in searchPatterns
                    from f in MockGetFiles(d, s)
                    select f;
                return new Context(
                    Files: result.Map(r => new File(r)).ToSeq());
            });

    static Try<Context> GetFileContent(Context context)
        => Try(() => context with { 
                Outputs = context.Files
                    .Map(f => MockReadFile(f.Name))
                    .ToSeq()
                });

    static string[] MockGetFiles(string directory, string searchPattern)
        => [$"{directory} {searchPattern}"];

    static string MockReadFile(string fileName)
        => $"{nameof(Lesson241027),-25} Content of file := {fileName}";

    record File(string Name);

    record Context(Seq<File> Files)
    {
        public Seq<string> Outputs { get; internal set; }
    }

}