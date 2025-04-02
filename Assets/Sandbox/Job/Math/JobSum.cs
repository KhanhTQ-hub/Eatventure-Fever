using kt.job.core;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace kt.job.math
{
     public class JobSum
    {
        private const Allocator allocator = Allocator.Persistent;
        private const bool compile = true;

        public JobSum()
        {

        }

        public float Execute(params float[] array)
        {
            NativeArray<float> input;
            NativeArray<float> output;
            Init(array, out input, out output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public float Execute(IEnumerable<float> array)
        {
            NativeArray<float> input;
            NativeArray<float> output;
            Init(array, out input, out output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public double Execute(params double[] array)
        {
            NativeArray<double> input;
            NativeArray<double> output;
            Init(array, out input, out output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public double Execute(IEnumerable<double> array)
        {
            NativeArray<double> input;
            NativeArray<double> output;
            Init(array, out input, out output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public int Execute(params int[] array)
        {
            NativeArray<int> output;
            Init(array, out var input, out output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public int Execute(IEnumerable<int> array)
        {
            Init(array, out var input, out var output);
            var rs = Execute(input, output);
            input.Dispose();
            output.Dispose();
            return rs;
        }

        public double Execute(NativeArray<double> input, NativeArray<double> output)
        {
            var job = new JobSumDouble();
            job.Init(input, output);
            job.Schedule().Complete();
            return output[0];
        }

        public float Execute(NativeArray<float> input, NativeArray<float> output)
        {
            var job = new JobSumFloat();
            job.Init(input, output);
            job.Schedule().Complete();
            return output[0];
        }

        public int Execute(NativeArray<int> input, NativeArray<int> output)
        {
            var job = new JobSumInt();
            job.Init(input, output);
            job.Schedule().Complete();
            return output[0];
        }

        private void Init<T>(IEnumerable<T> array, out NativeArray<T> input, out NativeArray<T> output) where T : struct
        {
            output = new NativeArray<T>(1, allocator);
            input = array.ToNativeArray(allocator);
        }

        private void Init<T>(T[] array, out NativeArray<T> input, out NativeArray<T> output) where T : struct
        {
            output = new NativeArray<T>(1, allocator);
            input = array.ToNativeArray(allocator);
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobSumDouble : ISumJob<double>
        {
            [ReadOnly]
            public NativeArray<double> Input;

            [WriteOnly]
            public NativeArray<double> Output;

            public void Init(NativeArray<double> Input, NativeArray<double> Output)
            {
                this.Input = Input;
                this.Output = Output;
            }

            public double Add(double result, double item) => result + item;

            public void Execute()
            {
                double result = 0;
                foreach (var item in Input)
                {
                    result = Add(result, item);
                }
                Output[0] = result;
            }
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobSumFloat : ISumJob<float>
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

            public float Add(float result, float item) => result + item;

            public void Execute()
            {
                float result = 0;
                foreach (var item in Input)
                {
                    result = Add(result, item);
                }
                Output[0] = result;
            }
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobSumInt : ISumJob<int>
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

            public int Add(int result, int item) => result + item;

            public void Execute()
            {
                var result = 0;
                foreach (var item in Input)
                {
                    result = Add(result, item);
                }
                Output[0] = result;
            }
        }

        private interface ISumJob<T> : IJob where T : struct
        {
            void Init(NativeArray<T> Input, NativeArray<T> Output);
            T Add(T result, T item);
        }
    }
}
