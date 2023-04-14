using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotNetDesignPatternDemos.Behavioral.IteratorAdapter;

// non-generic factory method adapter
public static class Array2D
{
  public static Array2D<T> Create<T>(T[,] values)
  {
    return new Array2D<T>(values);
  }
}
  
/// <summary>
/// Two-dimensional array. Stores data as 1D,
/// exposes as either 1D or 2D.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Array2D<T> : IEnumerable<T>
{
  private T[] data;
  private (int width, int height) size;

  public ref T[] Data => ref data;

  public Array2D(T[,] values)
  {
    Size = (values.GetLength(0), values.GetLength(1));
    for (int y = 0; y < Height; ++y)
    for (int x = 0; x < Width; ++x)
      this[x, y] = values[y, x];
  }
    
  // type inference factory method
  public static Array2D<T> From(T[,] values)
  {
    return new Array2D<T>(values);
  }
    
  public (int width, int height) Size
  {
    get => size;
    set
    {
      if (size == value) return;
      size = value;
      data = new T[size.width * size.height];
    }
  }

  public int Width => size.width;
    
  public int Height => size.height;

  public int Length => size.width * size.height;

  public Array2D(int width, int height)
  {
    Size = (width, height);
  }

  public IEnumerator<T> GetEnumerator()
  {
    return ((IEnumerable<T>) data).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int Get1DIndex(int x, int y)
  {
#if DEBUG
    if (x < 0 || x >= Width)
      throw new IndexOutOfRangeException("x");
    if (y < 0 || y >= Height)
      throw new IndexOutOfRangeException("y");
#endif

    return y * Width + x;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public (int x, int y) Get2DIndex(int index)
  {
#if DEBUG
    if (index < 0 || index >= data.Length)
      throw new IndexOutOfRangeException("index");
#endif

    return (index % Width, index / Width);
  }

  public T this[int index]
  {
    get => data[index];
    set => data[index] = value;
  }

  public T this[int x, int y]
  {
    get => data[Get1DIndex(x, y)];
    set => data[Get1DIndex(x, y)] = value;
  }

  public T[] this[Range range] => data[range];

  public override string ToString()
  {
    var sb = new StringBuilder("{");
    for (int y = 0; y < Height; ++y)
    {
      sb.Append('{');
      for (int x = 0; x < Width; ++x)
      {
        sb.Append(this[x, y]);
        if (x + 1 != Width) sb.Append(',');
      }
      sb.Append('}');
      if (y + 1 != Height) sb.Append(',');
    }

    return sb.ToString();
  }
}

public class OneDAdapter<T> : IEnumerable<T>
{
  private readonly T[,] arr;
  private int w, h;

  public OneDAdapter(T[,] arr)
  {
    this.arr = arr;
    w = arr.GetLength(0);
    h = arr.GetLength(1);
  }

  public IEnumerator<T> GetEnumerator()
  {
    for (int y = 0; y < h; ++y)
    for (int x = 0; x < w; ++x)
      yield return arr[x, y];
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}

public static class ReverseIterable
{
  public static ReverseIterable<T> From<T>(T[] arr)
  {
    return new ReverseIterable<T>(arr);
  }
}

public class ReverseIterable<T> : IEnumerable<T>
{
  private readonly T[] arr;

  public ReverseIterable(T[] arr) => this.arr = arr;

  public IEnumerator<T> GetEnumerator()
  {
    for (int i = arr.Length - 1; i >= 0; --i)
      yield return arr[i];
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
  
public class Demo
{
  public static void Main(string[] args)
  {
    var data = new [,] { { 1, 2 }, { 3, 4 } };
    var sum = new OneDAdapter<int>(data).Sum();
    foreach (var i in data)
      Console.WriteLine(i);
    // basic[0] = 0;
      
    //var x = new Array2D<int>(basic);
    var x = Array2D.Create(data);
    Console.WriteLine($"x = {x}");
    Console.WriteLine($"Sum = {x.Sum()}");

    var lastTwo = x[^2..];
    Console.WriteLine(string.Join(',', lastTwo));
      
    var values = new int[]{1, 2, 3};
    foreach (var z in ReverseIterable.From(values))
      Console.WriteLine(z);
  }
}