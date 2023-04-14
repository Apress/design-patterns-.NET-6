using System;
using DotNetDesignPatternDemos.Creational.Prototype;
using NUnit.Framework.Internal.Commands;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.PrototypeFactory;

public record Address(string StreetAddress, string City, int Suite)
{
  public string StreetAddress = StreetAddress, City = City;
  public int Suite = Suite;

  public Address(Address other)
  {
    StreetAddress = other.StreetAddress;
    City = other.City;
    Suite = other.Suite;
  }
}

public record Employee(string Name, Address Address)
{
  public string Name = Name;
  public Address Address = Address;

  public Employee(Employee other)
  {
    Name = other.Name;
    Address = new Address(other.Address);
  }

  public override string ToString()
  {
    return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address}";
  }
    
  //partial class EmployeeFactory {}
}

public class EmployeeFactory
{
  private static readonly Employee main = new(null, new Address("123 East Dr", "London", 0));
  private static readonly Employee aux = new(null, new Address("123B East Dr", "London", 0));
  
  private static Employee NewEmployee(Employee proto, string name, int suite)
  {
    var copy = proto.DeepCopy();
    copy.Name = name;
    copy.Address.Suite = suite;
    return copy;
  }
  
  public static Employee NewMainOfficeEmployee(string name, int suite) =>
    NewEmployee(main, name, suite);
  
  public static Employee NewAuxOfficeEmployee(string name, int suite) =>
    NewEmployee(aux, name, suite);
}

public class Demo
{
  static Employee main = new(null, new Address("123 East Dr", "London", 0));
    
  static void Main()
  {
    var main = new Employee(null, new Address("123 East Dr", "London", 0));
    var aux = new Employee(null, new Address("123B East Dr", "London", 0));
      
      
    var john = new Employee("John", new Address("123 London Road", "London", 123));

    //var chris = john;
    var jane = new Employee(john);

    jane.Name = "Jane";
      
    WriteLine(john); // oop s, john is called chris
    WriteLine(jane);


  }
}