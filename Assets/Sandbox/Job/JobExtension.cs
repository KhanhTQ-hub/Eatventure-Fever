using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace kt.job.core
{
    public static class JobExtension
    {
        public static NativeArray<T> ToNativeArray<T>(this T[] array, Allocator allocator = Allocator.Persistent) where T : struct
        {
            var input = new NativeArray<T>(array.Length, allocator);
            for (var i = 0; i < input.Length; i++)
                input[i] = array[i];

            return input;
        }

        public static NativeArray<T> ToNativeArray<T>(this IEnumerable<T> enumerable, Allocator allocator = Allocator.Persistent) where T : struct
        {
            var input = new NativeArray<T>(enumerable.Count(), allocator);
            var index = 0;
            foreach (var item in enumerable)
            {
                input[index] = item;
                index++;
            }

            return input;
        }
    }
}
