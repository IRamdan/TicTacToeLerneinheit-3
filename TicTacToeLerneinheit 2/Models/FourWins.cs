namespace TicTacToeLerneinheit_2.Models
{
    internal class FourWins : Match
    {
        internal FourWins()
        {
            GameFieldRows = 6;
            GameFieldCols = 7;
            WinCondition = 4;
            GameField = new string[GameFieldRows, GameFieldCols];
        }
        internal override void DrawBoard()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int HeaderIndex = 0; HeaderIndex < GameField.GetLength(1); HeaderIndex++)
            {
                Console.Write("   " + (HeaderIndex + 1) + "   ");
            }
            Console.WriteLine();

            for (int RowIndex = 0; RowIndex < GameField.GetLength(0); RowIndex++)
            {
                Console.WriteLine(new string('-', GameField.GetLength(1) * 7));
                for (int ColsIndex = 0; ColsIndex < GameField.GetLength(1); ColsIndex++)
                {
                    Console.Write("|");
                    if (GameField[RowIndex, ColsIndex] == null)
                    {
                        Console.Write("     ");
                    }
                    else
                    {
                        Console.Write($" {GameField[RowIndex, ColsIndex]}  ");
                    }
                    if (ColsIndex != GameField.GetLength(1) - 1 || ColsIndex == 1)
                    {
                        Console.Write("|");
                    }
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', GameField.GetLength(1) * 7));
            Console.ForegroundColor = ConsoleColor.White;
        }
        internal override bool GetInput()
        {
            GameFieldRows = GameField.GetLength(0);
            GameFieldCols = GameField.GetLength(1);
            GameLength = GameFieldRows * GameFieldCols;

            bool IsUserInputValid = false;
            do
            {
                Console.WriteLine($"Suche dir ein SpielFeld zwischen 1 und {GameFieldCols} aus {CurrentPlayer.Name}({CurrentPlayer.Sign}):");
                CurrentPlayer.Input = Console.ReadLine();

                if (int.TryParse(CurrentPlayer.Input, out PlayerInputToInt))
                {
                    PlayerInputToInt = Convert.ToInt32(CurrentPlayer.Input);
                    if (GameFieldCols < PlayerInputToInt || PlayerInputToInt <= 0 || PlayerInputToInt > GameLength)
                    {
                        SelectionOutOfBoundaries();
                    }
                    else
                    {
                        IsUserInputValid = ProcessInput();
                    }
                }
                else
                {
                    Console.Clear();
                    DrawBoard();
                    Console.WriteLine("Fehlerhafter Input\n");
                }

            } while (!IsUserInputValid);
            Console.Clear();
            return IsUserInputValid;
        }
        internal override void SelectionOutOfBoundaries()
        {
            Console.Clear();
            DrawBoard();
            Console.WriteLine($"Gewünschte Spalte ist nicht im Gültigkeitsbereich\n");
        }
    }
}
