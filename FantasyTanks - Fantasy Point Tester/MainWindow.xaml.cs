using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace FantasyTanks___Fantasy_Point_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PlayerCollection> LoadedReplays { get; } = new ObservableCollection<PlayerCollection>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


                if (files.Where(path => path.EndsWith(".wotreplay") == false).Count() > 0)
                {
                    MessageBox.Show("Unsupported File Type, only World of Tanks replays (*.wotreplay) files are supported");
                    return;
                }

                loadReplays(files, (e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey);
            }
        }

        private Microsoft.Win32.OpenFileDialog replayDialog = new Microsoft.Win32.OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            DefaultExt = ".wotreplay",
            Filter = "World of Tanks Replay Files (*.wotreplay)|*.wotreplay",
            Multiselect = true,
            Title = "Select Replay(s) to Load",
            InitialDirectory = "C:\\World_of_Tanks\\replays"
        };

        private void selectReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if(replayDialog.ShowDialog() == true)
            {
                loadReplays(replayDialog.FileNames);
            }

        }

        private void addReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (replayDialog.ShowDialog() == true)
            {
                loadReplays(replayDialog.FileNames, true);
            }

        }

        private const string OVERALL_STATS_KEY = "Overall Stats";

        private void loadReplays(string[] files, bool keepExisting = false)
        {
            try {
                if (keepExisting != true)
                {
                    this.LoadedReplays.Clear();
                }
                else
                {
                    this.LoadedReplays.Remove(this.LoadedReplays.First(r => r.Title == OVERALL_STATS_KEY));
                }

                
                var skippedReplays = 0;
                foreach (var path in files.OrderBy(p => p))
                {
                    Replay replay;

                    try
                    {
                        replay = new Replay(System.IO.Path.GetFileName(path), ReplayParser.Parse(path));
                    }
                    catch (Replay.ReplayException)
                    {
                        skippedReplays += 1;
                        continue;
                    }
                    var vehs = new PlayerCollection(replay);
                    this.LoadedReplays.Add(vehs);
                }

                var playerStatsByPlayer = new Dictionary<int, List<ReplayPlayer>>();
                foreach (var lr in this.LoadedReplays)
                {
                    foreach (var veh in lr.Vehicles)
                    {
                        if (playerStatsByPlayer.ContainsKey(veh.AccountDBID))
                        {
                            playerStatsByPlayer[veh.AccountDBID].Add(veh);
                        }
                        else
                        {
                            playerStatsByPlayer.Add(veh.AccountDBID, new List<ReplayPlayer>() { veh });
                        }
                    }
                }

                if (playerStatsByPlayer.Count > 0)
                {
                    var playersOverallStats = new List<ReplayPlayer>();

                    foreach (var kp in playerStatsByPlayer)
                    {
                        var replayPlayer = new ReplayPlayer(
                            kp.Key,
                            0,
                            kp.Value.Sum(v => v.Spotted),
                            kp.Value.Sum(v => v.Damaged),
                            kp.Value.Sum(v => v.DamageDealt),
                            kp.Value.Sum(v => v.SniperDamageDealt),
                            kp.Value.Sum(v => v.DamageAssistedTrack),
                            kp.Value.Sum(v => v.DamageAssistedRadio),
                            kp.Value.Sum(v => v.DamageReceived),
                            kp.Value.Sum(v => v.PotentialDamageReceived),
                            kp.Value.Sum(v => v.DamageBlockedByArmor),
                            kp.Value.Sum(v => v.Kills),
                            kp.Value.Sum(v => v.Health),
                            kp.Value.Sum(v => v.CapturePoints),
                            kp.Value.Sum(v => v.DroppedCapturePoints),
                            kp.Value.Sum(v => v.Shots),
                            kp.Value.Sum(v => v.DirectHits),
                            kp.Value.Sum(v => v.Piercings),
                            kp.Value.Sum(v => v.ExplosionHits),
                            kp.Value.Sum(v => v.PiercingsReceived),
                            kp.Value.Sum(v => v.DirectHitsReceived),
                            kp.Value.Sum(v => v.ExplosionHitsReceived),
                            kp.Value.Sum(v => v.NoDamageDirectHitsReceived),
                            int.MaxValue,
                            int.MaxValue,
                            kp.Value.Sum(v => v.Mileage),
                            kp.Value.Sum(v => v.LifeTime),
                            kp.Value.Sum(v => v.TeamDamageDealt),
                            kp.Value.Sum(v => v.TeamKills),
                            kp.Value.Sum(v => v.StunnedTanks),
                            kp.Value.Sum(v => v.DamageAssistedStun),
                            kp.Value.Sum(v => v.StunDuration),
                            kp.Value.Sum(v => v.StunTimes),
                            kp.Value.Sum(v => v.XP)
                        );

                        replayPlayer.SetFantasyPoints(kp.Value.Sum(v => v.OldFantasyPoints), kp.Value.Sum(v => v.NewFantasyPoints < 0 ? 0 : v.NewFantasyPoints));

                        replayPlayer.NFP_DamageBlockedByArmorPoints = (kp.Value.Sum(v => (v.NFP_DamageBlockedByArmorPoints)));
                        replayPlayer.NFP_DamagedPoints = (kp.Value.Sum(v => (v.NFP_DamagedPoints)));
                        replayPlayer.NFP_DamagePoints = (kp.Value.Sum(v => (v.NFP_DamagePoints)));
                        replayPlayer.NFP_DefensePointsPoints = (kp.Value.Sum(v => (v.NFP_DefensePointsPoints)));
                        replayPlayer.NFP_KillsPoints = (kp.Value.Sum(v => (v.NFP_KillsPoints)));
                        replayPlayer.NFP_PenRatePoints = (kp.Value.Sum(v => (v.NFP_PenRatePoints)));
                        replayPlayer.NFP_PlayerLifetimePoints = (kp.Value.Sum(v => (v.NFP_PlayerLifetimePoints)));
                        replayPlayer.NFP_PlayerLifetimeTeamAverage = double.NaN;
                        replayPlayer.NFP_SpottedAssistedPoints = (kp.Value.Sum(v => (v.NFP_SpottedAssistedPoints)));
                        replayPlayer.NFP_SpottedPoints = (kp.Value.Sum(v => (v.NFP_SpottedPoints)));
                        replayPlayer.NFP_TrackAssistedPoints = (kp.Value.Sum(v => (v.NFP_TrackAssistedPoints)));

                        replayPlayer.OFP_CapturePointsPoints = (kp.Value.Sum(v => (v.OFP_CapturePointsPoints)));
                        replayPlayer.OFP_DamageAssistedRadioPoints = (kp.Value.Sum(v => (v.OFP_DamageAssistedRadioPoints)));
                        replayPlayer.OFP_DamageAssistedTrackPoints = (kp.Value.Sum(v => (v.OFP_DamageAssistedTrackPoints)));
                        replayPlayer.OFP_DamagedPoints = (kp.Value.Sum(v => (v.OFP_DamagedPoints)));
                        replayPlayer.OFP_DamagePoints = (kp.Value.Sum(v => (v.OFP_DamagePoints)));
                        replayPlayer.OFP_DamageRatioPoints = (kp.Value.Sum(v => (v.OFP_DamageRatioPoints)));
                        replayPlayer.OFP_DefensePointsPoints = (kp.Value.Sum(v => (v.OFP_DefensePointsPoints)));
                        replayPlayer.OFP_HitRatePoints = (kp.Value.Sum(v => (v.OFP_HitRatePoints)));
                        replayPlayer.OFP_KillsPoints = (kp.Value.Sum(v => (v.OFP_KillsPoints)));
                        replayPlayer.OFP_MilagePoints = (kp.Value.Sum(v => (v.OFP_MilagePoints)));
                        replayPlayer.OFP_NoDamageDirectHitsReceivedPoints = (kp.Value.Sum(v => (v.OFP_NoDamageDirectHitsReceivedPoints)));
                        replayPlayer.OFP_PenRatePoints = (kp.Value.Sum(v => (v.OFP_PenRatePoints)));
                        replayPlayer.OFP_PotentialDamagePoints = (kp.Value.Sum(v => (v.OFP_PotentialDamagePoints)));
                        replayPlayer.OFP_SpottedPoints = (kp.Value.Sum(v => (v.OFP_SpottedPoints)));
                        replayPlayer.OFP_VictoryPoints = (kp.Value.Sum(v => (v.OFP_VictoryPoints)));

                        replayPlayer.OFP_CalculateMinMax();
                        replayPlayer.NFP_CalculateMinMax();

                        replayPlayer.SetPlayerItem(new Replay.PlayerItem() { Name = kp.Value.First().AccountName });
                        playersOverallStats.Add(replayPlayer);
                    }

                    this.LoadedReplays.Add(new PlayerCollection(OVERALL_STATS_KEY, playersOverallStats));
                }

                replayList.SelectedIndex = 0;
                replayPlayers.SelectedIndex = 0;

                replayListStatus.Text = $"Status: Skipped {skippedReplays} replay(s)";
            }
#if DEBUG
            finally
            {

            }
#else
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException)
            {
                MessageBox.Show(ex.Message, "Loading Replay(s) failed due to error");
            }
#endif
        }
    }
}
