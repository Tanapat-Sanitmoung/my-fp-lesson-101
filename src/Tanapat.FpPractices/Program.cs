using System.Linq;
using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;

var sample = "0002010102112632002816728000581200000000100000055204581253031445502015802LK5909Vits Food6007Colombo61050080062580032537c0a88562e4a599cab63d1992f0dac05181600766683296-000563042AB7";

CutQr(sample)
    .Bind(BuildOutput)
    .Match(
        Succ: (output) => Console.WriteLine("output: {0}", output),
        Fail: (ex) => Console.WriteLine("error: {0}", ex.Message));

Console.WriteLine("Press any key to continue");
Console.Read();

static Try<string[]> CutQr(string data)
{
    return Try(() => {
        var blocks = new List<string>();

        var index = 0;
        var lastIndex = data.Length;

        const int TagLength = 2;
        const int LengthLength = 2;

        while(index < lastIndex)
        {
            var tag = data.Substring(index, TagLength);

            index += TagLength;

            var lenStr = data.Substring(index, LengthLength);

            index += LengthLength;

            var len = ushort.Parse(lenStr);

            var value = data.Substring(index, len);

            index += len;

            blocks.AddRange([tag, lenStr, value]);
        }

        return blocks.ToArray();
    });
}

static Try<string> BuildOutput(string[] blocks)
{
    return Try(() => {
        var builder = new StringBuilder();

        for(int i = 0; i < blocks.Length; i += 3)
        {
            builder.AppendLine($"{blocks[i]} {blocks[i +1]} {blocks[i +2]}");
        }

        return builder.ToString();
    });
}