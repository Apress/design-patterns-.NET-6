using System;

namespace ISP
{
  public class Document
  {
  }
  
  public interface IMachine
  {
    void Print(Document d);
    void Fax(Document d);
    void Scan(Document d);
  }
  
  // ok if you need a multifunction machine
  public class MultiFunctionPrinter : IMachine
  {
    public void Print(Document d)
    {
      //
    }
  
    public void Fax(Document d)
    {
      //
    }
  
    public void Scan(Document d)
    {
      //
    }
  }
  
  public class OldFashionedPrinter : IMachine
  {
    public void Print(Document d)
    {
      // yep
    }
  
    public void Fax(Document d)
    {
      throw new NotImplementedException();
    }
  
    [Obsolete("Not supported", true)]
    public void Scan(Document d)
    {
      throw new NotImplementedException();
    }
  }
  
  public interface IPrinter
  {
    void Print(Document d);
  }
  
  public interface IScanner
  {
    void Scan(Document d);
  }
  
  public class Printer : IPrinter
  {
    public void Print(Document d)
    {
      
    }
  }
  
  public class Photocopier : IPrinter, IScanner
  {
    public void Print(Document d)
    {
      throw new NotImplementedException();
    }
  
    public void Scan(Document d)
    {
      throw new NotImplementedException();
    }
  }
  
  public interface IMultiFunctionDevice : IPrinter, IScanner //
  {
    
  }

  public class MultiFunctionMachine : IMultiFunctionDevice
  {
    // compose this out of several modules
    private readonly IPrinter printer;
    private readonly IScanner scanner;
  
    public MultiFunctionMachine(IPrinter printer, IScanner scanner)
    {
      this.printer = printer;
      this.scanner = scanner;
    }
  
    public void Print(Document d) => printer.Print(d);
    public void Scan(Document d) => scanner.Scan(d);
  }

  class MyClass
  {
    public static void Main(string[] args)
    {
      
    }
  }
}