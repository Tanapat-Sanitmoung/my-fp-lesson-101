
using LanguageExt.SomeHelp;

namespace Tanapat.Playground;

public static class Lesson241102
{
    public static void Execute()
    {
        string rawString = "0002020102111501A6001B63045567";

        _ = QrReader.Convert(rawString)
            .Match(
                Right: (r) => r.Iter(b => Console.WriteLine($"{b.Id}  {b.Value}")), 
                Left : (l) => { 
                        Console.WriteLine(l.Message); 
                        return unit;
                    }
            );
    }

    class QrReader 
    {
        public static Either<Problem, Seq<Block>> Convert(string rawString)
        {
            try
            {
                var output = new List<Block>();

                var reader = new StrReader(rawString);

                while (reader.NotAtEnd())
                {
                    var str1 = reader.Read(2);
                    var str2 = reader.Read(2);
                    var str3 = reader.Read(ushort.Parse(str2));

                    output.Add(new(str1, str3));
                }

                throw new Exception("Test");
                return output.ToSeq();
            }
            catch (Exception ex)
            {
                return new Problem(ex.Message);
            }
        }
    }

    record Block(string Id, string Value);
    record Problem(string Message);

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
