namespace ArgumentParser
{
    public static class ExtensionMethods
    {
        public static string With(this string template, params string[] formatArgs)
        {
            return string.Format(template, formatArgs);
        }

        public static string SafeToString(this object instance)
        {
            if (instance == null)
            {
                return "Null";
            }
            return instance.ToString();
        }
    }
}