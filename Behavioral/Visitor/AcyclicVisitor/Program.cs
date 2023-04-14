using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Acyclic;

public interface IVisitor {} // marker interface
  
public interface IVisitor<in TVisitable>
{
  void Visit(TVisitable obj);
}
  
  
public abstract class Expression
{
  public virtual void Accept(IVisitor visitor)
  {
    if (visitor is IVisitor<Expression> typed)
      typed.Visit(this);
  }
}

public class DoubleExpression : Expression
{
  public readonly double Value;

  public DoubleExpression(double value)
  {
    Value = value;
  }
    
  public override void Accept(IVisitor visitor)
  {
    if (visitor is IVisitor<DoubleExpression> typed)
      typed.Visit(this);
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

  public override void Accept(IVisitor visitor)
  {
    if (visitor is IVisitor<AdditionExpression> typed)
      typed.Visit(this);
  }
}
  
public class ExpressionPrinter : IVisitor,
  IVisitor<Expression>,
  IVisitor<DoubleExpression>,
  IVisitor<AdditionExpression>
{
  private readonly StringBuilder sb = new();

  public void Visit(DoubleExpression de)
  {
    sb.Append(de.Value);
  }

  public void Visit(AdditionExpression ae)
  {
    sb.Append("(");
    ae.Left.Accept(this);
    sb.Append("+");
    ae.Right.Accept(this);
    sb.Append(")");
  }

  public void Visit(Expression obj)
  {
    // default handler?
  }

  public override string ToString() => sb.ToString();
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
  }
}