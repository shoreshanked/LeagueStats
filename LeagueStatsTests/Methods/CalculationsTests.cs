using LeagueStats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using LeagueStatsTests.Properties;
using Newtonsoft.Json;
using Xunit;
using System.Collections;

namespace LeagueStats.Tests
{
    [TestClass()]
    public class TestDataGenerator : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { JsonConvert.DeserializeObject<List<MatchDataModel>>(Resources.MatchDataTest1_WinsLast10Games), 3 };
            yield return new object[] { JsonConvert.DeserializeObject<List<MatchDataModel>>(Resources.MatchDataTest2_WinsLast10Games), 3 };
            yield return new object[] { JsonConvert.DeserializeObject<List<MatchDataModel>>(Resources.MatchDataTest3_WinsLast10Games), 3 };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CalculationsTests
    {
        [Theory]
        [ClassData(typeof(TestDataGenerator))]
        public void winsInLast10GamesTest(List<MatchDataModel> matchData, int expectedValue)
        {
            List<string> summonerPuuidList = new List<string>();
            summonerPuuidList.Add("Pw5naI2DkgV1tlwsH0rypFrEYw78fLXIZMHXc9JVri4cQid7_VDCNKZfXEtrsFAcWnfMg8Bt1e4O1w");
             var test = Calculations.winsInLast10Games(matchData, summonerPuuidList);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedValue, test);

        }

    }
}