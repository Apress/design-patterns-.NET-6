using System;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Intrusive;

public abstract class Expression
{
  // adding a new operation
  public abstract void Print(StringBuilder sb);
}

public class DoubleExpression : Expression
{
  private double value;

  public DoubleExpression(double value)
  {
    this.value = value;
  }

  public override void Print(StringBuilder sb)
  {
    sb.Append(value);
  }
}

public class AdditionExpression : Expression
{
  private Expression left, right;

  public AdditionExpression(Expression left, Expression right)
  {
    this.left = left;
    this.right = right;
  }

  public override void Print(StringBuilder sb)
  {
    sb.Append("(");
    left.Print(sb);
    sb.Append("+");
    right.Print(sb);
    sb.Append(")");
  }
}

public class Demo
{
  private static void Main(string[] args)
  {
    var e = new AdditionExpression(
      new DoubleExpression(1),
      new AdditionExpression(
        new DoubleExpression(2),
        new DoubleExpression(3)));
    var sb = new StringBuilder();
    e.Print(sb);
    WriteLine(sb);

    // what is more likely: new type or new operation
  }
}