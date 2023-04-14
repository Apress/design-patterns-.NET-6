using System;
using System.Dynamic;
using System.Reflection;
using ImpromptuInterface;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.NullObject.Dynamic;

public interface ILog
{
  void Info(string msg);
  bool Warn(string msg);
}

class ConsoleLog : ILog
{
  public void Info(string msg)
  {
    WriteLine(msg);
  }
  
  public bool Warn(string msg)
  {
    WriteLine("WARNING: " + msg);
    return true;
  }
}

class OptionalLog: ILog
{
  private ILog impl;
  public static ILog NoLogging = null;

  public OptionalLog(ILog impl)
  {
    this.impl = impl;
  }

  public void Info(string msg)
  {
    impl?.Info(msg);
  }

  public bool Warn(string msg)
  {
    throw new NotImplementedException();
  }
}

public class BankAccount
{
  private ILog log;
  private int balance;

  public BankAccount(ILog log)
  {
    //this.log = new OptionalLog(log);
    this.log = log;
  }

  public void Deposit(int amount)
  {
    balance += amount;
    // check for null everywhere
    log?.Info($"Deposited ${amount}, balance is now {balance}");
  }

  public void Withdraw(int amount)
  {
    if (balance >= amount)
    {
      balance -= amount;
      log?.Info($"Withdrew ${amount}, we have ${balance} left");
    }
    else
    {
      log?.Warn($"Could not withdraw ${amount} because " +
                $"balance is only ${balance}");
    }
  }
}

public class Null<T> : DynamicObject where T:class
{
  public static T Instance
  {
    get
    {
      if (!typeof(T).IsInterface)
        throw new ArgumentException("I must be an interface type");
    
      return new Null<T>().ActLike<T>();
    }
  }
    
  public static Type GetUnderlyingType(MemberInfo member)
  {
    switch (member.MemberType)
    {
      case MemberTypes.Event:
        return ((EventInfo)member).EventHandlerType;
      case MemberTypes.Field:
        return ((FieldInfo)member).FieldType;
      case MemberTypes.Method:
        return ((MethodInfo)member).ReturnType;
      case MemberTypes.Property:
        return ((PropertyInfo)member).PropertyType;
      default:
        throw new ArgumentException
        (
          "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
        );
    }
  }
  
  public override bool TryInvokeMember(InvokeMemberBinder binder, 
    object[] args, out object result)
  {
    // cannot rely on binder.ReturnType
    var returnType = GetUnderlyingType(typeof(T).GetMember(binder.Name)[0]);
    if (returnType == typeof(void) || !returnType.IsValueType)
      result = null;
    else
      result = Activator.CreateInstance(returnType);
    return true;
  }
}

public class Demo
{
  static void Main()
  {
    var log = Null<ILog>.Instance;
    var ba = new BankAccount(log);
    ba.Deposit(100);
    ba.Withdraw(200);
  }
}