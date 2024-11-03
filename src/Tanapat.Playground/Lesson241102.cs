
namespace Tanapat.Playground;

public static class Lesson241102
{
    public static void Execute()
    {
        // var rawString = "0002010102110216478772000353320904155303920003533461531343007640052044640122151823000130810016A00000067701011201150107536000315010214KB0000019952850320KPS004KB00000199528531690016A00000067701011301030040214KB0000019952850420KPS004KB00000199528551430014A000000004101001064169710211123456789015204581453037645802TH5908D FRUITY6004CITY62250509433600972070842151823630462BA";
        // var rawString = "0002010102120216478772000363403104155303920003634291531343007640052044640101726324500130820016A000000677010112011501075360003150102154010172632450010320EDC1729835085284613531700016A000000677010113010300402154010172632450010420EDC1729835085284613551430014A00000000410100106416971021112345678901520458145303764540575.005802TH5907PATJAI56004CITY6225050944247985207086258940963041933";
        // var rawString = "0002010102110216478772000275885604155303920002759111531343007640052044640122089487900130810016A00000067701011201150107536000315080214KB0000019211490320KPS004KB00000192114931690016A00000067701011301030040214KB0000019211490420KPS004KB00000192114951430014A000000004101001064169710211123456789015204546253037645802TH5917BAANYIM RUKBAKERY6004CITY62250509100975157070842089487630441C1";
        string rawString = "0002020102111501A6001B63045567";

        var reader = new StrReader(rawString);

        while (reader.NotAtEnd())
        {
            var str1 = reader.Read(2);
            var str2 = reader.Read(2);
            var str3 = reader.Read(ushort.Parse(str2));

            Console.WriteLine($"{str1,-4}{str3,-50}");
        }

    }

    class StrReader
    {
        private readonly string _str;
        private int _idx = 0;

        public StrReader(string str) => _str = str;

        public bool NotAtEnd() => _idx < _str.Length;

        public string Read(int count) => 
            (_str.Substring(_idx, count), _idx+= count).Item1;
    }

}