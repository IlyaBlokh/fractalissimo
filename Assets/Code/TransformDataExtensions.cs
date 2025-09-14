namespace Code
{
  public static class TransformDataExtensions
  {
    public static int Hash(this TransformData[] transforms)
    {
      const int w = 31;
      int hash = 17;
      unchecked
      {
        foreach (TransformData t in transforms)
        {
          // probability
          hash = hash * w + t.probability.GetHashCode();

          // matrix components
          hash = hash * w + t.a.x.GetHashCode();
          hash = hash * w + t.a.y.GetHashCode();
          hash = hash * w + t.b.x.GetHashCode();
          hash = hash * w + t.b.y.GetHashCode();

          hash = hash * w + t.c.x.GetHashCode();
          hash = hash * w + t.c.y.GetHashCode();
          hash = hash * w + t.d.x.GetHashCode();
          hash = hash * w + t.d.y.GetHashCode();

          // color
          hash = hash * w + t.color.r.GetHashCode();
          hash = hash * w + t.color.g.GetHashCode();
          hash = hash * w + t.color.b.GetHashCode();
          hash = hash * w + t.color.a.GetHashCode();
        }
        return hash;
      }
    }
  }
}