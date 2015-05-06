using System;
using System.IO;
using System.Text;

namespace Windy
{
    internal static class Utilities
    {
        public static string LongToString(long number)
        {
            var alphabet =
                new[]
                {
                    '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                    'v', 'w', 'x', 'y', 'z'
                };

            var output = new StringBuilder();

            while (number > 0)
            {
                long digit;
                number = Math.DivRem(number, alphabet.Length, out digit);
                output.Append(alphabet[digit]);
            }

            return output.ToString();
        }

        public static string GenerateTempFileName(string prefix = null, long? nonce = null)
        {
            if (nonce == null)
            {
                nonce = DateTime.Now.Ticks;
            }

            return Path.Combine(Path.GetTempPath(),
                                "Windy_" + (!string.IsNullOrWhiteSpace(prefix) ? prefix + "_" : "") + LongToString(nonce.Value) + ".json");
        }

        public static void WriteFile(string fileName, string data)
        {
            using (var sw = new StreamWriter(File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)))
            {
                sw.Write(data);
            }        
        }
    }
}
