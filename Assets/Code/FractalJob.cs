using Code.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Code
{
    [BurstCompile]
    struct FractalJob : IJob
    {
        public int width;
        public int height;
        public int iterations;

        [ReadOnly] public NativeArray<TransformData> transforms;
        public NativeArray<float3> colorAccum;
        public NativeArray<int> hitCount;
        [ReadOnly] public NativeArray<float4> bounds;

        public void Execute()
        {
            var rng = new Random(1234);

            float totalProb = 0f;
            for (int i = 0; i < transforms.Length; i++)
                totalProb += transforms[i].probability;

            float2 p = new float2(0, 0);
            var b = bounds[0];
            float minX = b.x, maxX = b.y, minY = b.z, maxY = b.w;

            for (int i = 0; i < iterations; i++)
            {
                // Weighted random transform
                float r = rng.NextFloat(0f, totalProb);
                float accum = 0f;
                int chosen = 0;
                for (int j = 0; j < transforms.Length; j++)
                {
                    accum += transforms[j].probability;
                    if (r <= accum)
                    {
                        chosen = j;
                        break;
                    }
                }

                TransformData t = transforms[chosen];
                p = new float2(
                    t.a.x * p.x + t.b.x * p.y + t.c.x,
                    t.a.y * p.x + t.b.y * p.y + t.d.y
                );

                // Normalize coordinates to texture
                float nx = (p.x - minX) / (maxX - minX);
                float ny = (p.y - minY) / (maxY - minY);
                int x = (int)(nx * (width - 1));
                int y = (int)(ny * (height - 1));

                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    int idx = y * width + x;
                    hitCount[idx] += 1;
                    colorAccum[idx] += new float3(t.color.r, t.color.g, t.color.b);
                }
            }
        }
    }
}