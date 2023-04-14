using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Classic;

public abstract class Expression
{
  public Expression Parent;
  public virtual void Accept(IExpressionVisitor visitor) {}

  public void Accept(params IExpressionVisitor[] visitors)
  {
    foreach (var v in visitors)
    {
      Accept(v);
    }
  }
}

public class DoubleExpression : Expression
{
  public readonly double Value;

  public DoubleExpression(double value) => Value = value;

  public override void Accept(IExpressionVisitor visitor) 
    => visitor.Visit(this);
}

public class AdditionExpression : Expression
{
  public readonly Expression Left, Right;

  public AdditionExpression(Expression left, Expression right)
  {
    Left = left;
    Right = right;
    Left.Parent = Right.Parent = this;
  }

  public override void Accept(IExpressionVisitor visitor)
  {
    visitor.Visit(this);
  }
}

public class MultiplicationExpression : AdditionExpression
{
  public MultiplicationExpression(Expression left, Expression right) 
    : base(left, right) {}

  public override void Accept(IExpressionVisitor visitor)
  {
    visitor.Visit(this);
  }
}

public interface IExpressionVisitor
{
  void Visit(DoubleExpression de);
  void Visit(AdditionExpression ae);
}

public class ExpressionPrinter : IExpressionVisitor
{
  private readonly StringBuilder sb = new();
  
  public void Visit(DoubleExpression de)
  {
    sb.Append(de.Value);
  }
  
  public void Visit(AdditionExpression ae)
  {
    bool needBraces = ae.Parent is MultiplicationExpression;
    if (needBraces) sb.Append("(");
    ae.Left.Accept(this);
    sb.Append("+");
    ae.Right.Accept(this);
    if (needBraces) sb.Append(")");
  }

  public void Visit(MultiplicationExpression ae)
  {
    ae.Left.Accept(this);
    sb.Append("*");
    ae.Right.Accept(this);
  }
  
  public override string ToString() => sb.ToString();
}

public class ExpressionCalculator : IExpressionVisitor
{
  public double Result;

  // what you really want is double Visit(...)

  public void Visit(DoubleExpression de)
  {
    Result = de.Value;
  }

  public void Visit(AdditionExpression ae)
  {
    ae.Left.Accept(this);
    var a = Result;
    ae.Right.Accept(this);
    var b = Result;
    Result = a + b;
  }

  public void Visit(MultiplicationExpression ae)
  {
    ae.Left.Accept(this);
    var a = Result;
    ae.Right.Accept(this);
    var b = Result;
    Result = a * b;
  }
}

public static class ExtensionMethods
{
  public static string Print(this DoubleExpression e)
  {
    var ep = new ExpressionPrinter();
    ep.Visit(e);
    return ep.ToString();
  }
}

public class Demo
{
  public static void Main()
  {
    var e = new AdditionExpression(
      new DoubleExpression(1),
      new AdditionExpression(
        new DoubleExpression(2),
        new DoubleExpression(3)));
    var ep = new ExpressionPrinter();
    ep.Visit(e);
    WriteLine(ep.ToString());

    var calc = new ExpressionCalculator();
    calc.Visit(e);
    WriteLine($"{ep} = {calc.Result}");


    var e2 = new MultiplicationExpression(
      new DoubleExpression(1),
      new AdditionExpression(
        new DoubleExpression(2),
        new DoubleExpression(3)));
    ep = new ExpressionPrinter();
    ep.Visit(e2);
    WriteLine(ep.ToString()); // 1*(2+3)

    calc = new ExpressionCalculator();
    calc.Visit(e2);
    WriteLine($"{ep} = {calc.Result}");
  }
}