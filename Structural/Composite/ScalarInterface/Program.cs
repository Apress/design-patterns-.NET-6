using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetDesignPatternDemos.Structural.Composite.ScalarInterface
{

  public static class ExtensionMethods
  {
    public static void ConnectTo(
      this IEnumerable<Neuron> self, 
      IEnumerable<Neuron> other)
    {
      if (ReferenceEquals(self, other)) return;

      foreach (var from in self)
      foreach (var to in other)
      {
        from.Out.Add(to);
        to.In.Add(from);
      }
    }
  }
  
  public interface IScalar<out T> : IEnumerable<T>
  {
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      yield return (T) this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  public class Neuron : IScalar<Neuron>
  {
    public readonly List<Neuron> In = new(), Out = new();
  }

  public class NeuronLayer : Collection<Neuron>
  {
    public NeuronLayer(int count)
    {
      while (count --> 0)
        Add(new Neuron());
    }
  }

  public class Demo
  {
    static void Main(string[] args)
    {
      var neuron1 = new Neuron();
      var neuron2 = new Neuron();
      var layer1 = new NeuronLayer(3);
      var layer2 = new NeuronLayer(4);

      neuron1.ConnectTo(neuron2);
      neuron1.ConnectTo(layer1);
      layer2.ConnectTo(neuron1);
      layer1.ConnectTo(layer2);

      //var enumerator = neuron1.GetEnumerator();
    }
  }
}