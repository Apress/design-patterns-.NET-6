using System;
using System.ComponentModel;
using System.Transactions;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Transformer
{
  public abstract class Expression
  {
    public abstract T Reduce<T>(ITransformer<T> transformer);
  }

  public class DoubleExpression : Expression
  {
    public readonly double Value;

    public DoubleExpression(double value)
    {
      Value = value;
    }

    public override T Reduce<T>(ITransformer<T> transformer)
    {
      return transformer.Transform(this);
    }
  }

  public class AdditionExpression : Expression
  {
    public readonly Expression Left, Right;

    public AdditionExpression(Expression left, Expression right)
    {
      Left = left;
      Right = right;
    }

    public override T Reduce<T>(ITransformer<T> transformer)
    {
      return transformer.Transform(this);
    }
  }

  public interface ITransformer<T>
  {
    T Transform(DoubleExpression de);
    T Transform(AdditionExpression ae);
  }

  public class EvaluationTransformer : ITransformer<double>
  {
    public double Transform(DoubleExpression de) => de.Value;

    public double Transform(AdditionExpression ae)
    {
      return ae.Left.Reduce(this) + ae.Right.Reduce(this);
    }
  }

  public class PrintTransformer : ITransformer<string>
  {
    public string Transform(DoubleExpression de)
    {
      return de.Value.ToString();
    }

    public string Transform(AdditionExpression ae)
    {
      return $"({ae.Left.Reduce(this)} + {ae.Right.Reduce(this)})";
    }
  }

  public class SquareTransformer : ITransformer<Expression>
  {
    public Expression Transform(DoubleExpression de)
    {
      return new DoubleExpression(de.Value * de.Value);
    }

    public Expression Transform(AdditionExpression ae)
    {
      return new AdditionExpression(
        ae.Left.Reduce(this), 
        ae.Right.Reduce(this));
    }
  }

  public class Program
  {
    static void Main()
    {
      var expr = new AdditionExpression(
        new DoubleExpression(1), new DoubleExpression(2));
      var et = new EvaluationTransformer();
      var result = expr.Reduce(et);
      var pt = new PrintTransformer();
      var text = expr.Reduce(pt);
      Console.WriteLine($"{text} = {result}"); // (1 + 2) = 3

      var st = new SquareTransformer();
      var newExpr = expr.Reduce(st);
      text = newExpr.Reduce(pt);
      Console.WriteLine(text); // (1 + 4)
    }
  }
}
