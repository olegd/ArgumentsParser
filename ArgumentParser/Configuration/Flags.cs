using System.Collections.Generic;

namespace ArgumentParser.Configuration
{
    public class Flags
    {
        private readonly Dictionary<string, string> _synonymToArgumentMap = new Dictionary<string, string>();

        public void Add(string argument, params string[] synonyms)
        {
            _synonymToArgumentMap[argument] = argument;
            foreach (var synonym in synonyms)
            {
                if (string.IsNullOrWhiteSpace(synonym))
                {
                    continue;
                }
                var trimmedsynonym = synonym.Trim();
                _synonymToArgumentMap[trimmedsynonym] = argument;
            }
        }

        public string Resolve(string argument)
        {
            var trimmedArgument = argument.Trim();
            string result;
            if (_synonymToArgumentMap.TryGetValue(trimmedArgument, out result))
            {
                return result;
            }
            return null;
        }
    }
}