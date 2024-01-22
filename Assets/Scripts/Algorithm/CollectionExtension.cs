using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Algorithm
{
    public static class CollectionExtension
    {
   
        public static T RandomElement<T>(this IEnumerable<T> o)
        {
            var enumerable = o as T[] ?? o.ToArray();
            var count = enumerable.Count();
            return count == 1 ? enumerable.First() : enumerable.ElementAt(Random.Range(0, count));
        }
        
        
        // 洗牌
        // ReSharper disable once IdentifierTypo
        public static void KnuthDurstenfeldShuffle<T>(this IList<T> list)
        {
            //随机交换
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var currentIndex = Random.Range(0, i+1);
                (list[currentIndex], list[i]) = (list[i], list[currentIndex]);
            }
        }
        
        // 洗牌
        // ReSharper disable once IdentifierTypo
        public static IList<T> Shuffled<T>(this IList<T> list)
        {
            var newList = list;
            //随机交换
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var currentIndex = Random.Range(0, i+1);
                (newList[currentIndex], newList[i]) = (newList[i], newList[currentIndex]);
            }

            return newList;
        }
    
    }
    
    
    public static class ObjectExtension
    {
   
        public static void RemoveAllObjects<T>(this List<T> o) where T: Object
        {
            o.ForEach(Object.Destroy);
            o.Clear();
        }
    
    }
}