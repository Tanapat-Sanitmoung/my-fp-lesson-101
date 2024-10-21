using static Tanapat.FpPractices.Features.EMVQR;

var sample = "0002010102112632002816728000581200000000100000055204581253031445502015802LK5909Vits Food6007Colombo61050080062580032537c0a88562e4a599cab63d1992f0dac05181600766683296-000563042AB7";

CutQr(sample)
    .Bind(BuildOutput)
    .Match(
        Succ: (output) => Console.WriteLine("output: {0}", output),
        Fail: (ex) => Console.WriteLine("error: {0}", ex.Message));

Console.WriteLine("Press any key to continue");
Console.Read();