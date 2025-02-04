using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Utilities
{
    public static class CollectionUtility
    {
        public static void AddItem<TKey,TValue>(this SerializableDictionary<TKey, List<TValue>> serializableDictionary, TKey key, TValue value)
        {
            if (serializableDictionary.ContainsKey(key)) {
                serializableDictionary[key].Add(value);
                
                return;
            }

            serializableDictionary.Add(key, new List<TValue>() { value });
        }
    }
}