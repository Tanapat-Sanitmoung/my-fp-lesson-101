using System.Text;
using System.Collections.Generic;
using System.Formats.Asn1;

namespace Tanapat.Playground;

public static class Lesson241028
{

    /*
        // I will create a mapping Pipeline, it will take input and output,
        // a => func1(a, b) => func2(a, c) => function(a, d) => a

        // 1. we can reference F# as a project reference in C#
       
        // 2. arrow function that create arrow function
        // Example:
        Func<int, int> add(int x) => y => x + y;
        var result = add(10)(5);

        // 3. create new implicit Operator
    */

    public static void Execute()
    {
        // var rawString = "0002010102110216478772000353320904155303920003533461531343007640052044640122151823000130810016A00000067701011201150107536000315010214KB0000019952850320KPS004KB00000199528531690016A00000067701011301030040214KB0000019952850420KPS004KB00000199528551430014A000000004101001064169710211123456789015204581453037645802TH5908D FRUITY6004CITY62250509433600972070842151823630462BA";
        // var rawString = "0002010102120216478772000363403104155303920003634291531343007640052044640101726324500130820016A000000677010112011501075360003150102154010172632450010320EDC1729835085284613531700016A000000677010113010300402154010172632450010420EDC1729835085284613551430014A00000000410100106416971021112345678901520458145303764540575.005802TH5907PATJAI56004CITY6225050944247985207086258940963041933";
        // var rawString = "0002010102110216478772000275885604155303920002759111531343007640052044640122089487900130810016A00000067701011201150107536000315080214KB0000019211490320KPS004KB00000192114931690016A00000067701011301030040214KB0000019211490420KPS004KB00000192114951430014A000000004101001064169710211123456789015204546253037645802TH5917BAANYIM RUKBAKERY6004CITY62250509100975157070842089487630441C1";
        var rawString = "0002020102111501A6001B63045567";
 
        // Usage Design 
        Read(rawString)
            .Map(s => Upsert(s.Blocks, "53", "768"))
            .Map(s => Upsert(s.Blocks, "54", "123.23"))
            .Map(s => Upsert(s.Blocks, "99", "123.23"))
            .Map(s => CalCrc(s.Blocks))
            .Map(s => s.Blocks.Iter(b => Console.WriteLine($"{b.Tag.Text} {b.Value}")))
            .IfLeft(u => Console.WriteLine(u.Remainder));

    }

    static Either<ReadErrorResult, ReadResult> Read(string source)
    {
        var blockList = new List<Block>();

        var index = 0;
        var lastIndex = source.Length - 1;

        const int TagLength = 2;
        const int LenLength = 2;

        Func<int, bool> NotEnd = idx => idx < lastIndex;

        Func<int, (int, string)> ReadId = (idx) => 
            (idx + TagLength, source.Substring(idx, TagLength));

        Func<int, (int, string)> ReadLen = (idx) => 
            (idx + LenLength, source.Substring(idx, LenLength));

        Func<int, (int, string)> ReadValue = (idx) => 
        {
            (idx, var strLen) = ReadLen(idx);

            var valLen = ushort.Parse(strLen);

            return (idx + valLen, source.Substring(idx, valLen));
        };

        try
        {
            while (NotEnd(index))
            {
                (index, var tag) = ReadId(index);
                (index, var value) = ReadValue(index);
                blockList.Add(new Block(tag, value));

            } //while 

            return new ReadResult(blockList.ToSeq());
        }
        catch  (Exception ex)
        {
            return new ReadErrorResult(blockList.ToSeq(), source.Substring(index), ex);
        }

    }

    static UpsertResult Upsert(Seq<Block> blocks, string targetTag, string value)
    {
        var output = blocks.ToList();
        
        var target = new DataObjectId(targetTag);
        var targetOrder = target.GetOrder();
        var lastIndex = output.Count - 1;

        // Start reading from Block 01
        for (int i = 1; i < lastIndex; i++)
        {
            var prev = output[i - 1];
            var curr = output[i];   

            if (curr.Tag == target)
            {
                output[i] = new Block(target, value);
                break;
            }

            if (curr.Tag.GetOrder() > targetOrder && prev.Tag.GetOrder() < targetOrder)
            {
                output.Insert(i, new Block(target, value));
                break;
            }
        }

        return new UpsertResult(output.ToSeq());
    }

    static CalCrcResult CalCrc(Seq<Block> blocks)
    {
        var sb = new StringBuilder();

        var output = new List<Block>();

        foreach (var b in blocks)
        {
            sb.Append($"{b.Tag.Text}{b.Value.Length:D2}");

            if (b.Tag.Text != "63")
            {
                sb.Append(b.Value);

                output.Add(b);
            }
            else
            {
                output.Add(new Block(b.Tag, EmvCrcCalculator.ComputeChecksum(sb.ToString())));
            }
        }        

        return new CalCrcResult(output.ToSeq());
    }

    record ReadResult(Seq<Block> Blocks);

    record UpsertResult(Seq<Block> Blocks);

    record CalCrcResult(Seq<Block> Blocks);

    record ReadErrorResult(Seq<Block> Blocks, string Remainder, Exception Exception);

    record DataObjectId
    {
        static readonly Dictionary<string, int> _TagOrders = 
            Enumerable.Range(0, 100)
                .ToDictionary(
                    keySelector: (v) => v.ToString("D2"), 
                    elementSelector: (v) => v switch { 63 => 999, _ => v});

        public DataObjectId(string text)
        {
            Text = _TagOrders.ContainsKey(text)
                    ? text
                    : throw new ArgumentOutOfRangeException("Value out of length expected 00 to 99");
        }

        public int GetOrder() =>  _TagOrders[Text];

        public string Text { get; } 
    }

    static class DataObjectLength
    {
        // Possible values are 00 to 99 
        static readonly System.Collections.Generic.HashSet<string> 
            _PossibleValues =  Enumerable.Range(0, 100)
                .Select(v => v.ToString("D2")).ToHashSet();

        public static bool IsPossibleValue(string value)
            => _PossibleValues.Contains(value);
            
    }

    record Block
    {
        public DataObjectId Tag { get; }
        public string Value { get; }

        public Block(DataObjectId tag, string value)
        {
            Tag = tag;
            Value = value;
        }

        public Block(string text, string value)
            : this(new DataObjectId(text), value)   
        {

        }
    }

    public static TDest Map<TSrc, TDest>(this TSrc @this, Func<TSrc, TDest> f) => f(@this);    


    public sealed class EmvCrcCalculator
    {
        private static readonly ushort[] s_table = new ushort[256];

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void Initialize()
        {
            const ushort Polynomial = 0x1021;
            ushort temp, a;
            for (var i = 0; i < s_table.Length; i++)
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
                s_table[i] = temp;
            }
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

public static class FuncExtensions
{
    public static Func<TA, TC> Apply<TA, TB, TC>(this Func<TA, TB> func1, Func<TB, TC> func2)
        => (a) => func2(func1(a));
}