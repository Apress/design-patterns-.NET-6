using System;
using System.Diagnostics;

namespace DotNetDesignPatternDemos.Structural.ValueObject.PercentValue;

[DebuggerDisplay("{value*100.0f}%")]
public readonly record struct Percentage(decimal value)
{
  private readonly decimal value = value;

  public static decimal operator *(decimal f, Percentage p)
  {
    return f * p.value;
  }

  public static Percentage operator +(Percentage a, Percentage b)
  {
    return new Percentage(a.value + b.value);
  }

  public static implicit operator Percentage(int value)
  {
    return value.Percent();
  }

  public override string ToString()
  {
    return $"{value*100}%";
  }
}
  
public static class PercentageExtensions
{
  public static Percentage Percent(this int value)
  {
    return new Percentage(value/100.0m);
  }
    
  public static Percentage Percent(this decimal value)
  {
    return new Percentage(value/100.0m);
  }
}


class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine(10m * 5.Percent());
    Console.WriteLine(2.Percent() + 3m.Percent());
  }
}