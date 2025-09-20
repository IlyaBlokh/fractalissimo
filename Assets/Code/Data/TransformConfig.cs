using UnityEngine;

namespace Code.Data
{
  [CreateAssetMenu(menuName = "Fractalissimo/Transform Config", fileName = "Transform Config", order = 0)]
  public class TransformConfig : ScriptableObject
  {
    public TransformData[] Transforms;
  }
}