using Unity.Mathematics;
using UnityEngine;

namespace Code
{
  [System.Serializable]
  public struct TransformData
  {
    public float2 a, b, c, d;
    public Color color;
    [Range(0f, 1f)] public float probability;
  }
}