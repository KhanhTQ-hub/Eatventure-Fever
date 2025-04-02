using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace kt.job.geometry
{
    public static class AngleExtension
    {
        public static float SqrMagnitudeTo(this float3 from, float3 to, Allocator allocator = Allocator.Persistent) => SqrMagnitudeTo(to - from, allocator);
        public static float SqrMagnitudeTo(this float2 from, float2 to, Allocator allocator = Allocator.Persistent) => SqrMagnitudeTo(to - from, allocator);
        public static float SqrMagnitudeTo(this Vector3 from, Vector3 to, Allocator allocator = Allocator.Persistent) => SqrMagnitudeTo(to - from, allocator);
        public static float SqrMagnitudeTo(this Vector2 from, Vector2 to, Allocator allocator = Allocator.Persistent) => SqrMagnitudeTo(to - from, allocator);
        public static float SqrMagnitudeTo(this Vector3 distance, Allocator allocator = Allocator.Persistent) => new JobSqrMagnitude(allocator).Execute(distance.x, distance.y);
        public static float SqrMagnitudeTo(this Vector2 distance, Allocator allocator = Allocator.Persistent) => new JobSqrMagnitude(allocator).Execute(distance.x, distance.y);

        public static float AngleTo(this Vector3 vector, Allocator allocator = Allocator.Persistent) => new JobAngle(allocator).Execute(vector.x, vector.y);
        public static float AngleTo(this Vector2 vector, Allocator allocator = Allocator.Persistent) => new JobAngle(allocator).Execute(vector.x, vector.y);
        public static float AngleTo(this Vector3 from, Vector3 to, Allocator allocator = Allocator.Persistent) => AngleTo(to - from, allocator);
        public static float AngleTo(this Vector2 from, Vector2 to, Allocator allocator = Allocator.Persistent) => AngleTo(to - from, allocator);

        public static Vector3 MaxNormalized(this Vector3 vector, Allocator allocator = Allocator.Persistent) => new JobMaxNormalized(allocator).Execute(vector.x, vector.y);
        public static Vector2 MaxNormalized(this Vector2 vector, Allocator allocator = Allocator.Persistent) => new JobMaxNormalized(allocator).Execute(vector.x, vector.y);
    }
}