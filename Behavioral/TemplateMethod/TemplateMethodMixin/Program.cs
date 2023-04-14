namespace DotNetDesignPatterns.Behavioral.TemplateMethod.TemplateMethodMixin;

using DotNetDesignPatterns.Behavioral.TemplateMethod.TemplateMethodMixin;
using static System.Console;

interface IGame
{
  void Run()
  {
    Start();
    while (!HaveWinner)
      TakeTurn();
    WriteLine($"Player {WinningPlayer} wins.");
  }

  void Start()
  {
  }

  bool HaveWinner { get; }
  void TakeTurn();
  int WinningPlayer { get; }
}

public class Chess : IGame
{
  public Chess()
  {
    numberOfPlayers = 2;
  }

  public void Start()
  {
    WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
  }

  public bool HaveWinner => turn == maxTurns;

  public void TakeTurn()
  {
    WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
    currentPlayer = (currentPlayer + 1) % numberOfPlayers;
  }

  public int WinningPlayer => currentPlayer;
  private int currentPlayer = 0;
  private int maxTurns = 10;
  private int turn = 1;
  private readonly int numberOfPlayers;
}

public class Program
{
  public static void Main(string[] args)
  {
    IGame chess = new Chess();
    chess.Run();
  }
}