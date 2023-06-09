﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Singleton.Singleton;

public class MyDatabase
{
  private MyDatabase()
  {
    WriteLine("Initializing database");
  }
  private static readonly Lazy<MyDatabase> instance = new();

  public static MyDatabase Instance => instance.Value;
}
  
public interface IDatabase
{
  int GetPopulation(string name);
}

public class SingletonDatabase : IDatabase
{
  private readonly Dictionary<string, int> capitals;

  private SingletonDatabase()
  {
    WriteLine("Initializing database");

    capitals = File.ReadAllLines(
        Path.Combine(
          new FileInfo(typeof(IDatabase).Assembly.Location)
            .DirectoryName, 
          "capitals.txt")
      )
      .Batch(2)
      .ToDictionary(
        list => list.ElementAt(0).Trim(),
        list => int.Parse(list.ElementAt(1)));
  }

  public int GetPopulation(string name)
  {
    return capitals[name];
  }

  // laziness + thread safety
  private static Lazy<SingletonDatabase> instance = new(() =>
  {
    return new SingletonDatabase();
  });

  public static IDatabase Instance => instance.Value;
}

public class SingletonRecordFinder
{
  public int TotalPopulation(IEnumerable<string> names)
  {
    int result = 0;
    foreach (var name in names)
      result += SingletonDatabase.Instance.GetPopulation(name);
    return result;
  }
}

public class ConfigurableRecordFinder
{
  private IDatabase database;
  
  public ConfigurableRecordFinder(IDatabase database)
  {
    this.database = database;
  }
  
  public int GetTotalPopulation(IEnumerable<string> names)
  {
    int result = 0;
    foreach (var name in names)
      result += database.GetPopulation(name);
    return result;
  }
}

public class DummyDatabase : IDatabase
{
  public int GetPopulation(string name)
  {
    return new Dictionary<string, int>
    {
      ["alpha"] = 1,
      ["beta"] = 2,
      ["gamma"] = 3
    }[name];
  }
}

/// <summary>
/// IMPORTANT: be sure to turn off shadow copying for unit tests in R#!
/// </summary>
[TestFixture]
public class SingletonTests
{
  [Test]
  public void IsSingletonTest()
  {
    var db = SingletonDatabase.Instance;
    var db2 = SingletonDatabase.Instance;
    Assert.That(db, Is.SameAs(db2));
  }

  [Test]
  public void SingletonTotalPopulationTest()
  {
    // testing on a live database
    var rf = new SingletonRecordFinder();
    var names = new[] {"Seoul", "Mexico City"};
    int tp = rf.TotalPopulation(names);
    Assert.That(tp, Is.EqualTo(17500000 + 17400000));
  }

  [Test]
  public void DependentTotalPopulationTest()
  {
    var db = new DummyDatabase();
    var rf = new ConfigurableRecordFinder(db);
    Assert.That(
      rf.GetTotalPopulation(new[]{"alpha", "gamma"}),
      Is.EqualTo(4));
  }
}

public class Demo
{
  static void Main_()
  {
    var db = SingletonDatabase.Instance;

    // works just fine while you're working with a real database.
    var city = "Tokyo";
    WriteLine($"{city} has population {db.GetPopulation(city)}");
      
      
  }
}