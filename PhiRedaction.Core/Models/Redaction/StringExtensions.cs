using System.Text.RegularExpressions;

namespace PhiRedaction.Core.Models.Redaction
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces multiple consecutive spaces with a single space.
        /// </summary>
        public static string NormalizeSpaces(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : Regex.Replace(input, @"[ ]{2,}", " ");
        }

        /// <summary>
        /// Replaces multiple consecutive newlines (including mixed \r\n and \n) with a single Environment.NewLine.
        /// </summary>
        public static string NormalizeNewlines(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : Regex.Replace(input, @"(\r\n|\r|\n){2,}", Environment.NewLine);
        }
    }
}
