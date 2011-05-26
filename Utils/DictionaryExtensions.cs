using System.Collections.Generic;

namespace Utilities
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<T, K>(this Dictionary<T, K> dictionary, T key, K value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}