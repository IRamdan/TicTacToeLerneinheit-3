using System;
using System.Data;
using System.Data.SqlClient;
using TicTacToeLerneinheit_2.Models;

namespace TicTacToeLerneinheit_2.Datenbank
{
    internal class Database
    {


        //GetID entfernen, try catch, ident statt id und connectiondatabase, timestamps hinzufügen insert and update, console write entfernen aus der methode und auslagern, AddWithValue, mit objekten arbeiten
        internal static string ConnectionString = "Data Source=zub-PC143\\SQLEXPRESS;" + "Initial Catalog=Lerneinheit3;" + "User ID=Peter;" + "Password=1234;";
        private static T ConvertField<T>(object field) => (field == null || field == DBNull.Value) ? default : (T)Convert.ChangeType(field, typeof(T));
        public static string SaveNickName()
        {
            Console.Write("Bitte geben Sie einen Nickname ein: ");
            string Nickname = Console.ReadLine();
            return Nickname;
        }
        public static string SavePassword()
        {
            Console.Write("Bitte geben Sie ein Passwort ein: ");
            string Password = WritePasswordAsStars();
            return Password;
        }
        internal static Player CreatePlayerAccount(string p_Nickname, string p_Password)
        {
            while (IsNicknameTaken(p_Nickname))
            {
                Console.Clear();
                Console.WriteLine("Nickname bereits vergeben. Bitte versuchen Sie es erneut.");
                p_Nickname = SaveNickName();
                p_Password = SavePassword();
            }

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand CreatePlayerAccount = new SqlCommand("CreatePlayerAccount", Connection))
                {
                    CreatePlayerAccount.CommandType = CommandType.StoredProcedure;
                    CreatePlayerAccount.Parameters.AddWithValue("@p_Nickname", p_Nickname);
                    CreatePlayerAccount.Parameters.AddWithValue("@p_Password", p_Password);

                    CreatePlayerAccount.ExecuteNonQuery();
                    Console.Clear();
                    Console.WriteLine("Konto erfolgreich erstellt.");
                    Player NewPlayerAccount = new Player()
                    {
                        Name = p_Nickname,
                    };
                    return NewPlayerAccount;
                }
            }
        }
        internal static bool IsNicknameTaken(string p_Nickname)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand CheckNickname = new SqlCommand("SELECT 1 FROM Player WHERE Nickname = @Nickname", Connection))
                {
                    CheckNickname.Parameters.AddWithValue("@Nickname", p_Nickname);
                    return CheckNickname.ExecuteScalar() != null;
                }
            }
        }
        internal static string GetPasswordByNickname(string p_Nickname)
        {
            string Password = null;

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand GetPassword = new SqlCommand("SELECT Password FROM Player WHERE Nickname = @Nickname", Connection))
                {
                    GetPassword.Parameters.AddWithValue("@Nickname", p_Nickname);

                    using (SqlDataReader Reader = GetPassword.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            Password = Reader["Password"].ToString();
                        }
                    }
                }
            }

            return Password;
        }
        private static string WritePasswordAsStars()
        {
            string Password = "";
            ConsoleKeyInfo Key;
            do
            {
                Key = Console.ReadKey(true);

                if (Key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (Key.Key == ConsoleKey.Backspace)
                {
                    if (Password.Length > 0)
                    {
                        Password = Password.Remove(Password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    Password += Key.KeyChar;
                    Console.Write("*");
                }
            } while (true);

            Console.WriteLine();

            return Password;
        }
        internal static void ShowLeaderBoard()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                string ShowLeaderboard = "SELECT * FROM [dbo].[Leaderboard]";

                using (SqlCommand ShowLeaderBoardCommand = new SqlCommand(ShowLeaderboard, Connection))
                {
                    SqlDataReader Reader = ShowLeaderBoardCommand.ExecuteReader();

                    string Seperator = "====================================";
                    Console.WriteLine(Seperator);
                    Game.PrintCenteredText("Leaderboard", Seperator);
                    Console.WriteLine(Seperator + "\n");
                    Console.WriteLine("  Platz  |  Benutzername |  Punkte  ");

                    int Place = 0;
                    while (Reader.Read())
                    {
                        string PlayerName = Reader["Nickname"].ToString();
                        int Points = Convert.ToInt32(Reader["Points"]);
                        Place++;

                        ConsoleColor Color = Place % 2 == 0 ? ConsoleColor.Yellow : ConsoleColor.Cyan;
                        Console.ForegroundColor = Color;
                        Console.WriteLine($"    {Place,-5}|     {PlayerName,-10}|   {Points,-11}");
                        Console.ResetColor();
                    }
                }
            }
        }
        internal static void ShowPlayerGameStats()
        {
            Console.Write("Geben Sie den Nicknamen des Spielers ein: ");
            string Nickname = Console.ReadLine();
            Console.Clear();

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                string SelectedPlayerGamestate = $"SELECT * FROM [dbo].[PlayerGameStats] WHERE Nickname like '{Nickname}'";

                using (SqlCommand ShowPlayerGameStatsCommand = new SqlCommand(SelectedPlayerGamestate, Connection))
                {
                    SqlDataReader Reader = ShowPlayerGameStatsCommand.ExecuteReader();

                    string Seperator = "===================================================================================================================";

                    if (Reader.HasRows)
                    {
                        Console.WriteLine(Seperator);
                        Game.PrintCenteredText("Spielerstatistiken", Seperator);
                        Console.WriteLine(Seperator);
                        Console.WriteLine("  Player Name |  Total Games  |  Total Wins  |  Total Losses  |  Unfinished Games  |  Total Draws  | SuccesRate");

                        while (Reader.Read())
                        {
                            int TotalGames = ConvertField<int>(Reader["TotalGames"]);
                            int TotalWins = ConvertField<int>(Reader["TotalWins"]);
                            int TotalLosses = ConvertField<int>(Reader["TotalLosses"]);
                            int TotalUnfinishedGames = ConvertField<int>(Reader["TotalUnfinishedGames"]);
                            int TotalDraws = ConvertField<int>(Reader["TotalDraws"]);
                            decimal SuccessRate = ConvertField<decimal>(Reader["SuccessRate"]);
                            ConsoleColor color = ConsoleColor.Cyan;
                            Console.ForegroundColor = color;
                            Console.WriteLine($" {Nickname,-11}  |  {TotalGames,-11}  |  {TotalWins,-10}  |  {TotalLosses,-12}  |  {TotalUnfinishedGames,-16}  |  {TotalDraws,-12} | {SuccessRate,-12} %");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Game.PrintCenteredText("Der Eingegebene Spieler wurde nicht gefunden.", Seperator);
                    }
                }
            }
        }
        internal static void CreateGameTableEntry(GameType p_GameType, int p_Rows, int p_Columns, int p_WinCondition)
        {
            string GameTypeString = p_GameType.ToString();

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand CreateGame = new SqlCommand("SaveGame", Connection))
                {
                    CreateGame.CommandType = CommandType.StoredProcedure;

                    CreateGame.Parameters.AddWithValue("@p_GameType", GameTypeString);
                    CreateGame.Parameters.AddWithValue("@p_Rows", p_Rows);
                    CreateGame.Parameters.AddWithValue("@p_Columns", p_Columns);
                    CreateGame.Parameters.AddWithValue("@p_WinCondition", p_WinCondition);
                    CreateGame.ExecuteNonQuery();

                }
            }
        }
        internal static int CreateMatchTableEntry(int p_GameID)
        {
            int MatchID = -1;

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand CreateMatch = new SqlCommand("CreateMatch", Connection))
                {
                    CreateMatch.CommandType = CommandType.StoredProcedure;
                    CreateMatch.Parameters.AddWithValue("@p_GameID", p_GameID);
                    CreateMatch.Parameters.AddWithValue("@p_Winner", "Noch keiner");
                    CreateMatch.Parameters.AddWithValue("@p_Gamestate", "Gestartet");

                    MatchID = Convert.ToInt32(CreateMatch.ExecuteScalar());

                }
            }
            return MatchID;
        }
        internal static Player LoginPlayerAccount(string p_Nickname = null, string p_Password = null)
        {
            Player Player = null;
            string Nickname = p_Nickname;
            string Password = p_Password;

            if (p_Nickname == null || p_Password == null)
            {
                Nickname = SaveNickName();
                Password = SavePassword();
            }

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand Login = new SqlCommand("LoginPlayerAccount", Connection))
                {
                    Login.CommandType = CommandType.StoredProcedure;
                    Login.Parameters.AddWithValue("@nickname", Nickname);
                    Login.Parameters.AddWithValue("@password", Password);

                    var Result = (int)Login.ExecuteScalar();
                    if (Result == 1)
                    {
                        Console.WriteLine("Anmeldung erfolgreich!");
                        Player = new Player() { PlayerIdent = GetIDByValue("Player", "NickName", Nickname), Name = Nickname, Sign = Game.SetPlayerSkin() };
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Anmeldung fehlgeschlagen. Überprüfen Sie Ihren Benutzernamen und Ihr Passwort.");
                    }
                }
            }

            return Player;
        }
        internal static void AddPlayerToPlayerMatch(List<Player> p_Players)
        {

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                foreach (Player Player in p_Players)
                {
                    using (SqlCommand AddPlayerToPlayerMatch = new SqlCommand("AddPlayerToPlayerMatch", Connection))
                    {
                        AddPlayerToPlayerMatch.CommandType = CommandType.StoredProcedure;

                        AddPlayerToPlayerMatch.Parameters.AddWithValue("@p_PlayerID", GetIDByValue("Player", "NickName", Player.Name));
                        AddPlayerToPlayerMatch.Parameters.AddWithValue("@p_MatchID", GetLastID("Match"));

                        AddPlayerToPlayerMatch.ExecuteNonQuery();
                    }
                }
            }
        }
        internal static void SaveMove(Player p_Player, int p_Row, int p_Col)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand SaveMove = new SqlCommand("SaveMove", Connection))
                {
                    SaveMove.CommandType = CommandType.StoredProcedure;

                    SaveMove.Parameters.AddWithValue("@p_PlayerMatchID", GetIDByValue("PlayerMatch", "PlayerID", p_Player.PlayerIdent));
                    SaveMove.Parameters.AddWithValue("@p_GameID", GetLastID("Game"));
                    SaveMove.Parameters.AddWithValue("@p_Row", p_Row);
                    SaveMove.Parameters.AddWithValue("@p_Col", p_Col);
                    SaveMove.Parameters.AddWithValue("@p_Sign", p_Player.Sign);

                    SaveMove.ExecuteNonQuery();
                }
            }
        }
        internal static void SaveMatch(Player p_Winner, GameState p_GameState)
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                using (SqlCommand SaveMatch = new SqlCommand("SaveMatch", Connection))
                {
                    SaveMatch.CommandType = CommandType.StoredProcedure;
                    SaveMatch.Parameters.AddWithValue("@p_MatchID", GetLastID("Match"));
                    SaveMatch.Parameters.AddWithValue("@p_GameID", GetLastID("Game"));

                    if (p_Winner != null)
                    {
                        SaveMatch.Parameters.AddWithValue("@p_Winner", p_Winner.PlayerIdent);
                    }
                    else
                    {
                        SaveMatch.Parameters.AddWithValue("@p_Winner", "Unentschieden");
                    }

                    string GamestateToString = Convert.ToString(p_GameState);
                    SaveMatch.Parameters.AddWithValue("@p_Gamestate", GamestateToString);
                    SaveMatch.Parameters.AddWithValue("@p_IsDraw", GamestateToString == "IsDraw" ? 1 : 0);
                    SaveMatch.Parameters.AddWithValue("@p_IsUnfinished", GamestateToString == "IsRunning" ? 1 : 0);

                    SaveMatch.ExecuteNonQuery();

                }
            }
        }
        internal static int GetIDByValue(string p_TableName, string p_ColumnName, object p_SearchValue)
        {
            if (p_SearchValue == null)
            {
                throw new ArgumentNullException(nameof(p_SearchValue));
            }

            int Id = -1;

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                string GetIdByValueQuery = $"SELECT MAX(Ident) FROM {p_TableName} WHERE {p_ColumnName} = @SearchValue";

                using (SqlCommand GetIdByValueCommand = new SqlCommand(GetIdByValueQuery, Connection))
                {
                    GetIdByValueCommand.Parameters.Add(new SqlParameter("@SearchValue", p_SearchValue));

                    var Result = GetIdByValueCommand.ExecuteScalar();
                    if (Result != DBNull.Value)
                    {
                        Id = Convert.ToInt32(Result);
                    }

                }
            }

            return Id;
        }
        internal static int GetLastID(string tableName)
        {
            int LastID = -1;

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();
                string Query = $"SELECT MAX(Ident) FROM {tableName}";

                using (SqlCommand SelectLastIdent = new SqlCommand(Query, Connection))
                {
                    var result = SelectLastIdent.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        LastID = Convert.ToInt32(result);
                    }
                }
            }
            return LastID;
        }
    }
}