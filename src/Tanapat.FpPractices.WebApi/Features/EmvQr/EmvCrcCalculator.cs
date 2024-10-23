using System.Text;

namespace Tanapat.FpPractices.WebApi.Features.EmvQr;

public sealed class EmvCrcCalculator
{
    private static readonly ushort[] s_table = new ushort[256];

    [System.Runtime.CompilerServices.ModuleInitializer()]
    internal static void Initialize()
    {
        const ushort Polynomial = 0x1021;
        ushort temp, a;
        for (var i = 0; i < s_table.Length; i++)
        {
            temp = 0;
            a = (ushort)(i << 8);
            for (var j = 0; j < 8; j++)
            {
                if (((temp ^ a) & 0x8000) != 0)
                    temp = (ushort)((temp << 1) ^ Polynomial);
                else
                    temp <<= 1;
                a <<= 1;
            }
            s_table[i] = temp;
        }
    }

    public static string ComputeChecksum(string dataWithoutCrcValue)
    {
        ushort crc = 0xffff;
        var bytes = Encoding.UTF8.GetBytes(dataWithoutCrcValue);
        for (var i = 0; i < bytes.Length; i++)
        {
            crc = (ushort)((crc << 8) ^ s_table[(crc >> 8) ^ (0xff & bytes[i])]);
        }
        return crc.ToString("X4");
    }
}
