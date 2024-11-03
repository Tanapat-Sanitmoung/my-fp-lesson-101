
namespace Tanapat.Playground;

public static class Lesson241102
{
    public static void Execute()
    {
        string rawString = "0002020102111501A6001B63045567";

        var reader = new StrReader(rawString);

        while (reader.NotAtEnd())
        {
            var str1 = reader.Read(2);
            var str2 = reader.Read(2);
            var str3 = reader.Read(ushort.Parse(str2));

            Console.WriteLine($"{str1,-4}{str3,-50}");
        }

    }

    class StrReader
    {
        private readonly string _str;
        private int _idx = 0;

        public StrReader(string str) => _str = str;

        public bool NotAtEnd() => _idx < _str.Length;

        public string Read(int count) => 
            (_str.Substring(_idx, count), _idx+= count).Item1;
    }

}
