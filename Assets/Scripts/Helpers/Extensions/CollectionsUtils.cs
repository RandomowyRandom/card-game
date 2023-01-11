using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers.Extensions
{
    public static class CollectionsUtils
    {
        private static Random _random = new();
        
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            return list[_random.Next(0, list.Count)];
        }
    }
}