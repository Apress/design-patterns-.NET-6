using System;
using System.Collections.Generic;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Memento.UndoRedo;
// Revision history:
//
// 2020-08-05 Corrected changes to overwrite possible redo ops. 

public class Memento
{
  public int Balance { get; }

  public Memento(int balance)
  {
    Balance = balance;
  }
}

public class BankAccount // supports undo/redo
{
  private int balance;
  private List<Memento> changes = new();
  private int current = -1;
    
  // debug only!
  public int ChangeCount => changes.Count;

  private Memento addChange(Memento m)
  {
    // if there are changes after current, erase them
    if (changes.Count > current+1)
      changes.RemoveRange(current+1, changes.Count - current - 1);
    changes.Add(m);
    ++current;
    return m;
  }
    
  public BankAccount(int balance)
  {
    this.balance = balance;
    addChange(new Memento(balance));
  }

  public Memento Deposit(int amount)
  {
    balance += amount;
    return addChange(new Memento(balance));
  }

  public void Restore(Memento m)
  {
    if (m != null)
    {
      balance = m.Balance;
      changes.Add(m);
      current = changes.Count - 1;
    }
  }

  public Memento Undo()
  {
    if (current > 0)
    {
      var m = changes[--current];
      balance = m.Balance;
      return m;
    }
    return null;
  }

  public Memento Redo()
  {
    if (current + 1 < changes.Count)
    {
      var m = changes[++current];
      balance = m.Balance;
      return m;
    }
    return null;
  }

  public override string ToString()
  {
    return $"{nameof(balance)}: {balance}";
  }
}

public class Demo
{
  static void Main(string[] args)
  {
    var ba = new BankAccount(100);
    ba.Deposit(50);
    ba.Deposit(25);
    WriteLine(ba);

    ba.Undo();
    WriteLine($"Undo 1: {ba}");
    ba.Undo();
    WriteLine($"Undo 2: {ba}");
    ba.Redo();
    WriteLine($"Redo 2: {ba}");
      
    // check that we can alter after multiple undos
    ba.Undo();
    ba.Undo();
    ba.Deposit(100);
    Console.WriteLine(ba);
    Console.WriteLine(ba.ChangeCount);
  }
}