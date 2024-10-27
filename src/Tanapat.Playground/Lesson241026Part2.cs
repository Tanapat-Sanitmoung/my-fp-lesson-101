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
                    Folders: GetFolders(folders),
                    SearchPatterns: GetSearchPatterns(patterns)
                )
            );

    static Seq<Folder> GetFolders(string[] folders)
        => folders
            .Map(f => new Folder(f))
            .ToSeq();

    static Seq<SearchPattern> GetSearchPatterns(string[] searchPatterns)
        => searchPatterns
            .Map(p => new SearchPattern(p))
            .ToSeq();

    static Try<ExecutionContext> GetFiles(ExecutionContext context)
        => Try(() =>
            context with
            {
                Files = GetFilesInFolders(context.Folders, context.SearchPatterns)
            });

    static Seq<File> GetFilesInFolders(Seq<Folder> folders, Seq<SearchPattern> patterns)
        => folders
            .Fold(
                state: Seq<File>(),
                (state, eachFolder) => state.Append(MockSearchFiles(eachFolder, patterns)));

    static Seq<File> MockSearchFiles(Folder fd, Seq<SearchPattern> patterns)
        => patterns.Map(
            p => new File($"{fd.Name} {p.Value}"));

    static Try<ExecutionContext> PrintFilesContent(ExecutionContext context)
        => Try(() =>
        {
            context.Files
                .Iter(f => Console.WriteLine($"Content of {f.Name}"));

            return context with
            {
                Success = true
            };
        });

    public record ExecutionContext(Seq<Folder> Folders, Seq<SearchPattern> SearchPatterns)
    {
        public Seq<File> Files { get; set; }
        public bool Success { get; internal set; }
    }

    public record Folder(string Name);

    public record File(string Name);

    public record SearchPattern(string Value);

}