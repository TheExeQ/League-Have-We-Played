using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HaveWePlayed.src
{
    class SendHTTPRequest
    {
        public async Task<string> GetAccountId(string api_key, string summonername, string server)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://{server}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonername}"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("X-Riot-Token", api_key);

                HttpResponseMessage response = await client.SendAsync(request);

                var result = JsonConvert.DeserializeObject<Data.SummonerRoot>(response.Content.ReadAsStringAsync().Result);

                return result.accountId;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Something went wrong\n" +
                    "--------------\n" +
                    e);
                Console.WriteLine("--------------");
                Console.WriteLine("Program closing in 5 seconds.");

                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
                throw;
            }
        }

        public async Task<List<string>> GetDataList(string api_key, string summonername, string server, int searchGames)
        {
            try
            {
                string accountId = GetAccountId(api_key, summonername, server).Result;

                HttpClient client = new HttpClient();

                List<string> MatchIds = new List<string>();

                int gamesPlayed = 0;

                int beginIndex = 0;
                int endIndex = 100;

                if (searchGames < 100)
                {
                    endIndex = searchGames;
                }

                do
                {
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri($"https://{server}.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}?endIndex={ endIndex }&beginIndex={ beginIndex }"),
                        Method = HttpMethod.Get
                    };

                    request.Headers.Add("X-Riot-Token", api_key);

                    HttpResponseMessage response = await client.SendAsync(request);

                    var result = JsonConvert.DeserializeObject<Data.MatchRoot>(response.Content.ReadAsStringAsync().Result);

                    foreach (var match in result.matches)
                    {
                        MatchIds.Add(match.gameId);
                    }

                    beginIndex += 100;
                    endIndex = beginIndex + 100;

                    if (searchGames >= 2000)
                    {
                        System.Threading.Thread.Sleep(50);
                    }

                    gamesPlayed = result.totalGames;

                } while (searchGames > beginIndex);

                Console.OutputEncoding = System.Text.Encoding.Unicode;

                Console.WriteLine("-------------");
                Console.WriteLine($"{summonername}'s total games played: " + gamesPlayed);
                Console.WriteLine("-------------");

                Console.OutputEncoding = System.Text.Encoding.ASCII;

                return MatchIds;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Something went wrong\n" +
                    "--------------\n" +
                    e);
                Console.WriteLine("--------------");
                Console.WriteLine("Program closing in 5 seconds.");

                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
                throw;
            }
        }
    }
}
