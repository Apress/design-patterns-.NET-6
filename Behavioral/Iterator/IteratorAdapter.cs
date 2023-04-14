using System.Collections;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Behavioral.Iterator.Adapter;

public class OneDAdapter<T> : IEnumerable<T>
  where T : struct
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