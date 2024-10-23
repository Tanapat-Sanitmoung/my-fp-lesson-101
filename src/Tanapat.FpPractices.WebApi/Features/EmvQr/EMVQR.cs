using System.Text;
using LanguageExt;

namespace Tanapat.FpPractices.WebApi.Features.EmvQr;

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
