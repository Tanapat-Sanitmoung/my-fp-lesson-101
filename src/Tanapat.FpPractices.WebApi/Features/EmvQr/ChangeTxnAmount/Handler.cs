using FastEndpoints;

namespace Tanapat.FpPractices.WebApi.Features.EmvQr.ChangeTxnAmount;

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
