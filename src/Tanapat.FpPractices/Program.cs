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