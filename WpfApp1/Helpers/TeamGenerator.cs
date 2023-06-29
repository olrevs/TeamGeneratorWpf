using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Instances;

namespace WpfApp1.Helpers
{
    public static class TeamGenerator
    {
        private const string Key = "Team";

        private static Dictionary<string, List<Player>?> InitializeTeams(int teamsCount)
        {
            var teams = new Dictionary<string, List<Player>?>();

            for (int i = 1; i <= teamsCount; i++)
            {
                teams.Add($"{Key}{i}", new List<Player>());
            }

            return teams;
        }

        public static List<Player> ConvertToPlayersList(List<string> list, int number)
        {
            var playerList = new List<Player>();

            foreach (var item in list)
            {
                playerList.Add(new Player
                {
                    Name = item,
                    Level = number
                });
            }

            return playerList;
        }

        public static string ConvertPlayersListToString(List<Player> list)
        {
            List<string> playersName = new();

            foreach (var item in list)
            {
                playersName.Add(item.Name);
            }

            return string.Join("\r\n", playersName.ToArray());
        }

        public static int GetAllPlayers(Dictionary<string, List<Player>> dictionary)
        {
            return dictionary.Values.Sum(_ => _.Count);
        }

        public static Dictionary<string, List<Player>?> GenerateTeams(Dictionary<string, List<Player>> dictionary, int teamsCount)
        {
            var teams = InitializeTeams(teamsCount);
            var totalPlayersCount = GetAllPlayers(dictionary);
            var playersPerTeam = totalPlayersCount / teamsCount;
            var minCountOfPlayersPerPot = dictionary.Values.Min(_ => _.Count);

            for (int teamNumber = 1; teamNumber <= teamsCount; teamNumber++) // loop for all teams
            {
                while (teams?[$"{Key}{teamNumber}"]?.Count < playersPerTeam)
                {
                    for (int playerNumber = 1; playerNumber <= minCountOfPlayersPerPot; playerNumber++)
                    {
                        for (int potNumber = 1; potNumber <= dictionary.Keys.Count; potNumber++)
                        {
                            if (dictionary[$"PlayersPot{potNumber}"].Count > 0)
                            {
                                var randomPlayerIndex = GetRandomIndex(dictionary[$"PlayersPot{minCountOfPlayersPerPot}"].Count);
                                teams?[$"{Key}{teamNumber}"]?.Add(dictionary[$"PlayersPot{potNumber}"].ElementAt(randomPlayerIndex));
                                dictionary[$"PlayersPot{potNumber}"].RemoveAt(randomPlayerIndex);

                                break;
                            }
                        }
                    }
                }
            }

            return teams;
        }

        public static int GetRandomIndex(int count)
        {
            var r = new Random();
            int index = r.Next(count);

            return index;
        }

        public static int GetMinimumPlayersCountPerPot(Dictionary<string, List<Player>> dictionary)
        {
            foreach (var player in dictionary.Values)
            {
                //player.Count;
            }

            return dictionary.Values.Min(_ => _.Count);
        }


        public static Player GetRandomPlayer(List<Player> players)
        {
            var r = new Random();
            int index = r.Next(players.Count);

            return players.ElementAt(index);

            //return index;
        }

        private static List<int> GetPlayersCountPerLevel(List<Player> players)
        {
            var levels = players.Select(_ => _.Level).Distinct().Count();
            var playersCountPerLevel = new List<int>();

            for (int i = 1; i <= levels; i++)
            {
                var playersWithOneLevel = players.Where(player => player.Level.Equals(i)).ToList();
                playersCountPerLevel.Add(playersWithOneLevel.Count);
            }

            return playersCountPerLevel;
        }

        public static List<Player> GenerateTeam(ref List<Player> players)
        {
            List<Player> team = new();
            var levels = players.Select(_ => _.Level).Distinct().Count();

            for (int i = 1; i <= levels; i++)
            {
                var playersWithOneLevel = players.Where(player => player.Level.Equals(i)).ToList();
                if (playersWithOneLevel.Count > 0)
                {
                    var player = GetRandomPlayer(playersWithOneLevel);
                    {
                        team.Add(player);
                        players.Remove(player);
                        playersWithOneLevel.Remove(player);
                    }
                }
            }

            return team;
        }

        public static Dictionary<string, List<Player>> GenerateTeams(ref List<Player> players, int playersPerTeam, int teamCount)
        {
            var teams = InitializeTeams(teamCount);

            var playersCountPerLevel = GetPlayersCountPerLevel(players);

            var a = playersCountPerLevel.Distinct().Count() == 1
                ? playersCountPerLevel.FirstOrDefault()
                : playersCountPerLevel.Min() / teamCount;

            //if (players is not null)
            {

                //for (int j = 0; j < playersCountPerLevel.Min() / teamCount; j++)
                //{
                //    for (int i = 1; i <= teamCount; i++)
                //    {

                //        teams?[$"Team{i}"]?.AddRange(GenerateTeam(ref players));
                //    }
                //}
                //var playersCount = players.Count;
                //playersCountPerLevel = GetPlayersCountPerLevel(players);


                GetRemainerPlayers(ref players, ref teams);
            }

            return teams;
        }

        private static Dictionary<string, List<Player>> GetRemainerPlayers(ref List<Player> players, ref Dictionary<string, List<Player>> teams)
        {
            var minPlayersPerTeam = players.Count / teams.Count;

            while (players.Count > 0)
            {
                var highestTeam = GetRandomHighestTeam(teams);
                //var highestTeam = teams.MinBy(_ => _.Value.Sum(_ => _.Level)).Key;
                var lowestPlayer = GetRandomLowestPlayer(players);
                //var highestPlayer = players.MaxBy(_ => _.Level);
                //if (teams[highestTeam].Count() < minPlayersPerTeam)
                {
                    teams[highestTeam].Add(lowestPlayer);
                    players.Remove(lowestPlayer);
                }
                //else
                //{
                //    teams[highestTeam].Add(lowestPlayer);
                //    players.Remove(lowestPlayer);
                //}
            }

            return teams;
        }

        private static Player GetRandomLowestPlayer(List<Player> players)
        {
            var lowestPlayerLevel = players.Max(p => p.Level);
            var lowestPlayers = players.Where(_ => _.Level == lowestPlayerLevel).ToList();
            var index = GetRandomIndex(lowestPlayers.Count);

            return lowestPlayers.ElementAt(index);
        }

        private static string GetRandomHighestTeam(Dictionary<string, List<Player>> teams)
        {
            var highestTeamLevel = teams.Min(p => p.Value.Sum(_ => _.Level));
            var highestTeams = teams.Where(_ => _.Value.Sum(_ => _.Level) == highestTeamLevel).ToDictionary(k => k.Key, v => v.Value).Keys;
            var index = GetRandomIndex(highestTeams.Count);

            return highestTeams.ElementAt(index);
        }
    }
}
