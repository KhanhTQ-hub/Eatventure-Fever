using kt.job.core;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace kt.job.math
{
    public class JobRateArray
    {
        private const Allocator allocator = Allocator.Persistent;
        private const bool compile = true;

        public JobRateArray()
        {

        }

        public int[] Execute(params int[] array)
        {
            if (array == null || array.Length == 0) return array;

            NativeArray<int> input;
            NativeArray<int> output;
            Init(array, out input, out output);
            Execute(input, output);
            var rs = output.ToArray();
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public List<int> Execute(List<int> array)
        {
            if (array == null || array.Count == 0) return array;

            NativeArray<int> input;
            NativeArray<int> output;
            Init(array, out input, out output);
            Execute(input, output);
            var rs = output.ToList();
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public float[] Execute(params float[] array)
        {
            if (array == null || array.Length == 0) return array;

            NativeArray<float> input;
            NativeArray<float> output;
            Init(array, out input, out output);
            Execute(input, output);
            var rs = output.ToArray();
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public List<float> Execute(List<float> array)
        {
            if (array == null || array.Count == 0) return array;

            NativeArray<float> input;
            NativeArray<float> output;
            Init(array, out input, out output);
            Execute(input, output);
            var rs = output.ToList();
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public void Execute(NativeArray<int> input, NativeArray<int> output)
        {
            var job = new JobRateArrayInt();
            job.Init(input, output);
            job.Schedule().Complete();
        }

        public void Execute(NativeArray<float> input, NativeArray<float> output)
        {
            var job = new JobRateArrayFloat();
            job.Init(input, output);
            job.Schedule().Complete();
        }

        private void Init<T>(IReadOnlyCollection<T> array, out NativeArray<T> input, out NativeArray<T> output) where T : struct
        {
            output = new NativeArray<T>(array.Count, allocator);
            input = array.ToNativeArray(allocator);
        }

        private void Init<T>(T[] array, out NativeArray<T> input, out NativeArray<T> output) where T : struct
        {
            output = new NativeArray<T>(array.Length, allocator);
            input = array.ToNativeArray(allocator);
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobRateArrayFloat : IJob
        {
            [ReadOnly]
            public NativeArray<float> Input;

            [WriteOnly]
            public NativeArray<float> Output;

            public void Init(NativeArray<float> Input, NativeArray<float> Output)
            {
                this.Input = Input;
                this.Output = Output;
            }

            public void Execute()
            {
                float sum = 0;
                for (int i = 0; i < Input.Length; i++)
                {
                    sum += Input[i];
                    Output[i] = sum;
                }
            }
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobRateArrayInt : IJob
        {
            [ReadOnly]
            public NativeArray<int> Input;

            [WriteOnly]
            public NativeArray<int> Output;

            public void Init(NativeArray<int> Input, NativeArray<int> Output)
            {
                this.Input = Input;
                this.Output = Output;
            }

            public void Execute()
            {
                var sum = 0;
                for (var i = 0; i < Input.Length; i++)
                {
                    sum += Input[i];
                    Output[i] = sum;
                }
            }
        }
    }
}
