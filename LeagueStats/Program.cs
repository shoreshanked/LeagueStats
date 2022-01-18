using System;
using System.Collections.Generic;
using RestSharp;


namespace LeagueStats
{

    public static class ListExtenstions
    {
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string filePath = "C:\\Users\\Willi\\OneDrive\\Desktop\\Code Repository\\LeagueStats\\LeagueStats\\SaveState";

            var summonerList = new List<string>();
            var summonerPuuidList = new List<string>();
            var matchIdList = new List<string>();
            var matchDataList = new List<MatchDataModel>();
            IDictionary<string, List<string>> matchIdDictionary = new Dictionary<string, List<string>>();

            var client = new RestClient("https://euw1.api.riotgames.com");
            var clientRegion = new RestClient("https://europe.api.riotgames.com");
            var apiToken = "RGAPI-0872b866-35f8-4c14-b148-6d0cd2c09d51";
            var userSpecified = false;

            var savedDataJson = Jobs.loadMatchData<MatchDataModel>(filePath);
            var savedSummonerDataJson = Jobs.loadSummonerData<List<string>>(filePath);

            var input = "";
            if (savedDataJson != null)
            {
                Console.WriteLine("Would you like to Load the previous summoner data? \nPress Y to load data");
                input = Console.ReadLine();

                if(input.ToLower() == "y")
                {
                    matchDataList = savedDataJson;
                    summonerList = savedSummonerDataJson;
                }

            }

            if (savedDataJson is null || input.ToLower() != "y")
            {
                do
                {

                    Console.WriteLine("\nEnter Summoner name");
                    var summonerName = Console.ReadLine();

                    if (!string.IsNullOrEmpty(summonerName))
                    {
                        summonerList.AddMany(summonerName);
                        Jobs.saveSummonerData<List<string>>(filePath, summonerList, false);

                        var response = SummonerController.getSummonerPuuid(summonerList, apiToken, client);

                        if (!string.IsNullOrEmpty(response.puuid))
                        {
                            summonerPuuidList.Add(response.puuid);
                            
                            Console.WriteLine("\nAccount Puuid : " + response.puuid + "\n");
                            userSpecified = true;
                        }
                        else { summonerList.Clear(); }
                    }
                    else
                    {
                        Console.WriteLine("\nYou Must input a value for UserName!");
                    }
                } while (!userSpecified);


                if (summonerPuuidList.Count > 0)
                {
                    MatchController.getSummonerMatches(summonerPuuidList, apiToken, client, matchIdList, matchIdDictionary);
                }

                if (matchIdDictionary.Count > 0)
                {
                    MatchController.getMatchData(matchIdDictionary, apiToken, clientRegion, matchDataList);
                }

                if (matchDataList.Count > 0)
                {
                    Jobs.saveMatchData<MatchDataModel>(filePath, matchDataList, false);
                }

            }

            
            var winCount = 0;

            foreach(var match in matchDataList)
            {
                if(match.Metadata != null)
                {
                    var gameDuration = TimeConversion.SecondsToMinutes(match.Info.GameDuration);

                    Console.WriteLine("gameDuration: " + gameDuration);

                    foreach (var participant in match.Info.Participants)
                    {
                        if (participant.Puuid == summonerPuuidList[0])
                        {
                           // participant.
                        }
                    }
                }  
            }
            Console.WriteLine("Wins in last 10 Games: " + winCount);

            Console.ReadKey();
        }
    }
}
