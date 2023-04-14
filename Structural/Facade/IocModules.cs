using System;
using Autofac;

namespace DotNetDesignPatternDemos.Structural.Facade;

public interface IShape
{
  void Draw();
}
  
public interface IRectangle : IShape {}
public interface ISquare : IShape{}

public class Square : ISquare
{
  public void Draw() => Console.WriteLine("Basic square");
}

public class Rectangle : IRectangle
{
  public void Draw() => Console.WriteLine("Basic rectangle");
}

public class RoundedSquare : ISquare
{
  public void Draw() => Console.WriteLine("Rounded square");
}

public class RoundedRectangle : IRectangle
{
  public void Draw() => Console.WriteLine("Rounded rectangle");
}

class BasicModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<Square>().As<ISquare>();
    builder.RegisterType<Rectangle>().As<IRectangle>();
  }
}
  
class RoundedModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<RoundedSquare>().As<ISquare>();
    builder.RegisterType<RoundedRectangle>().As<IRectangle>();
  }
}
  
public class Demo
{
  public static void Main()
  {
    var b = new ContainerBuilder();
    //b.RegisterModule<BasicModule>();
    b.RegisterModule<RoundedModule>();
    var c = b.Build();
    var square = c.Resolve<ISquare>();
    square.Draw(); // Rounded square
  }
}