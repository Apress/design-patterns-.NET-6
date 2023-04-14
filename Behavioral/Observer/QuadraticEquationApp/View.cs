using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuadraticEquationApp.Annotations;

namespace QuadraticEquationApp
{
  public class View : INotifyPropertyChanged
  {
    private readonly QuadraticEquationSolver solver;

    public View(QuadraticEquationSolver solver)
    {
      this.solver = solver;
    }
    
    public Complex A
    {
      get => solver.A;
      set
      {
        if (value.Equals(solver.A)) return;
        solver.A = value;
        OnPropertyChanged();
        CalculateSolution();
      }
    }

    public Complex B
    {
      get => solver.B;
      set
      {
        if (value.Equals(solver.B)) return;
        solver.B = value;
        OnPropertyChanged();
        CalculateSolution();
      }
    }

    public Complex C
    {
      get => solver.C;
      set
      {
        if (value.Equals(solver.C)) return;
        solver.C = value;
        OnPropertyChanged();
        CalculateSolution();
      }
    }

    private Complex x1, x2;

    public Complex X1
    {
      get => x1;
      set
      {
        if (value.Equals(x1)) return;
        x1 = value;
        OnPropertyChanged();
        CalculateCoefficients();
      }
    }

    public Complex X2
    {
      get => x2;
      set
      {
        if (value.Equals(x2)) return;
        x2 = value;
        OnPropertyChanged();
        CalculateCoefficients();
      }
    }

    public void CalculateSolution()
    {
      (x1, x2) = solver.Solve();
      OnPropertyChanged(nameof(X1));
      OnPropertyChanged(nameof(X2));
    }

    public void CalculateCoefficients()
    {
      (solver.A, solver.B, solver.C) = (1, -(X1 + X2), X1 * X2);
      OnPropertyChanged(nameof(A));
      OnPropertyChanged(nameof(B));
      OnPropertyChanged(nameof(C));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
