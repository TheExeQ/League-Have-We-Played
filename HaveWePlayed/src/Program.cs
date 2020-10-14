using HaveWePlayed.src;
using System;
using System.Diagnostics;

namespace HaveWePlayed
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            Console.ReadLine();
        }

        static async void Run()
        {
            bool debug = false;

            string api_key;
            string server;
            string playerOne;
            string playerTwo;

            // Ignore this, this is just so i can debug directly from code.
            if (debug)
            {
                api_key = "";
                server = "";
                playerOne = "";
                playerTwo = "";
                goto debug;
            }

            Console.WriteLine("Input API Key, can be found on this site: https://developer.riotgames.com/");
            api_key = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("[ 1 ] NA");
            Console.WriteLine("[ 2 ] EUW");
            Console.WriteLine("[ 3 ] EUNE");

            server = Console.ReadLine();

            switch (server)
            {
                default:
                    break;

                case "1":
                    server = "NA1";
                    break;

                case "2":
                    server = "EUW1";
                    break;

                case "3":
                    server = "EUN1";
                    break;
            }

            Console.InputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine("input Player 1");
            playerOne = Console.ReadLine();
            Console.WriteLine("input Player 2");
            playerTwo = Console.ReadLine();

            Console.InputEncoding = System.Text.Encoding.ASCII;

            debug:

            Console.WriteLine("NOTE: can only search 2 000 games per sec or 10 000 games every 2 min");
            Console.WriteLine("input Player 1 Range (Search for x recent games)");
            int playerOneSearchRange = Int32.Parse(Console.ReadLine());
            Console.WriteLine("input Player 2 Range (Search for x recent games)");
            int playerTwoSearchRange = Int32.Parse(Console.ReadLine());

            SendHTTPRequest request = new SendHTTPRequest();
            var player1Matches = await request.GetDataList(api_key, playerOne, server, playerOneSearchRange);
            var player2Matches = await request.GetDataList(api_key, playerTwo, server, playerTwoSearchRange);

            int amountOfGames = 0;

            Console.WriteLine("-------------");
            Console.WriteLine("List of MatchIDS");
            Console.WriteLine("-------------");

            if (player1Matches.Count > player2Matches.Count)
            {
                for (int i = 0; i < player1Matches.Count; i++)
                {
                    for (int j = 0; j < player2Matches.Count; j++)
                    {
                        if (player1Matches[i] == player2Matches[j])
                        {
                            Console.WriteLine($"https://blitz.gg/lol/match/{ server }/{ playerOne }/{ player1Matches[i] }");
                            amountOfGames++;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < player2Matches.Count; i++)
                {
                    for (int j = 0; j < player1Matches.Count; j++)
                    {
                        if (player2Matches[i] == player1Matches[j])
                        {
                            Console.WriteLine($"https://blitz.gg/lol/match/{ server }/{ playerTwo }/{ player2Matches[i] }");
                            amountOfGames++;
                        }
                    }
                }
            }

            Console.WriteLine("-------------");
            Console.WriteLine("Successfully Searched: " + player1Matches.Count + " Matches from Player 1");
            Console.WriteLine("Successfully Searched: " + player2Matches.Count + " Matches from Player 2");
            Console.WriteLine("Total Games found: " + amountOfGames);
            Console.WriteLine("-------------");
        }
    }
}
