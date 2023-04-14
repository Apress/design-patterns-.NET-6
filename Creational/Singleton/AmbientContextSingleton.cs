using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNetDesignPatternDemos.Creational.Singleton;

public static class ExtensionMethods
{
  public static T DeepCopy<T>(this T self)
  {
    using (var stream = new MemoryStream())
    {
      BinaryFormatter formatter = new();
      formatter.Serialize(stream, self);
      stream.Seek(0, SeekOrigin.Begin);
      object copy = formatter.Deserialize(stream);
      return (T) copy;
    }
  }
}

[Serializable]
public sealed class BuildingContext : IDisposable
{
  // make this inaccessible
  private BuildingContext() {}
    
  public int Height { get; private set; }

  private static readonly Stack<BuildingContext> stack = new();
    
  static BuildingContext() { stack.Push(new BuildingContext()); }

  public static IDisposable WithHeight(int height)
  {
    var copy = Current.DeepCopy();
    copy.Height = height;
    stack.Push(copy);
    return copy;
  }

  public static BuildingContext Current => stack.Peek();
    
  public void Dispose()
  {
    if (stack.Count > 1) stack.Pop();
  }
}

public class Point
{
  public int X { get; }
  public int Y { get; }

  public Point(int x, int y)
  {
    X = x;
    Y = y;
  }

}

public class Wall
{
  public Point Start;
  public Point End;
  public int Height;

  public Wall(Point start, Point end, int? height = null)
  {
    Start = start;
    End = end;
    Height = height ?? BuildingContext.Current.Height;
  }
}

public class Building
{
  public List<Wall> Walls = new();
}

public class Demo
{
  public static void Main()
  {
    var building = new Building();

    using (BuildingContext.WithHeight(2000))
    {
      building.Walls.Add(new Wall(
        new Point(0, 1000), new Point(1000, 1000)));
      using (BuildingContext.WithHeight(1000))
      {
        building.Walls.Add(new Wall(
          new Point(1000, 2000), new Point(2000, 3000)));
      }
      building.Walls.Add(new Wall(
        new Point(0, 1000), new Point(1000, 1000)));
    }

    foreach (var wall in building.Walls)
    {
      Console.WriteLine(wall.Height);
    }
  }
}