
namespace TicTacToeLerneinheit_2.Models
{
    internal class Match
    {
        #region Vars
        internal List<Player> Players { get; set; }
        internal Player CurrentPlayer { get; set; }
        internal Player Winner { get; set; }
        internal string[,] GameField { get; set; }
        internal GameState GameStatus { get; set; }    
        internal int WinCondition { get; set; }
        internal int GameWinCondition;
        internal int PlayerInputToInt;
        internal int RoundCounter = 0;
        internal int GameFieldRows;
        internal int GameFieldCols;
        internal int GameLength;
        internal int CalculatedRow;
        internal int CalculatedCol;
        #endregion

        #region PlayerMethods
        internal Player GetStartingPlayer()
        {
            Random PlayerRandomizer = new();
            return Players[PlayerRandomizer.Next(Players.Count)];
        }
        internal void CurrentPlayerDeterminer()
        {
            if (CurrentPlayer == null)
            {
                CurrentPlayer = GetStartingPlayer();
            }
            else
            {
                int NextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;

                if (NextPlayerIndex > Players.Count - 1)
                    NextPlayerIndex = 0;
                CurrentPlayer = Players[NextPlayerIndex];
            }
        }
        #endregion

        #region InputProcessingMethods
        internal virtual bool GetInput()
        {
            GameFieldRows = GameField.GetLength(0);
            GameFieldCols = GameField.GetLength(1);
            GameLength = GameFieldRows * GameFieldCols;

            bool IsUserInputValid = false;
            do
            {
                Console.WriteLine($"Suche dir ein Spielfeld zwischen 1 und {GameLength} aus {CurrentPlayer.Name}({CurrentPlayer.Sign}):");
                CurrentPlayer.Input = Console.ReadLine();

                if (int.TryParse(CurrentPlayer.Input, out PlayerInputToInt))
                {
                    PlayerInputToInt = Convert.ToInt32(CurrentPlayer.Input);
                    if (PlayerInputToInt < 1 || PlayerInputToInt > GameLength)
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
                    Console.WriteLine("Bitte nur Zahlen im folgendem Gültigkeitsbereich auswählen:\n\n");
                }

            } while (!IsUserInputValid);
            Console.Clear();
            return IsUserInputValid;
        }
        internal virtual bool ProcessInput()
        {
            if (GameFieldCols < PlayerInputToInt || PlayerInputToInt <= 0 || PlayerInputToInt > GameLength)
            {
                return false;
            }
            else
            {
                int RowCounter = 0;
                bool IsEmpty = true;
                do
                {
                    CalculatedRow = GameFieldRows - RowCounter - 1;
                    CalculatedCol = (PlayerInputToInt - 1) % GameFieldCols;
                    if (CalculatedRow < 0)
                    {
                        Console.Clear();
                        DrawBoard();
                        Console.WriteLine("Diese Zeile ist voll!\n");
                        return false;
                    }
                    else if (GameField[CalculatedRow, CalculatedCol] == null)
                    {
                        GameField[CalculatedRow, CalculatedCol] = CurrentPlayer.Sign;
                        IsEmpty = false;
                    }
                    else
                    {
                        RowCounter++;
                    }
                } while (IsEmpty && RowCounter < GameLength);
                return true;
            }
        }
        internal virtual void SelectionOutOfBoundaries()
        {
            Console.Clear();
            DrawBoard();
            Console.WriteLine($"Gewünschtes Feld ist nicht im Gültigkeitsbereich\n");
        }
        #endregion

        #region GameStateMethods
        internal GameState CheckGameState()
        {
            if (CheckForWin())
            {
                Winner = CurrentPlayer;
                Console.WriteLine($"{CurrentPlayer.Name} hat gewonnen");
                return GameState.HasWinner;
            }

            if (RoundCounter == GameLength - 1)
            {
                Console.WriteLine("Oh nein!\n\nDas Spiel endet unentschieden.\n");
                return GameState.IsDraw;
            }
            RoundCounter++;
            return GameState.IsRunning;
        }
        internal bool CheckForDraw()
        {
            if (RoundCounter >= GameLength - 1)
            {
                Console.WriteLine("Maximale Spielzüge erreicht");
                return false;
            }
            else
            {
                RoundCounter++;
                return true;
            }
        }
        internal bool CheckForWin()
        {
            if (CheckHorizontal())
                return true;
            if (CheckVertical())
                return true;
            if (CheckDiagonal())
                return true;
            if (CheckCounterDiagonal())
                return true;

            return false;
        }
        internal bool CheckHorizontal()
        {
            int WinConditionCounter = 0;
            for (int CheckCounter = Math.Max(0, CalculatedCol - WinCondition + 1); CheckCounter <= Math.Min(GameFieldCols - 1, CalculatedCol + WinCondition - 1); CheckCounter++)
            {
                if (GameField[CalculatedRow, CheckCounter] == CurrentPlayer.Sign)
                {
                    WinConditionCounter++;
                    if (WinConditionCounter == WinCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    WinConditionCounter = 0;
                }
            }
            return false;
        }
        internal bool CheckVertical()
        {
            int WinConditionCounter = 0;
            for (int CheckCounter = Math.Max(0, CalculatedRow - WinCondition + 1); CheckCounter <= Math.Min(GameFieldRows - 1, CalculatedRow + WinCondition - 1); CheckCounter++)
            {
                if (GameField[CheckCounter, CalculatedCol] == CurrentPlayer.Sign)
                {
                    WinConditionCounter++;
                    if (WinConditionCounter == WinCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    WinConditionCounter = 0;
                }
            }
            return false;
        }
        internal bool CheckDiagonal()
        {
            int WinConditionCounter = 0;
            int RowCheckCounter = CalculatedRow;
            int ColCheckCounter = CalculatedCol;

            while (RowCheckCounter > 0 && ColCheckCounter > 0)
            {
                RowCheckCounter--;
                ColCheckCounter--;
            }

            while (RowCheckCounter < GameFieldRows && ColCheckCounter < GameFieldCols)
            {
                if (GameField[RowCheckCounter, ColCheckCounter] == CurrentPlayer.Sign)
                {
                    WinConditionCounter++;
                    if (WinConditionCounter == WinCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    WinConditionCounter = 0;
                }

                RowCheckCounter++;
                ColCheckCounter++;
            }
            return false;
        }
        internal bool CheckCounterDiagonal()
        {
            int WinConditionCounter = 0;
            int RowCheckCounter = CalculatedRow;
            int ColCheckCounter = CalculatedCol;

            while (RowCheckCounter < GameFieldRows - 1 && ColCheckCounter > 0)
            {
                RowCheckCounter++;
                ColCheckCounter--;
            }

            while (RowCheckCounter >= 0 && ColCheckCounter < GameFieldCols)
            {
                if (GameField[RowCheckCounter, ColCheckCounter] == CurrentPlayer.Sign)
                {
                    WinConditionCounter++;
                    if (WinConditionCounter == WinCondition)
                    {
                        return true;
                    }
                }
                else
                {
                    WinConditionCounter = 0;
                }

                RowCheckCounter--;
                ColCheckCounter++;
            }
            return false;
        }
        #endregion
        internal virtual void DrawBoard()
        {
            for (int RowIndex = 0; RowIndex < GameField.GetLength(0); RowIndex++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new string('-', GameField.GetLength(1) * 6));
                for (int ColumnIndex = 0; ColumnIndex < GameField.GetLength(1); ColumnIndex++)
                {
                    if (GameField[RowIndex, ColumnIndex] == null)
                    {
                        Console.Write("|     ");
                    }
                    else
                    {
                        Console.Write($"| {GameField[RowIndex, ColumnIndex]}  ");
                    }
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', GameField.GetLength(1) * 6));
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}