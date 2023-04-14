using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.NestedBuilder;

public record HtmlElement
{
  public string Name;
  public string Text;
  private readonly Lazy<List<HtmlElement>> elements = new();
  public IReadOnlyList<HtmlElement> Elements => elements.Value;
  private const int indentSize = 2;

  protected HtmlElement() : this("", "") { }

  protected HtmlElement(string name, string text)
  {
    Name = name;
    Text = text;
  }

  private string ToStringImpl(int indent)
  {
    var sb = new StringBuilder();
    var i = new string(' ', indentSize * indent);
    sb.Append($"{i}<{Name}>\n");
    if (!string.IsNullOrWhiteSpace(Text))
    {
      sb.Append(new string(' ', indentSize * (indent + 1)));
      sb.Append(Text);
      sb.Append('\n');
    }

    foreach (var e in Elements)
      sb.Append(e.ToStringImpl(indent + 1));

    sb.Append($"{i}</{Name}>\n");
    return sb.ToString();
  }

  public override string ToString()
  {
    return ToStringImpl(0);
  }
    
  public static HtmlBuilder Create(string name) => new(name);
  
  public class HtmlBuilder
  {
    private readonly string rootName;
    protected HtmlElement root = new();

    public HtmlBuilder(string rootName)
    {
      this.rootName = rootName;
      root.Name = rootName;
    }

    // not fluent
    public void AddChild(string childName, string childText)
    {
      var e = new HtmlElement(childName, childText);
      root.elements.Value.Add(e);
    }

    public HtmlBuilder AddChildFluent(string childName, string childText)
    {
      var e = new HtmlElement(childName, childText);
      root.elements.Value.Add(e);
      return this;
    }

    public static HtmlBuilder Init(string rootName)
    {
      return new HtmlBuilder(rootName);
    }

    public override string ToString() => root.ToString();

    public void Clear()
    {
      root = new HtmlElement{Name = rootName};
    }

    public HtmlElement Build() => root;

    public static implicit operator HtmlElement(HtmlBuilder builder)
    {
      return builder.root;
    }
  }

  public void Deconstruct(out string Name, out string Text)
  {
    Name = this.Name;
    Text = this.Text;
  }
}

public class Demo
{
  static void Main(string[] args)
  {
    var e = HtmlElement.Create("ul")
      .AddChildFluent("li", "hello")
      .AddChildFluent("li", "world")
      .Build();
    WriteLine(e);
  }
}