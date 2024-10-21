using static Tanapat.FpPractices.Features.EMVQR;

var sample = "0002010102112632002816728000581200000000100000055204581253031445502015802LK5909Vits Food6007Colombo61050080062580032537c0a88562e4a599cab63d1992f0dac05181600766683296-000563042AB7";

var buildResult = 
        from blocks in CutQr(sample)
        from text in BuildOutput(blocks)
        from html in BuildHtmlOutput(blocks)
        select (text, html);

buildResult.Match(
    HandleSuccess,
    HandleFail
);

Console.WriteLine("Press any key to continue");
Console.Read();

static void HandleSuccess((string text, string html) output)
{
    Console.WriteLine($"success: {Environment.NewLine}{output.text}");
    Console.WriteLine($"success: {Environment.NewLine}{output.html}");
}

static void HandleFail(Exception ex)
{
    Console.WriteLine($"error: {ex.Message}");
}