namespace PatternDemosCore.Structural.Decorator;

using static System.Console;
  
public interface ICreature
{
  int Age { get; set; }
}

public interface IBird : ICreature
{
  void Fly()
  {
    if (Age >= 10)
      WriteLine("I am flying");
  }
}

public interface ILizard : ICreature
{
  void Crawl()
  {
    if (Age < 10)
      WriteLine("I am crawling!");
  }
}

public class Dragon : IBird, ILizard
{
  // multiple inheritance?
  public int Age { get; set; }
}

public class Demo
{
  public static void Main_(string[] args)
  {
    var d = new Dragon {Age = 5};

    if (d is IBird bird)
      bird.Fly();

    if (d is ILizard lizard)
      lizard.Crawl();
  }
}