using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace kt.job.math
{
    public static class MathExtension
    {
        #region Rate Array

        public static float[] ToRateArray(this float[] input) => new JobRateArray().Execute(input);
        public static List<float> ToRateArray(this List<float> input) => new JobRateArray().Execute(input);
        public static int[] ToRateArray(this int[] input) => new JobRateArray().Execute(input);
        public static List<int> ToRateArray(this List<int> input) => new JobRateArray().Execute(input);
        public static int RandomIndexRateArray(this List<float> rate)
        {
            if (rate == null || rate.Count == 0)
                return -1;

            var random = Random(0, rate[rate.Count - 1]);
            //Debug.Log(rate[rate.Count - 1]);

            for (var i = 0; i < rate.Count; i++)
                if (rate[i] >= random)
                    return i;

            return 0;
        }
        public static int RandomIndexRateArray(this IList<int> rate)
        {
            if (rate == null || rate.Count == 0)
                return -1;

            float random = Random(0, rate[rate.Count - 1]);
            //Debug.Log(rate[rate.Count - 1]);
            for (var i = 0; i < rate.Count; i++)
                if (rate[i] >= random)
                    return i;

            return 0;
        }
        public static int RandomIndexRate(this List<float> list) => list.ToRateArray().RandomIndexRateArray();
        public static int RandomIndexRate(this List<int> list) => list.ToRateArray().RandomIndexRateArray();

        #endregion

        #region Sum

        public static float Sum(NativeArray<float> input, NativeArray<float> output) => new JobSum().Execute(input, output);
        public static double Sum(NativeArray<double> input, NativeArray<double> output) => new JobSum().Execute(input, output);
        public static int Sum(NativeArray<int> input, NativeArray<int> output) => new JobSum().Execute(input, output);
        public static float Sum(this float[] array) => new JobSum().Execute(array);
        public static float Sum(this IEnumerable<float> array) => new JobSum().Execute(array);
        public static double Sum(this double[] array) => new JobSum().Execute(array);
        public static double Sum(this IEnumerable<double> array) => new JobSum().Execute(array);
        public static int Sum(this int[] array) => new JobSum().Execute(array);
        public static int Sum(this IEnumerable<int> array) => new JobSum().Execute(array);

        #endregion

        #region Random

        public static int Random(int from, int to) => new JobRandom().Execute(from, to);
        public static float Random(float from, float to) => new JobRandom().Execute(from, to);
        public static double Random(double from, double to) => new JobRandom().Execute(from, to);
        public static int RandomTo(this int from, int to) => Random(from, to);
        public static float RandomTo(this float from, float to) => Random(from, to);
        public static double RandomTo(this double from, double to) => Random(from, to);
        public static int Random(this Vector2Int vector) => Random(vector.x, vector.y);
        public static float Random(this Vector2 vector) => Random(vector.x, vector.y);
        public static bool RandomBool() => new JobRandom().Execute(0, 2) == 0;
        public static bool RandomInRange(this float range) => Random(0f, 1f) <= range;
        public static bool RandomOutRange(this float range) => !RandomInRange(range);
        public static bool RandomInRange(this double range) => Random(0f, 1f) <= range;
        public static bool RandomOutRange(this double range) => !RandomInRange(range);
        
        public static T Random<T>(this T[] input) => (input == null || input.Length == 0) ? default : input[input.RandomIndex()];
        public static T Random<T>(this IList<T> input) => (input == null || input.Count == 0) ? default : input[input.RandomIndex()];

        public static T Random<T>(this ICollection<T> input) => (input == null || input.Count == 0) ? default : input.ElementAt(input.RandomIndex());
        public static T Random<T>(this IEnumerable<T> input) => (input == null || input.Count() == 0) ? default : input.ElementAt(input.RandomIndex());
        public static T Random<T>(this T defaultValue) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                var values = Enum.GetValues(typeof(T));
                return (T)values.GetValue(Random(0, values.Length - 1));
            }

            return defaultValue;
        }

        public static T GetRandomValueSatisfyingCondition<T>(this IEnumerable<T> inputList, Func<T, bool> conditionFunc)
        {
            var filteredList = inputList.Where(conditionFunc).ToList();
            if (filteredList.Count > 0)
            {
                Debug.Log(filteredList.Count);
                return filteredList[Random(0, filteredList.Count)];
            }
            else
            {
                // Trả về giá trị mặc định của kiểu T nếu không tìm thấy giá trị thỏa mãn điều kiện
                return default(T);
            }
        }

        public static int RandomIndex<T>(this T[] array) => Random(0, array.Length - 1);
        public static int RandomIndex<T>(this IList<T> list) => Random(0, list.Count - 1);
        public static int RandomIndex<T>(this ICollection<T> collection) => Random(0, collection.Count - 1);
        public static int RandomIndex<T>(this IEnumerable<T> enumerable) => Random(0, enumerable.Count() - 1);

        #endregion

        #region Shupffle And Swap

        public static void Shuffle<T>(this List<T> array)
        {
            if (array == null || array.Count <= 1) return;

            for (int i = 0; i < array.Count - 1; i++)
            {
                int random = Random(i, array.Count - 1);
                array.Swap(i, random);
            }
        }
        
        public static void Swap<T>(this List<T> array, int a, int b)
        {
            (array[a], array[b]) = (array[b], array[a]);
        }
     
        #endregion
        

        public static bool Compare(this double a, double b) => a >= b;
        public static double Clamp(this double value, double min, double max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }
        public static double Clamp(this double value, Vector2 vector) => value.Clamp(vector.x, vector.y);
        public static double Max(this double value, double target) => value.Compare(target) ? value : target;
        public static double Min(this double value, double target) => !value.Compare(target) ? value : target;

        public static string[] Scores = new string[] { "", "k", "M", "B", "T",
            "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az",
            "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz",
            "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", "cy", "cz",
            "da", "db", "dc", "dd", "de", "df", "dg", "dh", "di", "dj", "dk", "dl", "dm", "dn", "do", "dp", "dq", "dr", "ds", "dt", "du", "dv", "dw", "dx", "dy", "dz",
        };

        #region Show

        public static string Show(this double value) => new JobShow().Execute(value);

        #region Time

        public static string Show(this TimeSpan time)
        {
            if (time.TotalMilliseconds == 0)
                return "0s";
            StringBuilder str = new StringBuilder();
            if (time.Days > 0) str.Append($"{time.Days}d");
            if (time.Hours > 0) str.Append($" {time.Hours}h");
            if (time.Minutes > 0) str.Append($" {time.Minutes}m");
            if (time.Hours == 0)
            {
                if (time.Seconds > 0)
                    str.Append($" {time.Seconds}");
                else if (time.Milliseconds > 0)
                    str.Append("0");
                int ms = time.Milliseconds / 100;
                if (time.Minutes == 0 && ms > 0)
                    str.Append($".{ms}");
                if (time.Seconds > 0 || ms > 0)
                    str.Append("s");
            }

            return str.ToString();
        }

        public static string ShowLess(this TimeSpan time)
        {
            if (time.TotalMilliseconds == 0)
                return "0S";
            StringBuilder str = new StringBuilder();

            if (time.Days > 0) 
            {
                str.Append($"{time.Days}D");
                str.Append($" {time.Hours}H");
                return str.ToString();
            }

            if (time.Hours > 0)
            {
                str.Append($"{time.Hours}H");
                str.Append($" {time.Minutes}M");
                return str.ToString();
            }

            if (time.Minutes > 0)
            {
                str.Append($"{time.Minutes}M");
                str.Append($" {time.Seconds}S");
                return str.ToString();
            }

            if(time.Seconds > 0)
            {
                str.Append($"{time.Seconds}S");
                return str.ToString();
            }

            return str.ToString();
        }
        
        public static string ShowTime(this TimeSpan time)
        {
            string str = "";
            if (time.Days > 0) str += time.Days;
            if (time.Hours > 0)
            {
                if (time.Days > 0) str += ":";
                str += time.Hours.ToString("00");
            }

            if (time.Hours > 0) str += ":";
            str += time.Minutes.ToString("00");
            str += ":" + time.Seconds.ToString("00");
            return str;
        }
        
        public static string Show(this TimeSpan time, string format)
        {
            return time.ToString(format);
        }

        #endregion

        #endregion
    }
}
