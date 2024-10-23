using System.Text;
using FastEndpoints;
using LanguageExt;
using SkiaSharp;
using ZXing.SkiaSharp;

namespace Tanapat.FpPractices.WebApi.Features.EmvQr;

public record Problem(string Message);

public record Request(IFormFile File, decimal NewAmount);

public sealed class Handler : Endpoint<Request>
{
    public override void Configure()
    {
        Post("qr-code/change-amount");

        AllowFileUploads();

        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        const string TxnAmountTag = "54";

        var result =
            from qrInfo in QRImage.ReadFromStream(req.File.OpenReadStream())
            let newValue = req.NewAmount.ToString()
            from newQrCode in EMVQR.InsertOrUpdateBlock(qrInfo.QrCode, TxnAmountTag, newValue, recalculateCRC: true)
            let newQrInfo = new QRInfo(newQrCode, qrInfo.Width, qrInfo.Height)
            from outputStream in QRImage.WriteToStream(newQrInfo)
            let fileName = $"EMV-{DateTime.Now:yyyyMMddHHmmssfff}.jpg"
            select (outputStream, fileName);

        await result.Match(
            s => SendStreamAsync(s.outputStream, s.fileName),
            p => SendStringAsync(p.Message)
        );
    }

}

public static class EMVQR
{
    public static Either<Problem, string> InsertOrUpdateBlock(
        string source, string targetTag, string newValue, bool recalculateCRC = false)
    {
        try
        {

            var targetTagNum = ushort.Parse(targetTag);
            var previousTagNum = ushort.MaxValue;
            var currentTagNum = ushort.MinValue;

            var builder = new StringBuilder(source);

            var qrCodeSpan = source.AsSpan();

            var index = 0;
            var lastIndex = qrCodeSpan.Length - 1;

            const int TagLength = 2;
            const int LenLength = 2;

            while (index < lastIndex)
            {
                var currentTagIndex = index;
                var currentTag = qrCodeSpan.Slice(index, TagLength);
                index += TagLength;

                var currentLen = qrCodeSpan.Slice(index, LenLength);
                index += LenLength;

                var valueLength = ushort.Parse(currentLen);
                index += valueLength; //no need to read old value

                // keep track Tag number
                currentTagNum = ushort.Parse(currentTag);

                // remove and insert
                if (targetTagNum == currentTagNum)
                {
                    var idx = currentTagIndex + 2;
                    var removeLength = LenLength + valueLength;

                    builder
                        .Remove(idx, removeLength)
                        .Insert(idx, $"{newValue.Length:D2}{newValue}");

                    break;
                }

                // insert
                if (targetTagNum > previousTagNum && targetTagNum < currentTagNum)
                {
                    builder.Insert(currentTagIndex, $"{targetTag}{newValue.Length:D2}{newValue}");
                    break;
                }

                // keep trace previous Tag number
                previousTagNum = currentTagNum;

            } //while 

            if (recalculateCRC)
            {
                var qrWithNoCrcValue = builder.ToString(0, builder.Length - 4);
                var crc = EmvCrcCalculator.ComputeChecksum(qrWithNoCrcValue);
                builder.Append(crc);
            }
            
            return builder.ToString();  

        }
        catch (Exception ex)
        {
            return new Problem($"{nameof(InsertOrUpdateBlock)} failed:= {ex.Message}");
        }
    }

}

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

public record QRInfo(string QrCode, int Width, int Height);

public static class QRImage
{
    public static Either<Problem, QRInfo> ReadFromFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new Problem("Filed doesn't exists");
        }

        try
        {
            using var stream = File.OpenRead(fileName);

            return ReadFromStream(stream);
        }
        catch (Exception ex)
        {
            return new Problem(ex.Message);
        }
    }

    public static Either<Problem, QRInfo> ReadFromStream(Stream stream)
    {
        using var bitmap = SKBitmap.Decode(stream);

        if (bitmap == null)
        {
            return new Problem("Failed to decode stream");
        }

        var reader = new BarcodeReader();
        var decodeResult = reader.Decode(bitmap);

        return decodeResult is not null
            ? new QRInfo(decodeResult.Text, bitmap.Width, bitmap.Height)
            : new Problem("Failed to decode bitmap");
    }

    public static Either<Problem, Stream> WriteToStream(QRInfo qrInfo)
    {
        try
        {
            var writer = new BarcodeWriter()
            {
                Options = new ZXing.Common.EncodingOptions()
                {
                    Margin = 1,
                    Width = qrInfo.Width,
                    Height = qrInfo.Height
                },
                Format = ZXing.BarcodeFormat.QR_CODE
            };

            using var bitmap = writer.Write(qrInfo.QrCode);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);

            return data.AsStream();
        }
        catch (Exception ex)
        {
            return new Problem(ex.Message);
        }
    }
}
