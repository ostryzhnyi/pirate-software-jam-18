using System.Collections.Generic;

namespace ProjectX.CodeBase.Utils
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> range)
        {
            foreach (var item in range)
            {
                if (!dictionary.ContainsKey(item.Key))
                    dictionary.Add(item);
            }
            
            foreach (var item in dictionary)
            {
                if (!dictionary.ContainsKey(item.Key))
                    dictionary.Add(item);
            }
        }
        public static void AddRangeReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IReadOnlyDictionary<TKey, TValue> range)
        {
        
            foreach (var item in range)
            {
                if (!dictionary.ContainsKey(item.Key))
                    dictionary.Add(item);
            }
            
            foreach (var item in dictionary)
            {
                if (!dictionary.ContainsKey(item.Key))
                    dictionary.Add(item);
            }
        }
    }
}