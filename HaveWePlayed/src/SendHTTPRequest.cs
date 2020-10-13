﻿using Newtonsoft.Json;
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

        public async Task<List<string>> GetDataList(string api_key, string summonername, string server, int searchGames)
        {
            string accountId = GetAccountId(api_key, summonername, server).Result;

            HttpClient client = new HttpClient();

            List<string> MatchIds = new List<string>();

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

            } while (searchGames > beginIndex);

            return MatchIds;
        }
    }
}
