using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LeagueStats
{
    class Jobs
    {
        public static void WriteToJsonFile<MatchDataModel>(string savePath, List<MatchDataModel> mDataList, bool append = false) where MatchDataModel : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(mDataList);
                writer = new StreamWriter(savePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static List<MatchDataModel> ReadFromJsonFile<MatchDataModel>(string filePath) where MatchDataModel : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<MatchDataModel>>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


    }
}
