using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Random = Unity.Mathematics.Random;

namespace kt.job.math
{
    public struct JobRandom
    {
        private const Allocator allocator = Allocator.Persistent;
        private const bool compile = true;
        private uint seed;

        public float Execute(float from, float to)
        {
            if (from >= to)
                return from;

            NativeArray<float> output;
            In(out output);
            Schedule(from, to, output);
            return Out(output);
        }

        public double Execute(double from, double to)
        {
            if (from >= to)
                return from;

            NativeArray<double> output;
            In(out output);
            Schedule(from, to, output);
            return Out(output);
        }

        public int Execute(int from, int to)
        {
            if (from >= to)
                return from;

            NativeArray<int> output;
            In(out output);
            Schedule(from, to, output);
            return Out(output);
        }

        private void Schedule(float from, float to, NativeArray<float> result)
        {
            var job = new JobRandomFloat();
            job.Init(from, to, seed, result);
            job.Schedule().Complete();
        }

        private void Schedule(double from, double to, NativeArray<double> result)
        {
            var job = new JobRandomDouble();
            job.Init(from, to, seed, result);
            job.Schedule().Complete();
        }

        private void Schedule(int from, int to, NativeArray<int> result)
        {
            var job = new JobRandomInt();
            job.Init(from, to, seed, result);
            job.Schedule().Complete();
        }

        private void In<T>(out NativeArray<T> output) where T : struct
        {
            output = new NativeArray<T>(1, allocator);
            seed = (uint)(UnityEngine.Random.value * (uint.MaxValue - 1) + 1);
        }

        private T Out<T>(NativeArray<T> output) where T : struct
        {
            var result = output[0];
            output.Dispose();

            return result;
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobRandomInt : IRandomJob<int>
        {
            [ReadOnly]
            public int From;

            [ReadOnly]
            public int To;

            [WriteOnly]
            public NativeArray<int> Result;

            private Random random;

            public void Init(int From, int To, uint seed, NativeArray<int> Result)
            {
                this.From = From;
                this.To = To;
                this.Result = Result;
                random = new Random(seed);
            }

            public void Execute()
            {
                var result = Next();
                Result[0] = result;
            }

            public int Next() => random.NextInt(From, To + 1);
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobRandomFloat : IRandomJob<float>
        {
            [ReadOnly]
            public float From;

            [ReadOnly]
            public float To;

            [WriteOnly]
            public NativeArray<float> Result;

            private Random random;

            public void Init(float From, float To, uint seed, NativeArray<float> Result)
            {
                this.From = From;
                this.To = To;
                this.Result = Result;
                random = new Random(seed);
            }

            public void Execute()
            {
                var result = Next();
                Result[0] = result;
            }

            public float Next() => random.NextFloat(From, To);
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobRandomDouble : IRandomJob<double>
        {
            [ReadOnly]
            public double From;

            [ReadOnly]
            public double To;

            [WriteOnly]
            public NativeArray<double> Result;

            private Random random;

            public void Init(double From, double To, uint seed, NativeArray<double> Result)
            {
                this.From = From;
                this.To = To;
                this.Result = Result;
                random = new Random(seed);
            }

            public void Execute()
            {
                var result = Next();
                Result[0] = result;
            }

            public double Next() => random.NextDouble(From, To);
        }

        private interface IRandomJob<T> : IJob where T : struct
        {
            void Init(T From, T To, uint seed, NativeArray<T> Result);
            T Next();
        }
    }
}
