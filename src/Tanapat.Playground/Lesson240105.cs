using System.Text;

namespace Tanapat.Playground;

public class Lesson241105
{
    public static void Execute()
    {
        var rawString = "0002020102111501A6001B6304D546";

        BlockReader.Read(rawString)
            .Bind(BlocksValidator.Validate)
            .Match(
                Succ: HandleSuccess,
                Fail: HandleFail
            );
    }

    static void HandleSuccess(ValidBlocks validBlocks)
        => validBlocks.Blocks.Iter(b => Console.WriteLine($"{b.Id} {b.Value}"));

    static void HandleFail(Exception ex)
         => Console.WriteLine("Problem : {0}", ex.Message);

    record Block(string Id, string Value)
    {
        private static readonly System.Collections.Generic.HashSet<string> s_IdTable = 
            Enumerable.Range(start: 0, count:100)
                .Select(v => v.ToString("D2"))
                .ToHashSet();

        private readonly string _1 = s_IdTable.Contains(Id) 
            ? Id 
            : throw new ArgumentException($"Possible values are 00 to 99 but found [{Id}]", nameof(Id));
    }

    record RawBlocks(Seq<Block> Blocks);

    record ValidBlocks(Seq<Block> Blocks);

    class BlocksValidator
    {
        private static string GetStringForCrc(Seq<Block> blocks)
            => blocks.Fold(
                state: InitAppender(),
                f: AppendBlockString
            ).ToString();

        private static StringBuilder InitAppender() => new();

        private static StringBuilder AppendBlockString(StringBuilder appender, Block block)
            =>  appender.Append(BlocksToString(block));

        private static string BlocksToString(Block block)
            => block.Id switch {
                "63" => BlockToStringWithNoValue(block),
                _ => BlockToStringFull(block),
            };

        private static string BlockToStringFull(Block b) => $"{b.Id}{b.Value.Length:D2}{b.Value}";

        private static string BlockToStringWithNoValue(Block b) => $"{b.Id}{b.Value.Length:D2}";

        private static string CalculateCrc(Seq<Block> blocks)
            => EmvCrcCalculator.ComputeChecksum(dataWithoutCrcValue: GetStringForCrc(blocks));

        public static Try<ValidBlocks> Validate(RawBlocks rawBlock)
        {
            return Try(() => 
            {
                var calculatedCrc = CalculateCrc(rawBlock.Blocks);
                var sourceCrc = rawBlock.Blocks.Last().Value;
               
                return sourceCrc == calculatedCrc 
                        ? new ValidBlocks(rawBlock.Blocks)
                        : throw new Exception($"Invalid CRC [Src:={sourceCrc}, Calculated:={calculatedCrc}]");
                
            });
        }
    }

    class BlockReader 
    {
        public static Try<RawBlocks> Read(string source)
            => new BlockReader()
                .InstanceRead(source);

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

        private Block ReadBlock()
            => new(ReadId(), ReadValue());

        private bool NotEnd()
            => _index < _source.Length;

        private void Init(string source)
            => (_source, _index) = (source, 0);

        private BlockReader() {}

        private Try<RawBlocks> InstanceRead(string source)
            => Try(() => 
            {
                Init(source);

                var appender = new List<Block>();

                while (NotEnd())
                {
                    appender.Add(ReadBlock());
                }

                return new RawBlocks(appender.ToSeq());
            });
    }

    public sealed class EmvCrcCalculator
    {
        private static readonly ushort[] s_table = InitializeTable();
        private static ushort[] InitializeTable()
        {
            var table = new ushort[256];

            const ushort Polynomial = 0x1021;
            ushort temp, a;
            for (var i = 0; i < table.Length; i++)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (var j = 0; j < 8; j++)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ Polynomial);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            return table;
        }

        public static string ComputeChecksum(string dataWithoutCrcValue)
        {
            ushort crc = 0xffff;
            var bytes = Encoding.UTF8.GetBytes(dataWithoutCrcValue);
            for (var i = 0; i < bytes.Length; i++)
            {
                crc = (ushort)((crc << 8) ^ s_table[(crc >> 8) ^ (0xff & bytes[i])]);
            }
            return crc.ToString("X4");
        }
    }
}