using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PointsAndLines;

public readonly record struct Point(double X, double Y)
{
  public override string ToString() => $"({X},{Y})";
}

class Line
{
  public Point Start, End;
}

class GPoint
{
  public Point Point;
  public string Color = "black";
  // useful constructors here
  public double X => Point.X;
  public double Y => Point.Y;

  public static implicit operator Point(GPoint gp)
  {
    return gp.Point; 
  }
}

class GLine
{
  public readonly GPoint Start = new(), End = new();
  public double Thickness = 1.0;

  public static implicit operator Line(GLine gl)
  {
    return new Line() { Start = gl.Start, End = gl.End };
  }
}
  
class Program
{
  static void Main(string[] args)
  {
    GPoint p = new();
    // p.X++; // no, immutable 
  }
}