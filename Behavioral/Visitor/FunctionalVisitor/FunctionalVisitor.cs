using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using static System.Console;


namespace DotNetDesignPatternDemos.Behavioral.Visitor.Functional
{
  public abstract class Expression
  {
    public void Match(
      Action<DoubleExpression> visitDoubleExpression,
      Action<AdditionExpression> visitAdditionExpression,
      Action<AbsoluteDoubleExpression> visitAbsoluteDoubleExpression,
      Action<Expression> visitUnknownExpression = null)
    {
      switch (this)
      {
        case AbsoluteDoubleExpression e:
          visitAbsoluteDoubleExpression(e);
          break;
        case DoubleExpression e: 
          visitDoubleExpression(e);
          break;
        case AdditionExpression e:
          visitAdditionExpression(e);
          break;
        default:
          visitUnknownExpression?.Invoke(this);
          break;
      }
    }

    public T Match<T>(
      Func<DoubleExpression, T> visitDoubleExpression,
      Func<AdditionExpression, T> visitAdditionExpression,
      Func<AbsoluteDoubleExpression, T> visitAbsoluteDoubleExpression,
      Func<Expression, T> visitUnknownExpression = null) =>
      
      this switch
      {
        AbsoluteDoubleExpression e => visitAbsoluteDoubleExpression(e),
        AdditionExpression e => visitAdditionExpression(e),
        DoubleExpression e => visitDoubleExpression(e),
        { } => visitUnknownExpression switch
        {
          null => default(T),
          _ => visitUnknownExpression(this)
        }
      };
  }

  public class DoubleExpression : Expression
  {
    public readonly double Value;

    public DoubleExpression(double value)
    {
      Value = value;
    }
  }

  public class AbsoluteDoubleExpression : DoubleExpression
  {
    public AbsoluteDoubleExpression(double value) : base(value)
    {

    }
  }

  public class AdditionExpression : Expression
  {
    public readonly Expression Left, Right;

    public AdditionExpression(Expression left, Expression right)
      => (Left, Right) = (left, right);
  }
  
  /// <summary>
  /// Stateful visitor implementation.
  /// </summary>
  public class ExpressionPrinter
  {
    private readonly StringBuilder sb = new();

    public string Print(Expression e)
    {
      e.Match(Visit, Visit, Visit);
      return sb.ToString();
    }

    private void Visit(AbsoluteDoubleExpression ade)
    {
      sb.Append($"|{ade.Value}|");
    }

    private void Visit(AdditionExpression ae)
    {
      sb.Append('(');
      Print(ae.Left);
      sb.Append('+');
      Print(ae.Right);
      sb.Append(')');
    }

    private void Visit(DoubleExpression de)
    {
      sb.Append(de.Value);
    }
  }

  public class ExpressionPrinter2
  {
    private string Visit(DoubleExpression de)
    {
      return de.Value.ToString();
    }

    private string Visit(AdditionExpression ae)
    {
      return "(" + Print(ae.Left) + "+" + Print(ae.Right) + ")";
    }

    private string Visit(AbsoluteDoubleExpression ade)
    {
      return $"|{ade.Value}|";
    }

    public string Print(Expression e)
    {
      return e.Match(Visit, Visit, Visit);
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
          right: new AbsoluteDoubleExpression(-3)));
      var ep = new ExpressionPrinter2();
      WriteLine(ep.Print(e));
    }
  }
}