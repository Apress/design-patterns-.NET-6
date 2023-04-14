using System;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.AbstractVirtual
{
  public abstract class Expr
  {
    public Expr Parent;

    public virtual void Accept(ExpressionVisitor visitor) {}
  }

  public class DoubleExpression : Expr
  {
    public readonly double Value;

    public DoubleExpression(double value)
    {
      Value = value;
    }

    public override void Accept(ExpressionVisitor visitor)
    {
      visitor.Visit(this);
    }
  }

  public class AbsoluteDoubleExpression : DoubleExpression
  {
    public AbsoluteDoubleExpression(double value) : base(value) {}

    // try commenting this and see what happens
    public override void Accept(ExpressionVisitor visitor)
    {
      visitor.Visit(this);
    }
  }

  public abstract class ExpressionVisitor
  {
    public virtual void Visit(DoubleExpression de) {}
    public virtual void Visit(AbsoluteDoubleExpression ade) {}
  }

  public class ExpressionPrinter : ExpressionVisitor
  {
    protected readonly StringBuilder sb = new();
  
    public override void Visit(DoubleExpression de)
    {
      sb.Append(de.Value);
    }

    public override void Visit(AbsoluteDoubleExpression ade)
    {
      sb.Append($"|{ade.Value}|");
    }

    public override string ToString() => sb.ToString();
  }

  public class ExpressionCalculator : ExpressionVisitor
  {
    public double Result;

    public override void Visit(DoubleExpression de)
    {
      Result = de.Value;
    }

    public override void Visit(AbsoluteDoubleExpression ade)
    {
      Result = Math.Abs(ade.Value);
    }
  }

  public static class ExpressionExtensions
  {
    public static void Accept(this Expr expression, 
      params ExpressionVisitor[] visitors)
    {
      foreach (var visitor in visitors)
        expression.Accept(visitor);
    }
  }

  public class Demo
  {
    public static void Main()
    {
      var e = new AbsoluteDoubleExpression(-3);
      var ev = new ExpressionCalculator();
      var ep = new ExpressionPrinter();
      e.Accept(ep, ev);
      WriteLine($"{ep} = {ev.Result}"); // |-3| = 3
    }
  }
}