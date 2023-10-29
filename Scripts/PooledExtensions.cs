using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace OneWinter.ScriptObjPoolingFramework
{
    public static class PooledExtensions
    {
        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> enumerable, int elementsCount)
        {
            return RandomizeEnumerable(enumerable).Take(elementsCount);
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
            return RandomizeEnumerable(enumerable).FirstOrDefault();
        }

        private static IEnumerable<T> RandomizeEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// Normal `if(GameObject)` syntax doesn't work with Object Pooling; we need to check if the GameObject is active too
        /// </summary>
        /// <param name="pooledObject">The Pooled Object to check for null and active</param>
        /// <returns>True if the Pooled Object is not null and is active</returns>
        public static bool IsActive(this PooledObjectBase pooledObject)
        {
            return pooledObject && pooledObject.isActiveAndEnabled;
        }

        /// <summary>
        /// Helper method to retrieve only Active pooled objects from a enumerable/list
        /// </summary>
        /// <param name="pooledObjects">The enumerable/list of objects to filter</param>
        /// <returns>The active pooled objects in the passed enumerable/list</returns>
        public static IEnumerable<T> GetActive<T>(this IEnumerable<T> pooledObjects) where T : PooledObjectBase
        {
            return pooledObjects.Where(obj => obj.IsActive());
        }

        public static float ActiveCount<T>(this IEnumerable<T> pooledObjects) where T : PooledObjectBase
        {
            return pooledObjects?.Count(obj => obj.IsActive()) ?? 0;
        }
    
        public static bool AnyActive<T>(this IEnumerable<T> pooledObjects) where T : PooledObjectBase
        {
            return pooledObjects.Any(obj => obj.IsActive());
        }
        
        public static T GetWrapped<T>(this T[] array, int index)
        {
            return array[((index % array.Length) + array.Length) % array.Length];
        }
    
        public static T GetWrapped<T>(this IList<T> array, int index)
        {
            return array[((index % array.Count) + array.Count) % array.Count];
        }

    }
}