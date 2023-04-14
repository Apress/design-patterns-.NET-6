using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;


namespace DotNetDesignPatternDemos.Behavioral.Visitor.Reflective;

using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

public abstract class Expression
{
}

public class DoubleExpression : Expression
{
  public double Value;

  public DoubleExpression(double value)
  {
    Value = value;
  }
}

public class AdditionExpression : Expression
{
  public Expression Left;
  public Expression Right;

  public AdditionExpression(Expression left, Expression right)
  {
    Left = left;
    Right = right;
  }
}

  

public static class ExpressionPrinter
{
  private static readonly DictType actions = new()
  {
    [typeof(DoubleExpression)] = (e, sb) =>
    {
      var de = (DoubleExpression) e;
      sb.Append(de.Value);
    },
    [typeof(AdditionExpression)] = (e, sb) =>
    {
      var ae = (AdditionExpression) e;
      sb.Append("(");
      Print(ae.Left, sb);
      sb.Append("+");
      Print(ae.Right, sb);
      sb.Append(")");
    }
  };

  public static void Print(DoubleExpression de, 
    StringBuilder sb)
  {
    sb.Append(de.Value);
  }

  public static void Print(AdditionExpression ae, 
    StringBuilder sb)
  {
    sb.Append("(");
    Print((dynamic)ae.Left, sb);  // ok
    sb.Append("+");
    Print((dynamic)ae.Right, sb); // ok
    sb.Append(")");
  }

  public static void Print2(this Expression e, StringBuilder sb)
  {
    actions[e.GetType()](e, sb);
  }

  public static void Print(Expression e, StringBuilder sb)
  {
    switch (e)
    {
      case DoubleExpression de:
        sb.Append(de.Value);
        break;
      case AdditionExpression ae:
        sb.Append("(");
        Print(ae.Left, sb);
        sb.Append("+");
        Print(ae.Right, sb);
        sb.Append(")");
        break;
      default:
        // your choice what to do here
        throw new Exception("Unsupported expression type");
    }

    // breaks open-closed principle
    // will work incorrectly on missing case
  }
}

public class Demo
{
  public static void Main()
  {
    var e = new AdditionExpression(
      left: new DoubleExpression(1),
      right: new AdditionExpression(
        left: new DoubleExpression(2),
        right: new DoubleExpression(3)));
    var sb = new StringBuilder();
    e.Print2(sb); // extension method goodness!
    WriteLine(sb);
  }
}