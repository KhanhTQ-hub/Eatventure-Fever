using System;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace kt.job.math
{
    public struct JobShow
    {
        private const Allocator allocator = Allocator.Persistent;
        private const bool compile = true;

        public string Execute(double value)
        {
            var frice =  value > Constant.Max ? Constant.Max : value;
            if (frice < 0)
                return "0";
            
            if (frice < 1000)
                return Mathf.CeilToInt((float)frice) + "";

            string rs;

            var result = new NativeArray<double>(1, allocator);
            var index = new NativeArray<int>(1, allocator);

            var job = new JobShowDouble(frice, MathExtension.Scores.Length, result, index);
            job.Schedule().Complete();

            var str = new StringBuilder("1");
            for (var j = 0; j < 3 - MathExtension.Scores[index[0]].Length; j++)
                str.Append("0");

            if (result[0] > str.ToString().ParseInt())
                rs = $"{Math.Floor(result[0])}{MathExtension.Scores[index[0]]}";
            else
                rs = $"{result[0]:F1}{MathExtension.Scores[index[0]]}";

            result.Dispose();
            index.Dispose();

            return rs;
        }


        [BurstCompile(CompileSynchronously = compile)]
        private struct JobShowDouble : IJob
        {
            [ReadOnly]
            public double Value;

            [ReadOnly]
            public double Length;

            [WriteOnly]
            public NativeArray<double> Result;

            [WriteOnly]
            public NativeArray<int> Index;

            public JobShowDouble(double Value, int Length, NativeArray<double> Result, NativeArray<int> Index)
            {
                this.Value = Value;
                this.Length = Length;
                this.Result = Result;
                this.Index = Index;
            }

            public void Execute()
            {
                var value = Value;
                int i;
                for (i = 0; i < Length; i++)
                    if (value < 1000)
                        break;
                    else value = Math.Floor(value / 100d) / 10d;
                Result[0] = value;
                Index[0] = i;
            }
        }
    }
}
