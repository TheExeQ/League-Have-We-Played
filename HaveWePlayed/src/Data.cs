using System.Collections.Generic;

namespace HaveWePlayed
{
    public class Data
    {
        public class MatchRoot
        {
            public List<Match> matches { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public int totalGames { get; set; }
        }
        public class Match
        {
            public string platformId { get; set; }
            public string gameId { get; set; }
            public int champion { get; set; }
            public int queue { get; set; }
            public int season { get; set; }
            public string timestamp { get; set; }
            public string role { get; set; }
            public string lane { get; set; }
        }

        public class SummonerRoot
        {
            public string id { get; set; }
            public string accountId { get; set; }
            public string puuid { get; set; }
            public string name { get; set; }
            public int profileIconId { get; set; }
            public long revisionDate { get; set; }
            public int summonerLevel { get; set; }
        }
    }
}
