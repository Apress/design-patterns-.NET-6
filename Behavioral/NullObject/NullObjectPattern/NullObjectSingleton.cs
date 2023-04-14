using System;

namespace NullObjectPattern;

public interface ILog
{
  public void Warn();

  public static ILog Null => NullLog.Instance;

  private sealed class NullLog : ILog
  {
    private NullLog() {}

    private static readonly Lazy<NullLog> instance = new(() => new NullLog());

    public static ILog Instance => instance.Value;

    public void Warn()
    {
        
    }
  }
}

public class BankAccount
{
  public BankAccount(ILog log)
  {
      
  }
}

class Program
{
  static void Main(string[] args)
  {
    var ba = new BankAccount(ILog.Null);
  }
}