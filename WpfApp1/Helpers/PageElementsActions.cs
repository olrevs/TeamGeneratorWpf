using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Instances;

namespace WpfApp1.Helpers
{
    public static class PageElementsActions
    {
        private static TextBox GeneratePot(int potNumber, bool isEnabled = true)
        {
            return new()
            {
                Height = 100,
                Width = 150,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Name = $"PotTextBox{potNumber}",
                Margin = new Thickness(potNumber == 1 ? 0 : 20, 5, 0, 0),
                IsEnabled = isEnabled
            };
        }

        private static TextBlock GeneratePotLabel(int potNumber)
        {
            return new()
            {
                Text = $"Koszyk {potNumber}:",
                Margin = new Thickness(potNumber == 1 ? 0 : 20, 10, 0, 0)
            };
        }

        private static TextBox? GetChildTextBoxFromStackPanel(StackPanel stackPanel)
        {
            return stackPanel.Children.OfType<TextBox>().FirstOrDefault();
        }

        public static void ClearStackPanels(Dictionary<string, StackPanel> dictionary, string stackPanelName, int count = 4)
        {
            for (int i = 1; i <= count; i++)
            {
                dictionary[$"{stackPanelName}{i}"].Children.Clear();
            }
        }

        public static void GenerateStackPanels(Dictionary<string, StackPanel> dictionary, string stackPanelName, int count, bool isEnabled = true)
        {
            for (int i = 1; i <= count; i++)
            {
                dictionary[$"{stackPanelName}{i}"].Children.Add(GeneratePotLabel(i));
                dictionary[$"{stackPanelName}{i}"].Children.Add(GeneratePot(i, isEnabled));
            }
        }

        public static Dictionary<string, List<Player>> GetPlayersListFromAvailablePots(Dictionary<string, StackPanel> dictionary, int potCount)
        {
            Dictionary<string, List<Player>> allPots = new();
            var stackPanelName = "Pot";

            for (int i = 1; i <= potCount; i++)
            {
                var potTextBox = GetChildTextBoxFromStackPanel(dictionary[$"{stackPanelName}{i}"]);
                var playersPerPot = TeamGenerator.ConvertToPlayersList(potTextBox?.Text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)?.ToList(), i);

                allPots.Add($"PlayersPot{i}", playersPerPot);
            }

            return allPots;
        }

        public static List<Player> GetPlayersFromAvailablePots(Dictionary<string, StackPanel> dictionary, int potCount)
        {
            List<Player> players = new();
            var stackPanelName = "Pot";

            for (int i = 1; i <= potCount; i++)
            {
                var potTextBox = GetChildTextBoxFromStackPanel(dictionary[$"{stackPanelName}{i}"]);
                var playersPerPot = TeamGenerator.ConvertToPlayersList(potTextBox?.Text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)?.ToList(), i);

                players.AddRange(playersPerPot);
            }

            return players;
        }

        public static void SetPlayersToAvailablePots(Dictionary<string, StackPanel> dictionary, Dictionary<string, List<Player>?> teams, int teamCount)
        {
            var stackPanelName = "Team";

            if (teams != null)
            {
                for (int i = 1; i <= teamCount; i++)
                {
                    var teamTextBox = GetChildTextBoxFromStackPanel(dictionary[$"{stackPanelName}{i}"]);
                    if (teamTextBox != null)
                    {
                        teamTextBox.Text = TeamGenerator.ConvertPlayersListToString(teams[$"{stackPanelName}{i}"]);
                    }
                }
            }
        }
    }
}
