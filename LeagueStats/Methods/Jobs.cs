using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace LeagueStats
{
    class Jobs
    {
        public static void saveMatchData<MatchDataModel>(string savePath, List<MatchDataModel> mDataList, bool append = false) where MatchDataModel : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(mDataList);
                writer = new StreamWriter(savePath + "\\test.txt", append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void saveSummonerData<List>(string savePath, List<string> summonerList, bool append = false) where List : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(summonerList);
                writer = new StreamWriter(savePath + "\\summoner.txt", append);
                writer.Write(contentsToWriteToFile);

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static List<MatchDataModel> loadMatchData<MatchDataModel>(string filePath) where MatchDataModel : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath + "\\test.txt");
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<MatchDataModel>>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static List<string> loadSummonerData<List>(string filePath) where List : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath + "\\summoner.txt");
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<string>>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


    }
}
