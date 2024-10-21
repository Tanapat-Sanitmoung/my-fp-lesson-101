using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Tanapat.FpPractices.Features;

public static class EMVQR
{
        
    public static Try<string[]> CutQr(string data)
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

    public static Try<string> BuildOutput(string[] blocks)
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
}