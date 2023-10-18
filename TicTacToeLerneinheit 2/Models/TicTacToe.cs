namespace TicTacToeLerneinheit_2.Models
{
    internal class TicTacToe : Match
    {
        internal TicTacToe()
        {
            GameFieldRows = 3;
            GameFieldCols = 3;
            WinCondition = 3;
            GameField = new string[GameFieldRows, GameFieldCols];
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
