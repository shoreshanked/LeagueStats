using System;
using System.Collections.Generic;


namespace LeagueStats
{

    class Calculations
    {
        public static int winsInLast10Games(List<MatchDataModel> matchdata, List<string> summonerPuuidList)
        {
            var winCount = 0;
            foreach(var match in matchdata)
            {
                if(match.Metadata != null)
                {
                    foreach(var participant in match.Info.Participants)
                    {
                        if(participant.Puuid == summonerPuuidList[0] && participant.Win == true)
                        {
                            winCount++;
                        }
                    }
                }
            }
            return winCount;
        }

        public static List<MatchOverviewModel> calculateOverviewStats(List<MatchDataModel> matchdata, List<string> summonerPuuidList)
        {
            List<MatchOverviewModel> masterGameList = new List<MatchOverviewModel>();
            List<MatchOverviewModel> myTeamList = new List<MatchOverviewModel>();
            List<MatchOverviewModel> opponentTeamList = new List<MatchOverviewModel>();

            foreach (var match in matchdata)
            {
                List<Participant> team1 = new List<Participant>();
                List<Participant> team2 = new List<Participant>();
                MatchOverviewModel team1Overview = new MatchOverviewModel();
                MatchOverviewModel team2Overview = new MatchOverviewModel();

                if (match.Metadata != null)
                {
                    //var gameDuration = TimeConversion.SecondsToMinutes(match.Info.GameDuration);
                    //Console.WriteLine("gameDuration: " + gameDuration);

                    foreach (var participant in match.Info.Participants)
                    {
                        sortTeams(participant, team1, team2);
                    }

                    team1Overview.teamID = teamCheck(team1);
                    team1Overview.matchID = match.Metadata.MatchId;
                    team1Overview.totalKills = calculateTotalTeamKills(team1);
                    team1Overview.totalAssists = calculateTotalTeamAssists(team1);
                    team1Overview.totalDeaths = calculateTotalTeamDeaths(team1);
                    team1Overview.totalHealing = calculateTotalTeamHealing(team1);
                    team1Overview.totalGoldEarned = calculateTotalTeamGold(team1);
                    team1Overview.totalGoldSpent = calculateTotalTeamGoldSpent(team1);
                    team1Overview.winGame = winCheck(team1);
                    team1Overview.myTeam = summonerCheck(team1, summonerPuuidList);

                    team2Overview.teamID = teamCheck(team2);
                    team2Overview.matchID = match.Metadata.MatchId;
                    team2Overview.totalKills = calculateTotalTeamKills(team2);
                    team2Overview.totalAssists = calculateTotalTeamAssists(team2);
                    team2Overview.totalDeaths = calculateTotalTeamDeaths(team2);
                    team2Overview.totalHealing = calculateTotalTeamHealing(team2);
                    team2Overview.totalGoldEarned = calculateTotalTeamGold(team2);
                    team2Overview.totalGoldSpent = calculateTotalTeamGoldSpent(team2);
                    team2Overview.winGame = winCheck(team2);
                    team2Overview.myTeam = summonerCheck(team2, summonerPuuidList);

                    masterGameList.AddMany(team1Overview, team2Overview);
                }
            }

            

            foreach (var gamePlayed in masterGameList)
            {
                if (gamePlayed.myTeam == true)
                {
                    myTeamList.Add(gamePlayed);
                }
                else
                {
                    opponentTeamList.Add(gamePlayed);
                }
            }

            foreach(var game in myTeamList)
            {
                Console.WriteLine("Total Deaths: {0}\nTotal Assists: {1}\nTotal Kills: {2}\n", game.totalDeaths, game.totalAssists, game.totalKills);

            }
            return null;
        }

        

        public static bool summonerCheck(List<Participant> team, List<string> summonerPuuidList)
        {
            foreach(var player in team)
            {
                if(player.Puuid.Contains(summonerPuuidList[0]))
                {
                    return true;
                }
            }return false;
        }

        public static int teamCheck(List<Participant> team)
        {
            if (team[0].TeamId == 100) { return 1; }
            else { return 2; }
        }

        public static bool winCheck(List<Participant> team)
        {
            if(team[0].Win == true){return true;}
            else{return false;}  
        }

        public static int calculateTotalTeamKills(List<Participant> team)
        {
            var totalTeamKills = 0;
            foreach (var player in team)
            {
                 totalTeamKills = totalTeamKills + player.Kills;
            }
            return totalTeamKills;
        }

        public static int calculateTotalTeamDeaths(List<Participant> team)
        {
            var totalTeamDeaths = 0;
            foreach (var player in team)
            {
                totalTeamDeaths = totalTeamDeaths + player.Deaths;
            }
            return totalTeamDeaths;
        }

        public static int calculateTotalTeamAssists(List<Participant> team)
        {
            var totalTeamAssists = 0;
            foreach (var player in team)
            {
                totalTeamAssists = totalTeamAssists + player.Assists;
            }
            return totalTeamAssists;
        }

        public static int calculateTotalTeamHealing(List<Participant> team)
        {
            var totalTeamHealing = 0;
            foreach (var player in team)
            {
                totalTeamHealing = totalTeamHealing + player.TotalHealsOnTeammates;
            }
            return totalTeamHealing;
        }

        public static int calculateTotalTeamGold(List<Participant> team)
        {
            var totalTeamGold = 0;
            foreach (var player in team)
            {
                totalTeamGold = totalTeamGold + player.GoldEarned;
            }
            return totalTeamGold;
        }

        public static int calculateTotalTeamGoldSpent(List<Participant> team)
        {
            var totalTeamGoldSpent = 0;
            foreach (var player in team)
            {
                totalTeamGoldSpent = totalTeamGoldSpent + player.GoldSpent;
            }
            return totalTeamGoldSpent;
        }

        public static void sortTeams(Participant participant, List<Participant> team1, List<Participant> team2)
        {
            if (participant.TeamId == 100)
            {
                team1.Add(participant);
            }
            else if(participant.TeamId == 200)
            {
                team2.Add(participant);
            }
        }
    }
}
