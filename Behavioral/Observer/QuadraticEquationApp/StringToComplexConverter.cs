using System;
using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace QuadraticEquationApp
{
  [ValueConversion(typeof(Complex), typeof(string))]
  public class ComplexToStringConverter : IValueConverter
  {
    /// <summary>
    /// A rather clumsy string representation of a complex number.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var c = (Complex) value;
      if (c.Imaginary == 0.0)
        return c.Real.ToString();
      return c.ToString();
    }

    /// <summary>
    /// Here we only parse double-precision numbers.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null) return Complex.Zero;
      if (double.TryParse(value.ToString(), out var d))
        return new Complex(d, 0);
      return Complex.Zero;
    }
  }
}