namespace Tanapat.Playground;

public static class Lesson241027Candidate
{
    public static void Execute()
    {
        var directories = new Folder[] { new("folder"), new("folder2") };
        var searchPatterns = new SearchPattern[] { new("*.jpg"), new("*.png") };

        foreach (var f in MockGetFiles(directories, searchPatterns))
        {
            Console.WriteLine(MockGetContent(f));
        }
    }

    record File(string Name);

    record Folder(string Name);

    record SearchPattern(string Value);

    static IEnumerable<File> MockGetFiles(
        Folder[] directories, SearchPattern[] searchPatterns)
    {
        foreach(Folder d in directories )
        {
            foreach(SearchPattern p in searchPatterns )
            {
                yield return new File($"{d.Name} {p.Value}");
            }
        }
    }

    static string MockGetContent(File file)
        => $"{nameof(Lesson241027Candidate),-25} Content of file := {file.Name}";
}