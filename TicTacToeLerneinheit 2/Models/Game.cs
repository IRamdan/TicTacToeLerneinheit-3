namespace TicTacToeLerneinheit_2.Models
{

    internal class Game
    {
        #region Vars
        internal int NumberOfPlayers = 2;
        internal int GameWinCondition;
        internal List<string> Skins = new() { "🐶", "🐱", "🐭", "🐹", "🐰", "🐻", "🐼", "🐨", "🐯" };
        internal List<Player> Players { get; set; }
        internal List<Match> History = new();
        internal Match CurrentMatch = null;
        internal GameType GameSelection = new();
        internal GameType CurrentType { get; set; }
        internal string[,] GameField { get; set; }
        #endregion

        #region PlayerInitMethods
        internal void InitPlayer()
        {
            Players = new List<Player>();
            int PlayerNumber = 1;
            do
            {
                Console.Write($"Spieler {PlayerNumber}: ");
                Players.Add(new Player() { Name = Console.ReadLine(), Sign = SetPlayerSkin() });
                PlayerNumber++;
                Console.Clear();
            } while (Players.Count < NumberOfPlayers);
        }
        internal string SetPlayerSkin()
        {

            ShowSkins();

            string PlayerSkin = null;
            while (PlayerSkin == null)
            {
                string UserInput = Console.ReadLine();
                PlayerSkin = ChooseSkin(UserInput);
            }

            return PlayerSkin;
        }
        internal string ChooseSkin(string p_UserInput)
        {
            bool IsInteger = int.TryParse(p_UserInput, out int Index);

            if (IsInteger && Index > 0 && Index <= Skins.Count)
            {
                return Skins[Index - 1];
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Falsche Eingabe, bitte versuchen Sie es erneut.");
                ShowSkins();
                return null;
            }
        }
        internal void ShowSkins()
        {
            Console.WriteLine("Wählen Sie einen Skin aus:");
            for (int SkinIndex = 0; SkinIndex < Skins.Count; SkinIndex++)
            {
                Console.WriteLine($"{SkinIndex + 1}: {Skins[SkinIndex]}");
            }
        }
        #endregion

        #region InitGameMethods
        internal void InitGame()
        {
            InitGameHeader();
            ChooseGame();
            InitGameFooter();
        }
        internal void InitGameHeader()
        {
            Console.WriteLine("===================================\n           SPIELAUSWAHL            \n===================================\n");
            Console.WriteLine(@"          1. TicTacToe            " + "\n");
            Console.WriteLine(@"        2. Vier Gewinnt           ");
            Console.WriteLine("\n   Bitte wählen Sie ein Spiel aus");
        }
        internal void InitGameFooter()
        {

            Console.WriteLine("==============================================");
            Console.WriteLine($@"Sie haben sich für {CurrentType} entschieden
                ");
            Console.WriteLine("Das Spiel wird vorbereitet....oder so");
            Console.WriteLine("==============================================\n");
            Console.WriteLine("Bitte beliebige Taste zum fortfahren drücken");
            Console.ReadKey();
            Console.Clear();
        }
        internal void ChooseGame()
        {
            bool IsGameChoosen = false;
            string UserInput = Console.ReadLine();

            do
            {
                if (UserInput != "1" && UserInput != "2")
                {
                    Console.Clear();
                    InitGameHeader();
                    Console.WriteLine("\n\nUngültige Eingabe. Bitte versuche es erneut.");
                    UserInput = Console.ReadLine();
                }
                else if (UserInput == "1")
                {
                    GameSelection = GameType.TicTacToe;
                    IsGameChoosen = true;
                }
                else if (UserInput == "2")
                {
                    GameSelection = GameType.FourWins;
                    IsGameChoosen = true;
                }
            }
            while (!IsGameChoosen);
            Console.Clear();
        }
        internal static void ShowStatistics(List<Match> p_History, List<Player> p_PlayerList)
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("         SPIELSTATISTIKEN          ");
            Console.WriteLine("===================================\n");
            Console.ResetColor();

            foreach (var Player in p_PlayerList)
            {
                int Wins = p_History.Count(Match => Match.Winner == Player);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0,-15} {1,15}", Player.Name, Wins + " Siege \n");
                Console.ResetColor();
            }

            int Draws = p_History.Count(Match => Match.Winner == null);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,-15} {1,13}", "Unentschieden", Draws + "\n");
            Console.ResetColor();
            Console.WriteLine("===================================\n\nZum beenden beliebige Taste drücken");
            Console.ReadKey();
            Environment.Exit(0);
        }
        #endregion

        #region MatchMakingMethods
        internal Match CreateMatch()
        {
            if (CurrentMatch != null)
            {
                History.Add(CurrentMatch);
            }
            if (GameSelection == GameType.TicTacToe)
            {
                CurrentMatch = new TicTacToe() { Players = Players };
            }
            else if (GameSelection == GameType.FourWins)
            {
                CurrentMatch = new FourWins() { Players = Players };
            }

            return CurrentMatch;
        }
        internal static bool Replay()
        {
            Console.WriteLine("\nEnter für eine neue Runde\nEsc zum Anzeigen der Spielstatistik.");

            while (true)
            {
                var Key = Console.ReadKey(true).Key;

                switch (Key)
                {
                    case ConsoleKey.Enter:
                        Console.Clear();
                        return true;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine("Ungültige Eingabe!\nDrücke Enter für eine neue Runde oder Esc zum Anzeigen der Spielstatistik.");
                        break;
                }
            }
        }
        #endregion
    }
}
