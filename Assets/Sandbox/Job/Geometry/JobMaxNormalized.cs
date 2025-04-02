using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace kt.job.geometry
{
    public struct JobMaxNormalized
    {
        public Allocator Allocator;
        private const bool compile = true;

        public JobMaxNormalized(Allocator allocator = Allocator.Persistent)
        {
            this.Allocator = allocator;
        }

        public Vector2 Execute(float disX, float disY)
        {
            var result = new NativeArray<float2>(1, Allocator);

            var job = new JobMaxNormalizedModel();
            job.Init(disX, disY, result);
            job.Schedule().Complete();

            var rs = result[0];
            result.Dispose();

            return rs;
        }

        [BurstCompile(CompileSynchronously = compile)]
        private struct JobMaxNormalizedModel : IJob
        {
            [ReadOnly]
            public float DisX;

            [ReadOnly]
            public float DisY;

            [WriteOnly]
            public NativeArray<float2> Result;

            public void Init(float DisX, float DisY, NativeArray<float2> Result)
            {
                this.DisX = DisX;
                this.DisY = DisY;
                this.Result = Result;
            }

            public void Execute()
            {
                var x = DisX > 0 ? DisX : -DisX;
                var y = DisY > 0 ? DisY : -DisY;

                float rs;
                if (x > y)
                {
                    if (x <= 0.1) x = 0.1f;

                    rs = 1 / x;
                    x = 1;
                    y *= rs;
                }
                else
                {
                    if (y <= 0.1) y = 0.1f;

                    rs = 1 / y;
                    y = 1;
                    x *= rs;
                }

                Result[0] = new float2((DisX > 0 ? 1 : -1) * x, (DisY > 0 ? 1 : -1) * y);
            }
        }
    }
}
