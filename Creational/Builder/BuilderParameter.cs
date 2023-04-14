using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Creational.Builder.BuilderParameter;

public class MailService
{
  public class EmailBuilder
  {
    private class Email
    {
      public string From, To, Subject, Body;
    }
      
    private readonly Email email;

    private EmailBuilder(Email email) => this.email = email;

    public EmailBuilder From(string from)
    {
      email.From = from;
      return this;
    }

    public EmailBuilder To(string to)
    {
      email.To = to;
      return this;
    }

    public EmailBuilder Subject(string subject)
    {
      email.Subject = subject;
      return this;
    }
   
    public EmailBuilder Body(string body)
    {
      email.Body = body;
      return this;
    }

    private static void SendEmailInternal(Email email)
    {
      Console.WriteLine($"From: {email.From}");
      Console.WriteLine($"To: {email.To}");
      if (!string.IsNullOrWhiteSpace(email.Subject))
        Console.WriteLine($"Subject: {email.Subject}");
      Console.WriteLine(email.Body);
    }

    public static void SendEmail(Action<EmailBuilder> builder)
    {
      var email = new Email();
      builder(new EmailBuilder(email));
      SendEmailInternal(email);
    }
  }

  public void SendEmail(Action<EmailBuilder> builder)
  {
    EmailBuilder.SendEmail(builder);
  }
}

public class Program
{
  public static void Main(string[] args)
  {
    var ms = new MailService();
    ms.SendEmail(email => email.From("foo@bar.com")
      .To("bar@baz.com")
      .Body("Hello, how are you?"));
  }
}