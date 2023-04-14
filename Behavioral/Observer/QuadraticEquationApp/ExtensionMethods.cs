using System.Linq;

namespace QuadraticEquationApp
{
  public static class ExtensionMethods
  {
    public static bool IsOneOf<T>(this T item, params T[] list)
    {
      return list.Contains(item);
    }
  }
}