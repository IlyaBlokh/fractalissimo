using System;

namespace Code.Data
{
  public static class FractalExtensions
  {
    public static TransformData[] Data(this Fractal fractal)
    {
      switch (fractal)
      {
        case Fractal.BarnsleyFern:
          return FractalParams.BarnsleyFern;
        case Fractal.SierpinskiTriangle:
          return FractalParams.SierpinskiTriangle;
        case Fractal.SierpinskiTriangle2:
          return FractalParams.SierpinskiTriangle2;
        case Fractal.TreeLike:
          return FractalParams.TreeLike;
        case Fractal.DragonCurve:
          return FractalParams.DragonCurve;
        default:
          throw new ArgumentOutOfRangeException(nameof(fractal), fractal, null);
      }
    }
  }
}