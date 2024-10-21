using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Tanapat.FpPractices.Features;

public static class EMVQR
{
        
    public static Either<Exception, Seq<string>> CutQr(string data)
    {
        try 
        {
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

            return Right<Exception, Seq<string>>(blocks.ToSeq());
        }
        catch(Exception ex)
        {
            return Left<Exception, Seq<string>>(ex);
        }
    }

    public static Either<Exception, string> BuildOutput(Seq<string> blocks)
    {
        try {
            var builder = new StringBuilder();

            for(int i = 0; i < blocks.Length; i += 3)
            {
                builder.AppendLine($"{blocks[i]} {blocks[i +1]} {blocks[i +2]}");
            }

            return Right<Exception, string>(builder.ToString());
        }
        catch(Exception ex)
        {
            return Left<Exception, string>(ex);
        }
    }

    public static Either<Exception, string> BuildHtmlOutput(Seq<string> blocks)
    {
        try 
        {
            var builder = new StringBuilder();

            builder.AppendLine("<html>");

            builder.AppendLine(@"<style type=""text/css""> .qr-tag { color: blue } .qr-len { color: red} .qr-value { color: black } </style>");

            builder.AppendLine("<pre>");

            for(int i = 0; i < blocks.Length; i += 3)
            {
                builder.AppendLine($"<text class='qr-tag'>{blocks[i]}</text> <text class='qr-len'>{blocks[i +1]}</text> <text class='qr-value'>{blocks[i +2]}</text>");
            }

            builder.AppendLine("</pre>");

            builder.AppendLine("</html");

            return Right<Exception, string>(builder.ToString());
        }
        catch (Exception ex)
        {
            return Left<Exception, string>(ex);
        }
    }
}