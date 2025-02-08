namespace school_major_project.HelperClass
{
    public static class UrlHelper
    {
        public static string ToUrlSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            return text.Trim().ToLower().Replace(" ", "-");
        }

        public static string FromUrlSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return string.Empty;
            }

            return slug.Replace("-", " ");
        }
    }

}
