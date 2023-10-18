using TicTacToeLerneinheit_2.Datenbank;
using TicTacToeLerneinheit_2.Models;

namespace TicTacToeLerneinheit_2
{
    internal class Program
    {
        internal static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game CurrentGame = new();
            Match CurrentMatch;
            CurrentGame.MainMenu();
            CurrentMatch = CurrentGame.CreateMatch();
            Database.CreateGameTableEntry(CurrentGame.GameSelection, CurrentMatch.GameFieldRows, CurrentMatch.GameFieldCols, CurrentMatch.WinCondition);
            
            do
            {
                //Ident direkt ohne getlastID Methode
                Database.CreateMatchTableEntry(Database.GetLastID("Game"));
                Database.AddPlayerToPlayerMatch(CurrentGame.Players);
                CurrentMatch.GetStartingPlayer();
                do
                {
                    CurrentMatch.DrawBoard();
                    CurrentMatch.CurrentPlayerDeterminer();
                    CurrentMatch.GetInput();
                    CurrentMatch.GameStatus = CurrentMatch.CheckGameState();
                    Database.SaveMove(CurrentMatch.CurrentPlayer, CurrentMatch.CalculatedRow, CurrentMatch.CalculatedCol);
                } while (CurrentMatch.GameStatus == GameState.IsRunning);
                Database.SaveMatch(CurrentMatch.Winner, CurrentMatch.GameStatus);
                CurrentMatch = CurrentGame.CreateMatch();
            }
            while (Game.Replay());
            Game.ShowStatistics(CurrentGame.History, CurrentGame.Players);
        }
    }
}