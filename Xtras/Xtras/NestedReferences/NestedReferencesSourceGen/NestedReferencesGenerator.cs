using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace NestedReferencesSourceGen
{
  [Generator]
  public class NestedReferencesGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
      
    }

    public void Execute(GeneratorExecutionContext context)
    {
      context.AddSource("hello", "fail");
    }
  }
}
