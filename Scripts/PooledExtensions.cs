using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace OneWinter.ScriptObjPoolingFramework
{
    public static class PooledExtensions
    {
        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(_ => Guid.NewGuid()).Take(elementsCount);
        }

        public static T RandomItem<T>(this T[] array)
        {
            return array.Length == 0 ? default : array[Random.Range(0, array.Length)];
        }

        public static T RandomItem<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list[Random.Range(0, list.Count)];
        }

        public static T RandomItem<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ToArray().RandomItem();
        }

        /// <summary>
        ///     Normal `if(GameObject)` syntax doesn't work with Object Pooling; we need to check if the GameObject is active too
        /// </summary>
        /// <param name="obj">The Pooled Object to check for null and active</param>
        /// <returns>True if the Pooled Object is not null and is active</returns>
        public static bool IsActive(this PooledObjectBase obj)
        {
            return obj && obj.isActiveAndEnabled;
        }
    }
}