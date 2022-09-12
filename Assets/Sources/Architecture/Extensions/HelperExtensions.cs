using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sources.Architecture.Extensions
{
    public static class HelperExtensions
    {
        private static string[] _postfixes;
        public static string[] Postfixes => _postfixes ??= GetResourcePostfixes();

        public static string ToResourceFormat(this double value)
        {
            var thousandDigit = 0;
            while (value / 1000 >= 1)
            {
                value /= 1000;
                thousandDigit++;
            }

            return $"{value.ToString("###0.###", CultureInfo.InvariantCulture)}{Postfixes[thousandDigit]}";
        }

        private static string[] GetResourceStandartPostfixes()
        {
            return new string[]
            {
                "", " K", " M", " T"
            };
        }

        private static string[] GetResourcePostfixes()
        {
            int startIndex = 65;
            int lastIndex = 91;
            int length = lastIndex - startIndex;
            string[] generatedPostfixes = new string[length * length];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append((char)(i + startIndex));
                for (int j = 0; j < length; j++)
                {
                    stringBuilder.Append((char)(j + startIndex));
                    generatedPostfixes[i * length + j] = stringBuilder.ToString();
                    stringBuilder.Remove(2, 1);
                }

                stringBuilder.Remove(1, 1);
            }

            stringBuilder.Clear();

            var standartPostfixes = GetResourceStandartPostfixes();

            List<string> postfixes = new List<string>(generatedPostfixes.Length + standartPostfixes.Length);
            postfixes.AddRange(standartPostfixes);
            postfixes.AddRange(generatedPostfixes);

            return postfixes.ToArray();
        }
    }
}