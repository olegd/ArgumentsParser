using System;

namespace ArgumentParser.Configuration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DefineSynonymAttribute : Attribute
    {
        public string ArgumentName;
        public string Synonyms;

        public DefineSynonymAttribute(string argumentName)
        {
            ArgumentName = argumentName;
        }
    }
}