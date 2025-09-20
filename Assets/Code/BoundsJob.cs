using Code.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Code
{
  [BurstCompile]
  struct BoundsJob : IJob
  {
    public int iterations;
    [ReadOnly] public NativeArray<TransformData> transforms;
    public NativeArray<float4> bounds; // (minX, maxX, minY, maxY)

    public void Execute()
    {
      Random rng = new Random(5678);

      float totalProb = 0f;
      for (int i = 0; i < transforms.Length; i++)
        totalProb += transforms[i].probability;

      float2 p = new float2(0, 0);

      float minX = float.MaxValue, maxX = float.MinValue;
      float minY = float.MaxValue, maxY = float.MinValue;

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

        minX = math.min(minX, p.x);
        maxX = math.max(maxX, p.x);
        minY = math.min(minY, p.y);
        maxY = math.max(maxY, p.y);
      }

      bounds[0] = new float4(minX, maxX, minY, maxY);
    }
  }
}