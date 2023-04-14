using System;
using System.Numerics;
using System.Threading.Tasks;

namespace QuadraticEquationApp
{
  public class QuadraticEquationSolver
  {
    public Complex A, B, C;

    public Complex Discriminant => B * B - 4 * A * C;

    public Tuple<Complex, Complex> Solve()
    {
      var rootDisc = Complex.Sqrt(Discriminant);
      return Tuple.Create(
        (-B + rootDisc) / (2 * A),
        (-B - rootDisc) / (2 * A));
    }
  }
}