
code:
```C#
using LanguageExt;
using Tanapat.FpPractices.Features;
using static Tanapat.FpPractices.Features.EMVQR;

var buildResult = 
    from qrCode in ReadFromImage("qr.png")
    from _1 in Obeserv("After ReadFromImage")
    from blocks in CutQr(qrCode)
    from _2 in Obeserv("After CutQr")
    from textOutput in BuildOutput(blocks, before: AddMainTagComment)
    from _3 in Obeserv("After BuildOutput")
    select textOutput;

buildResult.Match(HandleSuccess, HandleFail);

Console.WriteLine("Press any key to continue");
Console.Read();

static void HandleSuccess(string output)
{
    Console.WriteLine($"success: {Environment.NewLine}{output}");
}

static void HandleFail(Problem ex)
{
    Console.WriteLine($"error: {ex.Message}");
}

static Either<Problem, Unit> Obeserv(string message) 
{
    Console.WriteLine(message);
    return Unit.Default; 
}
```

output:
```console
dotnet run
After ReadFromImage
After CutQr
After BuildOutput
success: 
00 02 01
//Dynamic QR
01 02 11
26 32 00281672800058120000000010000005
52 04 5812
53 03 144
55 02 01
58 02 LK
59 09 Vits Food
60 07 Colombo
61 05 00800
62 58 0032537c0a88562e4a599cab63d1992f0dac05181600766683296-0005
63 04 2AB7

Press any key to continue
```