using System;
using System.Collections.Generic;
using System.IO;
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
        
        public static void SaveData(string filePath, List<string> summoners, List<string> summonersPuuid, List<MatchDataModel> matches )
        {
            Jobs.saveSummonerData<List<string>>(filePath, summoners, false);
            Jobs.saveSummonerPuuidData<List<string>>(filePath, summonersPuuid, false);
            Jobs.saveMatchData<MatchDataModel>(filePath, matches, false);
        }

        public static (List<string> summonerPuuidSave, List<string> summonerSave, List<MatchDataModel> matchesSave) LoadData(List<string> summonerPuuid, List<string> summoner, List<MatchDataModel> matches, string filePath)
        {
            matches = Jobs.loadMatchData<MatchDataModel>(filePath);
            summoner = Jobs.loadSummonerData<List<string>>(filePath);
            summonerPuuid = Jobs.loadSummonerPuuidData<List<string>>(filePath);

            return(summonerPuuid, summoner, matches);
        }

        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SaveState");

            var summonerList = new List<string>();
            var summonerPuuidList = new List<string>();
            var matchIdList = new List<string>();
            var matchDataList = new List<MatchDataModel>();
            IDictionary<string, List<string>> matchIdDictionary = new Dictionary<string, List<string>>();

            //API variables
            var client = new RestClient("https://euw1.api.riotgames.com");
            var clientRegion = new RestClient("https://europe.api.riotgames.com");
            var apiToken = "RGAPI-0872b866-35f8-4c14-b148-6d0cd2c09d51";
            var userSpecified = false;

            //check whether saved data exists. Returns Null is data file is empty
            var getSaveData = LoadData(summonerPuuidList, summonerList, matchDataList, filePath);

            //If saved data methods are not null and user wants to reload, then reload
            var input = "";
            if (getSaveData.matchesSave != null && getSaveData.summonerPuuidSave != null && getSaveData.summonerSave != null)
            {
                Console.WriteLine("Would you like to Load the previous summoner data? \nPress Y to load data");
                input = Console.ReadLine();

                if(input.ToLower() == "y")
                {
                    matchDataList = getSaveData.matchesSave;
                    summonerList = getSaveData.summonerSave;
                    summonerPuuidList = getSaveData.summonerPuuidSave;
                }

            }

            if (getSaveData.matchesSave is null || input.ToLower() != "y")
            {
                do
                {
                    //Console.WriteLine("\nEnter Summoner name");
                    //var summonerName = Console.ReadLine();

                    summonerList.AddMany("ChardeeMacDennis");

                    //if (!string.IsNullOrEmpty(summonerName))
                    if(summonerList.Count > 0)
                    {
                        //summonerList.AddMany(summonerName);
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
                //only save to file if there is actual data to save
                if (summonerList.Count > 0 && summonerPuuidList.Count > 0 && matchDataList.Count > 0)
                {
                    SaveData(filePath, summonerList, summonerPuuidList, matchDataList);
                }
            }

            Calculations.winsInLast10Games(matchDataList, summonerPuuidList);
            Calculations.last10Games(matchDataList, summonerPuuidList);

            Calculations.calculateOverviewStats(matchDataList, summonerPuuidList);

            Console.ReadKey();
        }
    }
}
