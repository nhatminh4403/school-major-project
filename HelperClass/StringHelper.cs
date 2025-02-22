using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace school_major_project.HelperClass
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string GenerateSlug(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            // Remove diacritics
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder slug = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    slug.Append(c);
            }

            // Convert to lower case
            string result = slug.ToString().Normalize(NormalizationForm.FormC).ToLower();

            // Replace spaces with hyphens
            result = Regex.Replace(result, @"\s+", "-");

            // Remove invalid characters
            result = Regex.Replace(result, @"[^a-z0-9\-]", "");

            // Remove multiple hyphens
            result = Regex.Replace(result, @"-+", "-");

            // Trim hyphens from ends
            result = result.Trim('-');

            return result;
        }

        public static string RemoveSlug(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return "";

            // Replace hyphens with spaces
            return slug.Replace("-", " ");
        }
    }

}
