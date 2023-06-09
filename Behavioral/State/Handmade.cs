using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DotNetDesignPatternDemos.Behavioral.State.Handmade;

public enum State
{
  OffHook,
  Connecting,
  Connected,
  OnHold,
  OnHook
}

public enum Trigger
{
  CallDialed,
  HungUp,
  CallConnected,
  PlacedOnHold,
  TakenOffHold,
  LeftMessage
}

public class Demo
{
  private static Dictionary<State, List<(Trigger, State)>> rules
    = new()
    {
      [State.OffHook] = new ()
      {
        (Trigger.CallDialed, State.Connecting)
      },
      [State.Connecting] = new()
      {
        (Trigger.HungUp, State.OnHook),
        (Trigger.CallConnected, State.Connected)
      },
      [State.Connected] = new ()
      {
        (Trigger.LeftMessage, State.OnHook),
        (Trigger.HungUp, State.OnHook),
        (Trigger.PlacedOnHold, State.OnHold)
      },
      [State.OnHold] = new ()
      {
        (Trigger.TakenOffHold, State.Connected),
        (Trigger.HungUp, State.OnHook)
      }
    };

  public static void MainInteractive(string[] args)
  {
    State state = State.OffHook, exitState = State.OnHook;
    do
    {
      Console.WriteLine($"The phone is currently {state}");
      Console.WriteLine("Select a trigger:");

      for (var i = 0; i < rules[state].Count; i++)
      {
        var (t, _) = rules[state][i];
        Console.WriteLine($"{i}. {t}");
      }

      int input = int.Parse(Console.ReadLine());

      var (_, s) = rules[state][input];
      state = s;
    } while (state != exitState);
    Console.WriteLine("We are done using the phone.");
  }
    
  public static void Main(string[] args)
  {
    State state = State.OffHook, exitState = State.OnHook;

    var queue = new Queue<int>(new[]{0, 1, 2, 0, 0});

    do
    {
      Console.WriteLine($"The phone is currently {state}");
      Console.WriteLine("Select a trigger:");

      for (var i = 0; i < rules[state].Count; i++)
      {
        var (t, _) = rules[state][i];
        Console.WriteLine($"{i}. {t}");
      }

      int input = queue.Dequeue();
      Console.WriteLine(input);

      var (_, s) = rules[state][input];
      state = s;
    } while (state != exitState);
    Console.WriteLine("We are done using the phone.");
  }

}