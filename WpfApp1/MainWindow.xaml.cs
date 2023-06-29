using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Helpers;
using WpfApp1.Instances;
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, StackPanel> _potsElementsDictionary;
        Dictionary<string, StackPanel> _teamsElementDictionary;
        private int _potCount;
        private int _teamCount;

        public MainWindow()
        {
            InitializeComponent();
            InitalizePageElements();
        }

        private void InitalizePageElements()
        {
            _potsElementsDictionary = new Dictionary<string, StackPanel>
            {
                { "Pot1", Pot1 },
                { "Pot2", Pot2 },
                { "Pot3", Pot3 },
                { "Pot4", Pot4 }
            };
            _teamsElementDictionary = new Dictionary<string, StackPanel>
            {
                { "Team1", Team1 },
                { "Team2", Team2 },
                { "Team3", Team3 },
                { "Team4", Team4 }
            };
        }

        private void GeneratePotsButton_Click(object sender, RoutedEventArgs e)
        {
            _potCount = Convert.ToInt32(PotsCountComboBox.Text);

            var stackPanelName = "Pot";
            PageElementsActions.ClearStackPanels(_potsElementsDictionary, stackPanelName);
            PageElementsActions.ClearStackPanels(_teamsElementDictionary, "Team");
            PageElementsActions.GenerateStackPanels(_potsElementsDictionary, stackPanelName, _potCount);

            TeamsGroupBox.IsEnabled = true;
        }

        private void GenerateTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            _teamCount = Convert.ToInt32(TeamsCountComboBox.Text);
            var stackPanelName = "Team";
            PageElementsActions.ClearStackPanels(_teamsElementDictionary, stackPanelName);

            //var allplayersFromPots = PageElementsActions.GetPlayersFromAvailablePots(_potsElementsDictionary, _potCount);

            var allPlayers = PageElementsActions.GetPlayersFromAvailablePots(_potsElementsDictionary, _potCount);
            var playersPerTeam = allPlayers.Count / _teamCount;

            var teams = TeamGenerator.GenerateTeams(ref allPlayers, playersPerTeam, _teamCount);
            
            PageElementsActions.GenerateStackPanels(_teamsElementDictionary, stackPanelName, _teamCount, isEnabled: false);
            PageElementsActions.SetPlayersToAvailablePots(_teamsElementDictionary, teams, _teamCount);

            //var list = Gen();
            //var a = TeamGenerator.GenerateTeam(ref allPlayers, playersPerTeam);




            //if (TeamGenerator.GetAllPlayers(allplayersFromPots) > _teamCount)
            //{
            //    PageElementsActions.GenerateStackPanels(_teamsElementDictionary, stackPanelName, _teamCount, isEnabled: false);

            //    var teams = TeamGenerator.GenerateTeams(allplayersFromPots, _teamCount);
            //    PageElementsActions.SetPlayersToAvailablePots(_teamsElementDictionary, teams, _teamCount);
            //}
            //else
            //{
            //    MessageBox.Show("Chyba Ty!!!");
            //}
        }

        private static List<Player> Gen()
        {
            return new()
            {
                new()
                {
                    Level = 1,
                    Name = "Roman"
                },
                new()
                {
                    Level = 1,
                    Name = "Pawel"
                },
                new()
                {
                    Level = 2,
                    Name = "Bartpsz"
                },
                new()
                {
                    Level = 2,
                    Name = "Bartek"
                },
            };
        }
    }
}
