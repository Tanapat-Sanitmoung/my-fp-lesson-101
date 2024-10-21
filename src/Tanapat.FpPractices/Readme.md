
code:
```C#
using static Tanapat.FpPractices.Features.EMVQR;

var sample = "0002010102112632002816728000581200000000100000055204581253031445502015802LK5909Vits Food6007Colombo61050080062580032537c0a88562e4a599cab63d1992f0dac05181600766683296-000563042AB7";

CutQr(sample)
    .Bind(BuildOutput)
    .Match(
        Succ: (output) => Console.WriteLine("output: {0}", output),
        Fail: (ex) => Console.WriteLine("error: {0}", ex.Message));

Console.WriteLine("Press any key to continue");
Console.Read();
```

output:
```console
dotnet run
output: 00 02 01
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

--

code:
```C#
using static Tanapat.FpPractices.Features.EMVQR;

var sample = "0002010102112632002816728000581200000000100000055204581253031445502015802LK5909Vits Food6007Colombo61050080062580032537c0a88562e4a599cab63d1992f0dac05181600766683296-000563042AB7";

CutQr(sample)
    .Bind(BuildHtmlOutput)
    .Match(
        Succ: (output) => File.WriteAllText("output.html", output),
        Fail: (ex) => Console.WriteLine("error: {0}", ex.Message));

Console.WriteLine("Press any key to continue");
Console.Read();
```

[Output file](output.html)