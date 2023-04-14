using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace VisitorAdapter
{
  public sealed record Node(string Name, 
    string Value, List<Node> Children);

  public interface IVisitable
  {
    void Accept(IVisitor visitor);
  }

  public interface IVisitor
  {
    void VisitBinaryOp(Node node, string op);
    void VisitInlineOp(Node node, string op); 
  }

  public class VisitableNode : IVisitable
  {
    private readonly Node node;
    public VisitableNode(Node node) => this.node = node;

    public void Accept(IVisitor visitor)
    {
      if (node.Name == "Operator")
      {
        switch (node.Value)
        {
          case "Equal": 
            visitor.VisitInlineOp(node.Children[0], "==");
            break;
          case "Greater":
            visitor.VisitInlineOp(node.Children[0], ">");
            break;
          case "And":
            visitor.VisitBinaryOp(node, "&&");
            break;
        }
      }
    }
  }

  public class NodePrinter : IVisitor
  {
    private StringBuilder sb = new();

    public void VisitBinaryOp(Node node, string op)
    {
      sb.Append("(");
      node.Children[0].ToVisitable().Accept(this);
      sb.Append($" {op} ");
      node.Children[1].ToVisitable().Accept(this);
      sb.Append(")");
    }

    public void VisitInlineOp(Node node, string op)
    {
      sb.Append($"({node.Name} {op} {node.Value})");
    }

    public override string ToString() => sb.ToString();
  }

  public static class NodeExtensions
  {
    public static IVisitable ToVisitable(this Node node)
    {
      return new VisitableNode(node);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      var node = JsonConvert.DeserializeObject<Node>(
        File.ReadAllText("input.json"));
      NodePrinter p = new();
      node.ToVisitable().Accept(p);
      Console.WriteLine(p);
      // ((Age > 16) && (Citizen == True))
    }
  }
}
