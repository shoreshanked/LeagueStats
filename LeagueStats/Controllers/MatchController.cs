using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace LeagueStats
{
    class MatchController
    {
        static public void getSummonerMatches(List<String> summonerPuuidList, string apiToken, RestClient clientRegion, List<string> matchIdList, IDictionary<string, List<string>> matchIdDictionary)
        {
            foreach (var summoner in summonerPuuidList)
            {
                var getMatchesViaPuuid = new RestRequest("https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/" + summoner + "/ids?queue=450&start=0&count=10", Method.Get);
                getMatchesViaPuuid.AddHeader("X-Riot-Token", apiToken);

                var response = clientRegion.GetAsync(getMatchesViaPuuid).Result;

                matchIdList = response.Content.Replace("\"", "").Trim('[', ']').Split(',').ToList();
                matchIdDictionary.Add(summoner, matchIdList);
            }
         }

        static public void getMatchData(IDictionary<string, List<string>> matchIdDictionary, string apiToken, RestClient clientRegion, List<MatchDataModel> matchDataList)
        {
            foreach (var puuid in matchIdDictionary)
            {
                foreach (var matchID in puuid.Value)
                {
                    var getMatchDataViaMatchID = new RestRequest("https://europe.api.riotgames.com/lol/match/v5/matches/" + matchID, Method.Get);
                    getMatchDataViaMatchID.AddHeader("X-Riot-Token", apiToken);

                    var matchdata = clientRegion.ExecuteGetAsync(getMatchDataViaMatchID).Result;
                    MatchDataModel data = new MatchDataModel();

                    data = JsonConvert.DeserializeObject<MatchDataModel>(matchdata.Content);
                    matchDataList.Add(data);
                }
            }
        }
    }
}
