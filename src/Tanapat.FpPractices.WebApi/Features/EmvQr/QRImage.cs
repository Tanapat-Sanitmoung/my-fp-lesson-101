using LanguageExt;
using SkiaSharp;
using ZXing.SkiaSharp;

namespace Tanapat.FpPractices.WebApi.Features.EmvQr;

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
