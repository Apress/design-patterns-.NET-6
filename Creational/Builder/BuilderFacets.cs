﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.BuilderFacets;

public class Person
{
  public Person()
  {
    WriteLine("Creating an instance of Person");
  }

  // address
  public string StreetAddress, Postcode, City;
  
  // employment info
  public string CompanyName, Position;
  
  public int AnnualIncome;
  
  public override string ToString()
  {
    return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}," +
           $" {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}," +
           $" {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
  }
}

public class PersonBuilder // facade 
{
  // the object we're going to build
  protected readonly Person person;

  public PersonBuilder() => person = new Person();
  protected PersonBuilder(Person person) => this.person = person;

  public PersonAddressBuilder Lives => new(person);
  public PersonJobBuilder Works => new(person);
  
  public static implicit operator Person(PersonBuilder pb)
  {
    return pb.person;
  }
}

public class PersonJobBuilder : PersonBuilder
{
  public PersonJobBuilder(Person person) : base(person) {}

  public PersonJobBuilder At(string companyName)
  {
    person.CompanyName = companyName;
    return this;
  }

  public PersonJobBuilder AsA(string position)
  {
    person.Position = position;
    return this;
  }

  public PersonJobBuilder Earning(int annualIncome)
  {
    person.AnnualIncome = annualIncome;
    return this;
  }
}

public class PersonAddressBuilder : PersonBuilder
{
  public PersonAddressBuilder(Person person) : base(person) {}
  
  public PersonAddressBuilder At(string streetAddress)
  {
    person.StreetAddress = streetAddress;
    return this;
  }
  
  public PersonAddressBuilder WithPostcode(string postcode)
  {
    person.Postcode = postcode;
    return this;
  }
  
  public PersonAddressBuilder In(string city)
  {
    person.City = city;
    return this;
  }
}
  

public class Demo
{
  static void Main(string[] args)
  {
    var pb = new PersonBuilder();
    Person person = pb
      .Lives
      .At("123 London Road")
      .In("London")
      .WithPostcode("SW12BC")
      .Works
      .At("Fabrikam")
      .AsA("Engineer")
      .Earning(123000);

    WriteLine(person);
  }
}