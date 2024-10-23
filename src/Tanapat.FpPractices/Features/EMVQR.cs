using System.Text;
using LanguageExt;
using SkiaSharp;
using ZXing.SkiaSharp;

namespace Tanapat.FpPractices.Features;

public record Problem(string Message);
public record QrBlock(string Tag, string Len, string Value);

public static class EMVQR
{
    public static Either<Problem, string> ReadFromImage(string file)
    {
        try
        {
            using var stream = File.OpenRead(file);
            using var bitmap = SKBitmap.Decode(stream);

            var reader = new BarcodeReader();
            var decodeResult = reader.Decode(bitmap);

            if (decodeResult == null)
            {
                return new Problem("Could not decode input file");
            }

            return decodeResult.Text;

        }
        catch (Exception ex)
        {
            return new Problem(ex.Message);
        }
    }

    public static Either<Problem, Seq<QrBlock>> CutQr(string data)
    {
        try 
        {
            var blocks = new List<QrBlock>();

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

                blocks.Add(new QrBlock(tag, lenStr, value));
            }

            return blocks.ToSeq();
        }
        catch(Exception ex)
        {
            return new Problem($"{nameof(CutQr)} failed:= {ex.Message}");
        }
    }

    public static Either<Problem, string> BuildOutput(
        Seq<QrBlock> blocks, 
        Action<StringBuilder, QrBlock>? before = null,
        Action<StringBuilder, QrBlock>? after = null)
    {
        try {
            var builder = new StringBuilder();

            foreach(var b in blocks)
            {
                before?.Invoke(builder, b);

                builder.AppendLine($"{b.Tag} {b.Len} {b.Value}");

                after?.Invoke(builder, b);
            }

            return builder.ToString();
        }
        catch(Exception ex)
        {
            return new Problem($"{nameof(BuildOutput)} failed:= {ex.Message}");
        }
    }

    public static void AddMainTagComment(
        StringBuilder builder,
        QrBlock qrBlock
    )
    {
        try
        {
            if(qrBlock.Tag == "01")
            {
                _ = qrBlock.Value switch
                {
                    "11" => builder.AppendLine("//Dynamic QR"),
                    "12" => builder.AppendLine("//Static QR"),
                    _ => default
                };
            }
        }
        catch
        {
            //ignore error
        }
    }
}