using System.Security.AccessControl;
using TicTacToeLerneinheit_2.Datenbank;

namespace TicTacToeLerneinheit_2.Models
{
    internal class Game
    {
        #region Vars
        Database Database = new Database();
        internal int NumberOfPlayers = 2;
        internal int GameWinCondition;
        public static List<string> Skins = new() { "🐶", "🐱", "🐭", "🐹", "🐰", "🐻", "🐼", "🐨", "🐯" };
        internal List<Player> Players { get; set; }
        internal List<Match> History = new();
        internal Match CurrentMatch = null;
        internal GameType GameSelection = new();
        internal GameType CurrentType { get; set; }
        internal string[,] GameField { get; set; }
        int GameID { get; set; }
        #endregion
        #region PlayerInitMethods
        internal void MainMenu()
        {
            bool GameStarted = false;
            do
            {
                Console.Clear();
                MainMenuHeader();

                int PlayerSelection;
                if (int.TryParse(Console.ReadLine(), out PlayerSelection))
                {
                    switch (PlayerSelection)
                    {
                        case 1:
                            Console.Clear();
                            InitGame();
                            InitPlayer();
                            GameStarted = true;
                            break;
                        case 2:
                            Console.Clear();
                            Database.ShowLeaderBoard();
                            ReturnToMainMenu("====================================");
                            break;
                        case 3:
                            Console.Clear();
                            Database.ShowPlayerGameStats();
                            ReturnToMainMenu("===================================================================================================================");
                            break;
                        default:
                            Console.WriteLine("Ungültige Auswahl. Bitte eine gültige Option wählen.");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Auswahl. Bitte eine gültige Option wählen.");
                    Console.ReadKey();
                }

            } while (!GameStarted);
        }
        internal void ReturnToMainMenu(string p_Seperator)
        {
            ReturnToMainMenuHeader(p_Seperator);
            Console.ReadKey();
            Console.Clear();
        }
        internal static string SetPlayerSkin()
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
        internal static string ChooseSkin(string p_UserInput)
        {
            bool IsInteger = int.TryParse(p_UserInput, out int Index);

            if (IsInteger && Index > 0 && Index <= Game.Skins.Count)
            {
                return Game.Skins[Index - 1];
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Falsche Eingabe, bitte versuchen Sie es erneut.");
                ShowSkins();
                return null;
            }
        }
        internal static void ShowSkins()
        {
            Console.WriteLine("Wählen Sie einen Skin aus:");
            for (int SkinIndex = 0; SkinIndex < Game.Skins.Count; SkinIndex++)
            {
                Console.WriteLine($"{SkinIndex + 1}: {Skins[SkinIndex]}");
            }
        }
        internal void InitPlayer()
        {
            Players = new List<Player>();
            do
            {
                string hasAccount = "";
                while (hasAccount.ToLower() != "j" && hasAccount.ToLower() != "n")
                {
                    string Seperator = "==========================================";
                    Console.Clear();
                    Console.WriteLine(Seperator);
                    Game.PrintCenteredText("Spieler-Anmeldung", Seperator);
                    Console.WriteLine(Seperator);
                    Game.PrintCenteredText("Haben Sie bereits ein Konto? (j/n)", Seperator);
                    hasAccount = Console.ReadLine();
                    if (hasAccount.ToLower() != "j" && hasAccount.ToLower() != "n")
                    {
                        Console.Clear();
                        Console.WriteLine("Falsche Auswahl, versuche es erneut!\n");
                    }
                }

                if (hasAccount.ToLower() == "j")
                {
                    Player ExistingPlayer = Database.LoginPlayerAccount();

                    if (ExistingPlayer != null)
                    {
                        Players.Add(ExistingPlayer);
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Es konnte kein Konto gefunden werden. Bitte versuchen Sie es erneut.");
                    }
                }
                else if (hasAccount.ToLower() == "n")
                {
                    Console.WriteLine("Gebe bitte deinen neuen Benutzernamen ein!");
                    string NickName = Console.ReadLine();
                    Console.WriteLine("Gebe bitte dein neues Passwort ein!");
                    string Password = Console.ReadLine();
                    Player NewPlayer = Database.CreatePlayerAccount(NickName, Password);
                    NewPlayer = Database.LoginPlayerAccount(NewPlayer.Name, Database.GetPasswordByNickname(NewPlayer.Name));
                    Players.Add(NewPlayer);
                }
            } while (Players.Count < NumberOfPlayers);
        }
        #endregion
        #region InitGameMethods
        internal void InitGame()
        {
            InitGameHeader();
            ChooseGame();
            InitGameFooter();
        }
        internal void MainMenuHeader()
        {
            string Seperator = "===================================";
            Console.WriteLine(Seperator);
            PrintCenteredText("Hauptmenü", Seperator);
            Console.WriteLine(Seperator);
            Console.WriteLine("\n" + @"         1. Neues Spiel            " + "\n");
            Console.WriteLine(@"         2. Leaderboard            " + "\n");
            Console.WriteLine(@"       3. Spielerstatistik         ");
            Console.Write("\n\n   Bitte wählen Sie eine Option: ");
        }
        internal void InitGameHeader()
        {

            string Seperator = "===================================";
            Console.WriteLine(Seperator);
            PrintCenteredText("Spielauswahl", Seperator);
            Console.WriteLine(Seperator);
            Console.WriteLine("\n");
            PrintCenteredText("1. TicTacToe", Seperator);
            Console.WriteLine("\n");
            PrintCenteredText("2. Vier Gewinnt", Seperator);
            Console.WriteLine("\n");
            PrintCenteredText("Bitte wählen Sie ein Spiel aus", Seperator);
        }
        internal void ReturnToMainMenuHeader(string p_Seperator)
        {
            Console.WriteLine(p_Seperator);
            PrintCenteredText("Drücken Sie eine beliebige Taste,", p_Seperator);
            PrintCenteredText("um zum Hauptmenü zurückzukehren.", p_Seperator);
            Console.WriteLine(p_Seperator);
        }
        internal static void PrintCenteredText(string p_Text, string p_StringToGetLength)
        {
            int Padding = (p_StringToGetLength.Length - p_Text.Length) / 2;
            string CenteredText = new string(' ', Padding) + p_Text;
            Console.WriteLine(CenteredText);
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
            string Seperator = "===================================";
            Console.WriteLine(Seperator);
            PrintCenteredText("Spielstatistiken", Seperator);
            Console.WriteLine(Seperator);
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
            Console.WriteLine(Seperator + "\n\n");
            PrintCenteredText("Zum beenden beliebige Taste drücken", Seperator);
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
