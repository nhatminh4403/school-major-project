using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace school_major_project.HelperClass
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Replace("Đ", "D-special-")
              .Replace("đ", "d-special-");

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            text = new string(chars).Normalize(NormalizationForm.FormC);

            // Then replace spaces with dashes
            text = text.Replace(" ", "-");
            text = text.Replace("D-special-", "d")
                             .Replace("d-special-", "d");

            // Optionally: remove other special characters and convert to lowercase
            text = Regex.Replace(text, @"[^a-zA-Z0-9\-]", "");
            text = text.ToLowerInvariant();

            return text;
        }
        
    }

}
