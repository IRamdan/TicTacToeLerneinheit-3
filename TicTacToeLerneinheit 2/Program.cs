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
            CurrentGame.InitGame();
            CurrentGame.InitPlayer();
            /*CurrentMatch = CurrentGame.CreateMatch()*/

            do
            {
                CurrentMatch = CurrentGame.CreateMatch();

                CurrentMatch.GetStartingPlayer();
                do
                {
                    CurrentMatch.DrawBoard();
                    CurrentMatch.CurrentPlayerDeterminer();
                    CurrentMatch.GetInput();
                } while (CurrentMatch.CheckGameState() == GameState.IsRunning);
            }
            while (Game.Replay());
            Game.ShowStatistics(CurrentGame.History, CurrentGame.Players);
        }
    }
}