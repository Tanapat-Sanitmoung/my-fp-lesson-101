
using System.Reflection.Metadata.Ecma335;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Tanapat.Playground;

public static class Lesson241026Part2
{

    public static Option<bool> Execute()
        => CreateExecutionContext(["folder1", "folder2"], ["*.jpg", "*.png"])
                .Bind(GetFiles)
                .Bind(PrintFilesContent)
                .Map(ctx => ctx.Success)
                .IfFail(f => false);
    static Try<ExecutionContext> CreateExecutionContext(string[] folders, string[] patterns)
        => Try(() => 
            new ExecutionContext(
                Folders: folders.Map(f => new Folder(f)).ToSeq(),
                SearchPatterns: patterns.Map(p => new SearchPattern(p)).ToSeq()));

    static Try<ExecutionContext> GetFiles(ExecutionContext context)
        => Try(() => 
            context with 
            {
                Files = context
                    .Folders
                    .Fold(
                        state: Seq<File>(), 
                        (s, fd) => s.Append(Mock(fd, context.SearchPatterns)))
            });

    static Seq<File> Mock(Folder fd, Seq<SearchPattern> patterns)
        => patterns.Map(
            p => new File($"{fd.Name} {p.Value}"));

    static Try<ExecutionContext> PrintFilesContent(ExecutionContext context)
        => Try(() => {
            context.Files
                .Iter(f => Console.WriteLine($"Content of {f.Name}"));
            
            return context with {
                Success = true
            };
        });

    public record ExecutionContext(Seq<Folder> Folders, Seq<SearchPattern> SearchPatterns)
    {
        public Seq<File> Files { get; set;}
    public bool Success { get; internal set; }
}

    public record Folder(string Name);

    public record File(string Name);

    public record SearchPattern(string Value);

}


public interface IFileService
{

}