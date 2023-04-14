using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NullVisitable
{
  public interface INode
  {
    void Accept(IVisitor visitor);
  }

  public record Literal(double Value) : INode
  {
    public virtual void Accept(IVisitor visitor)
    {
      visitor.Visit(this);
    }
  }

  public record BinaryExpression(INode Left, INode Right, char Op) : INode
  {
    public void Accept(IVisitor visitor)
    {
      visitor.Visit(this);
    }
  }

  public record Variable(string Name) : INode
  {
    public void Accept(IVisitor visitor)
    {
      visitor.Visit(this);
    }
  }

  public sealed record NullLiteral() : Literal(0)
  {
    public override void Accept(IVisitor visitor)
    {
      if (visitor is not ExpressionPrinter)
        base.Accept(visitor);
    }
  }

  public interface IVisitor
  {
    void Visit(Literal lit);
    void Visit(BinaryExpression be);
    void Visit(Variable v);
  }

  public class ExpressionPrinter : IVisitor
  {
    private readonly StringBuilder sb = new();
    public override string ToString() => sb.ToString();
    
    public void Visit(Literal lit)
    {
      sb.Append(lit.Value);
    }

    public void Visit(BinaryExpression be)
    {
      var (left, right, op) = be;
      sb.Append("(");
      left.Accept(this);
      sb.Append(op);
      right.Accept(this);
      sb.Append(")");
    }

    public void Visit(Variable v)
    {
      sb.Append(v.Name);
    }
  }

  public class ExpressionCalculator : IVisitor
  {
    public double Value;
    private readonly Dictionary<string, double> variables;

    public ExpressionCalculator(Dictionary<string, double> variables) 
      => this.variables = variables;

    public void Visit(Literal lit) => Value = lit.Value;

    public void Visit(BinaryExpression be)
    {
      var (left, right, op) = be;
      left.Accept(this);
      var first = Value;
      right.Accept(this);
      switch (op)
      {
        case '-': 
          Value = first - Value;
          break;
      }
    }

    public void Visit(Variable v)
    {
      if (variables.ContainsKey(v.Name))
        Value = variables[v.Name];
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      var exp = new BinaryExpression(new NullLiteral(), new Variable("X"), '-');
      var printer = new ExpressionPrinter();
      var calculator = new ExpressionCalculator(new Dictionary<string, double> { ["X"] = 123 });
      exp.Accept(printer);
      exp.Accept(calculator);
      Console.WriteLine($"{printer} = {calculator.Value}"); 
      // (-X) = -123
    }
  }
}
