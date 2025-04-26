using System.Globalization;
using System.Text;

namespace school_major_project.HelperClass
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            foreach (char c in normalizedString)
            {
                System.Globalization.UnicodeCategory category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            result = result.Replace("đ", "d").Replace("Đ", "D");
            return result;
        }
        public static bool IsNumeric(string value)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        // Equivalent to capitalizeName (more idiomatic C# naming)
        public static string Capitalize(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            return name.ToUpperInvariant();
        }
    }

}
