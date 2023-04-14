using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDesignPatternDemos.Behavioral.Iterator.ArrayBackedProperties;

public class Creature : IEnumerable<int>
{
  private int [] stats = new int[3];

  public IEnumerable<int> Stats => stats;

  private const int strength = 0;

  public ref int Strength => ref stats[strength];
    
  // slightly more complicated way
  // public int Strength
  // {
  //   get => stats[strength];
  //   set => stats[strength] = value;
  // }

  //public int Agility { get; set; }
  //public int Intelligence { get; set; }

  public ref int Agility => ref stats[1];
  public ref int Intelligence => ref stats[2];

  public double AverageStat => stats.Average();

  //public double AverageStat => SumOfStats / 3.0;

  //public double SumOfStats => Strength + Agility + Intelligence;
  public double SumOfStats => stats.Sum();

  //public double MaxStat => Math.Max(
  //  Math.Max(Strength, Agility), Intelligence);

  public double MaxStat => stats.Max();

  public IEnumerator<int> GetEnumerator()
  {
    return stats.AsEnumerable().GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  // public int this[int index]
  // {
  //   get => stats[index];
  //   set => stats[index] = value;
  // }
  public ref int this[int index] => ref stats[index];
}

public class Demo
{
  static void Main(string[] args)
  {
    var creature = new Creature {Strength = 10, Intelligence = 11, Agility = 12};
    Console.WriteLine($"Creature has average stat = {creature.AverageStat}, " +
                      $"max stat = {creature.MaxStat}, " +
                      $"sum of stats = {creature.SumOfStats}.");
  }
}