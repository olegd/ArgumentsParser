namespace ArgumentParser
{
    public static class ExtensionMethods
    {
        public static string With(this string template, params string[] formatArgs)
        {
            return string.Format(template, formatArgs);
        }
    }
}