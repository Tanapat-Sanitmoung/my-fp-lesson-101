using System.Collections;

namespace Tanapat.Playground;

public class Lesson241105
{
    public static void Execute()
    {
        var rawString = "xx02020102111501A6001B63045567";

        BlockReader.Read(rawString, (id, value) => new Block(id, value))
            .Bind(blocks => Try(() => blocks))
            .Match(
                Succ: s => s.Iter(b => Console.WriteLine($"{b.Id} {b.Value}")),
                Fail: ex => Console.WriteLine("Problem : {0}", ex.Message)
            );
    }

    record Block(string Id, string Value)
    {
        private static System.Collections.Generic.HashSet<string> s_IdTable = 
            Enumerable.Range(start: 0, count:100)
                .Select(v => v.ToString("D2"))
                .ToHashSet();

        private readonly string _1 = s_IdTable.Contains(Id) 
            ? Id 
            : throw new ArgumentException("Possible values are 00 to 99", nameof(Id));

        private readonly string _2 = Value ?? throw new ArgumentNullException(nameof(Value));
    }

    class BlockReader 
    {
        public static Try<Seq<T>> Read<T>(string source, Func<string, string, T> convertor)
            => new BlockReader().InstanceRead(source, convertor);

        private int _index;
        private string _source = string.Empty;

        private string Read(int len) 
            => (_source.Substring(_index,  len), _index+= len).Item1;

        private string ReadId()
            => Read(2);

        private int ReadValueLen()
            => int.Parse(Read(2));

        private string ReadValue()
            => Read(ReadValueLen());

        private T ReadBlock<T>(Func<string, string, T> createBlock)
            => createBlock(ReadId(), ReadValue());

        private bool NotEnd()
            => _index < _source.Length;

        private void Init(string source)
            => (_source, _index) = (source, 0);

        private BlockReader() {}

        private Try<Seq<T>> InstanceRead<T>(string source, Func<string, string, T> convertor)
            => Try(() => 
            {
                Init(source);

                var appender = new List<T>();

                while (NotEnd())
                {
                    appender.Add(ReadBlock(convertor));
                }

                return appender.ToSeq();
            });
    }
}