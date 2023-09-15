namespace TicTacToeLerneinheit_2.Models
{
    internal class TicTacToe : Match
    {
        internal TicTacToe()
        {
            GameWinCondition = 3;
            GameFieldRows = 3;
            GameFieldCols = 3;
            GameField = new string[GameFieldRows, GameFieldCols];
            WinCondition = GameWinCondition;
        }

        internal override bool ProcessInput()
        {
            CalculatedRow = (PlayerInputToInt - 1) / GameFieldRows;
            CalculatedCol = (PlayerInputToInt - 1) % GameFieldCols;

            if (GameLength < PlayerInputToInt || PlayerInputToInt < 0 || PlayerInputToInt > GameLength || GameField[CalculatedRow, CalculatedCol] != null)
            {
                Console.Clear();
                DrawBoard();
                Console.WriteLine("Feld ist besetzt\n");
                return false;
            }
            else
            {
                GameField[CalculatedRow, CalculatedCol] = $"{CurrentPlayer.Sign}";
                return true;
            }

        }

    }
}
