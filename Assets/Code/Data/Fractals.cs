using Unity.Mathematics;
using UnityEngine;

namespace Code.Data
{
  public static class Fractals
  {
    static Fractals()
    {
      BarnsleyFern = new TransformData[] {
        new()
        {
          a = new float2(0f, 0f),
          b = new float2(0f, 0.16f),
          c = new float2(0f, 0f),
          d = new float2(0f, 0f),
          probability = 0.01f,
          color = new Color(0.2f, 0.8f, 0.2f) // green stem
        },
        new()
        {
          a = new float2(0.85f, 0.04f),
          b = new float2(-0.04f, 0.85f),
          c = new float2(0f, 1.6f),
          d = new float2(0f, 0f),
          probability = 0.85f,
          color = new Color(0.2f, 0.9f, 0.3f) // main leaves
        },
        new()
        {
          a = new float2(0.2f, -0.26f),
          b = new float2(0.23f, 0.22f),
          c = new float2(0f, 1.6f),
          d = new float2(0f, 0f),
          probability = 0.07f,
          color = new Color(0.3f, 0.7f, 0.3f) // side leaf 1
        },
        new()
        {
          a = new float2(-0.15f, 0.28f),
          b = new float2(0.26f, 0.24f),
          c = new float2(0f, 0.44f),
          d = new float2(0f, 0f),
          probability = 0.07f,
          color = new Color(0.3f, 0.6f, 0.3f) // side leaf 2
        }
      };
    }

    public static TransformData[] BarnsleyFern;
  }
}