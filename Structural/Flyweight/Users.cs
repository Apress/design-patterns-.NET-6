﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace DotNetDesignPatternDemos.Structural.Flyweight.Users;

using System;
using static System.Console;

public class User
{
  public string FullName { get; }

  public User(string fullName)
  {
    FullName = fullName;
  }
}

public class User2
{
  static List<string> strings = new();
  private int[] names;

  public User2(string fullName)
  {
    int getOrAdd(string s)
    {
      int idx = strings.IndexOf(s);
      if (idx != -1) return idx;
      else
      {
        strings.Add(s);
        return strings.Count - 1;
      }
    }

    names = fullName.Split(' ').Select(getOrAdd).ToArray();
  }

  public string FullName => string.Join(" ", names.Select(i => strings[i]));
}

[TestFixture]
public class Demo
{
  static void Main(string[] args)
  {
      
  }

  public void ForceGC()
  {
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();
  }

  public static string RandomString()
  {
    Random rand = new();
    return new string(
      Enumerable.Range(0, 10).Select(i => (char) ('a' + rand.Next(26))).ToArray());
  }

  [Test]
  public void TestUser()
  {
    var users = new List<User>();

    var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
    var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

    foreach (var firstName in firstNames)
    foreach (var lastName in lastNames)
      users.Add(new User($"{firstName} {lastName}"));

    ForceGC();

    dotMemory.Check(memory =>
    {
      WriteLine(memory.SizeInBytes);
    });
  }

  [Test]
  public void TestUser2()
  {
    var users = new List<User2>();

    var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
    var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

    foreach (var firstName in firstNames)
    foreach (var lastName in lastNames)
      users.Add(new User2($"{firstName} {lastName}"));

    ForceGC();

    dotMemory.Check(memory =>
    {
      WriteLine(memory.SizeInBytes);
    });
  }
}