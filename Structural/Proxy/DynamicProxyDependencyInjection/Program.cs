using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Castle.DynamicProxy;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Proxy.DynamicProxyDependencyInjection;

public class Logger : IInterceptor
{
  private TextWriter writer;

  public Logger(TextWriter writer)
  {
    this.writer = writer;
  }

  public void Intercept(IInvocation invocation)
  {
    writer.Write("Calling {0} with {1}",
      invocation.Method.Name,
      string.Join("", 
        invocation.Arguments.Select(a => (a ?? "").ToString())));
    
    invocation.Proceed();
    
    writer.Write($"Result is {invocation.ReturnValue}");
  }
}


[Intercept(typeof(Logger))]
public class BankAccount
{
  private int balance;

  public virtual void Deposit(int amount)
  {
    balance += amount;
  }
}


public class Demo
{
  public static void Main()
  {
    var builder = new ContainerBuilder();
    builder.RegisterType<BankAccount>()
      .EnableClassInterceptors();
    builder.Register(c => new Logger(Console.Out));

    var container = builder.Build();
    var instance = container.Resolve<BankAccount>();
    instance.Deposit(100);
  }
}