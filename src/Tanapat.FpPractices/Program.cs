using LanguageExt;
using Tanapat.FpPractices.Features;
using static Tanapat.FpPractices.Features.EMVQR;

var buildResult = 
    from qrCode in ReadFromImage("EMV-25671023161940885.jpg")
    from blocks in CutQr(qrCode)
    from textOutput in BuildOutput(blocks, before: AddMainTagComment)
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