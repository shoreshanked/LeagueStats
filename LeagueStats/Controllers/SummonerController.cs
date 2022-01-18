using System;
using System.Collections.Generic;
using RestSharp;

namespace LeagueStats
{
    class SummonerController
    {

        static public SummonerModel getSummonerPuuid(List<String> summonerList, string apiToken, RestClient client)
        {
            SummonerModel response = new SummonerModel();

            foreach (var item in summonerList)
            {
                var getSummoner = new RestRequest("/lol/summoner/v4/summoners/by-name/" + item, Method.Get);
                getSummoner.AddHeader("X-Riot-Token", apiToken);

                try{response = client.GetAsync<SummonerModel>(getSummoner).Result;}
                catch{Console.WriteLine("\nUsername does not exist");}
            }
            return response;
        }

    }
}
