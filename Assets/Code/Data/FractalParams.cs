using Unity.Mathematics;
using UnityEngine;

namespace Code.Data
{
  public static class FractalParams
  {
    public static readonly TransformData[] BarnsleyFern;
    public static readonly TransformData[] SierpinskiTriangle;
    public static readonly TransformData[] SierpinskiTriangle2;
    public static readonly TransformData[] TreeLike;
    public static readonly TransformData[] DragonCurve;

    static FractalParams()
    {
      BarnsleyFern = new TransformData[]
      {
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
      SierpinskiTriangle = new[]
      {
        new TransformData
        {
          a = new float2(0.5f, 0f),
          b = new float2(0f, 0.5f),
          c = new float2(-0.5f, 0f),
          d = new float2(0f, 1f),
          probability = 0.33f,
          color = Color.red
        },
        new TransformData
        {
          a = new float2(0.5f, 0f),
          b = new float2(0f, 0.5f),
          c = new float2(0.5f, 0f),
          d = new float2(0f, 0.5f),
          probability = 0.33f,
          color = Color.green
        },
        new TransformData
        {
          a = new float2(0.5f, 0.5f),
          b = new float2(-0.5f, 0.5f),
          c = new float2(0f, -0.5f),
          d = new float2(0f, 0.5f),
          probability = 0.34f,
          color = Color.blue
        }
      };
      SierpinskiTriangle2 = new[]
      {
        new TransformData
        {
          a = new float2(0.5f, 0.0f),
          b = new float2(0.0f, 0.5f),
          c = new float2(-0.5f, 0.0f),
          d = new float2(0.0f, 0.5f),
          color = new Color(1f, 0.3f, 0.2f),
          probability = 0.33f
        },
        new TransformData
        {
          a = new float2(0.5f, 0.0f),
          b = new float2(0.0f, 0.5f),
          c = new float2(0.5f, 0.0f),
          d = new float2(0.0f, 0.5f),
          color = new Color(0.2f, 0.8f, 1f), // cyan-blue
          probability = 0.33f
        },
        new TransformData
        {
          a = new float2(0.5f, 0.5f),
          b = new float2(-0.5f, 0.5f),
          c = new float2(0.0f, -0.5f),
          d = new float2(0.0f, 0.5f),
          color = new Color(1f, 1f, 0.2f), // yellow
          probability = 0.34f
        }
      };

      TreeLike = new[]
      {
        new TransformData
        {
          a = new float2(0.5f, 0f),
          b = new float2(0f, 0.5f),
          c = new float2(0f, 0f),
          d = new float2(0f, 0f),
          probability = 0.5f,
          color = new Color(0.6f, 0.4f, 0.2f) // trunk
        },
        new TransformData
        {
          a = new float2(0.5f, 0f),
          b = new float2(0f, 0.5f),
          c = new float2(0.5f, 0.5f),
          d = new float2(0f, 0f),
          probability = 0.25f,
          color = new Color(0.3f, 0.8f, 0.3f) // branch left
        },
        new TransformData
        {
          a = new float2(0.5f, 0f),
          b = new float2(0f, 0.5f),
          c = new float2(-0.5f, 0.5f),
          d = new float2(0f, 0f),
          probability = 0.25f,
          color = new Color(0.2f, 0.6f, 0.2f) // branch right
        }
      };
      
      DragonCurve = new[]
      {
        new TransformData {
          a = new float2(0.824074f, 0.281482f),
          b = new float2(-0.212346f, 0.864198f),
          c = new float2(-1.88229f, -0.110607f),
          d = new float2(0f, 0f),
          probability = 0.787473f,
          color = Color.magenta
        },
        new TransformData {
          a = new float2(0.088272f, 0.520988f),
          b = new float2(-0.463889f, -0.377778f),
          c = new float2(0.78536f, 8.095795f),
          d = new float2(0f, 0f),
          probability = 0.212527f,
          color = Color.cyan
        }
      };
    }
  }
}