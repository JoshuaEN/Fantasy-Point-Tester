using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTanks___Fantasy_Point_Tester
{
    public class Replay : INotifyPropertyChanged
    {
        public string Path { get; }

        [JsonIgnore]
        public IReadOnlyList<ReplayPlayer> Vehicles { get; }

        [JsonIgnore]
        public BattleInfoItem BattleInfo { get; }

        [JsonIgnore]
        public CommonItem Common { get; }


        private IReadOnlyList<ReplayPlayer> side1RosterCache;
        [JsonIgnore]
        public IReadOnlyList<ReplayPlayer> Side1Roster
        {
            get
            {
                if (side1RosterCache == null)
                    side1RosterCache = GetRosterForSide(1);

                return side1RosterCache;
            }
        }

        private IReadOnlyList<ReplayPlayer> side2RosterCache;
        [JsonIgnore]
        public IReadOnlyList<ReplayPlayer> Side2Roster
        {
            get
            {
                if (side2RosterCache == null)
                    side2RosterCache = GetRosterForSide(2);

                return side2RosterCache;
            }
        }

        private IReadOnlyList<ReplayPlayer> GetRosterForSide(int side)
        {
            return this.Vehicles.Where(item => item.Side == side).OrderByDescending(i => i.NewFantasyPoints).ToList().AsReadOnly();
        }


        public Replay(string path, ReplayParser.ReplayContent replayContent)
        {

            var jsonChunk1 = replayContent.Chunk1;
            var jsonChunk2 = replayContent.Chunk2;

            this.Path = path;

            if (replayContent.IsMagicNumberValid == false)
            {
                throw new InvalidMagicNumberReplayException($"Replay Magic Number Invalid (the replay may still be being saved): got {replayContent.MagicNumber}");
            }

            #region Parse Chunk 1

            try
            {
                this.BattleInfo = JsonConvert.DeserializeObject<BattleInfoItem>(jsonChunk1);
            }
            catch (JsonReaderException ex)
            {
                throw new ReplayInvalidChunk1Exception($"Invalid JSON Chunk 1 (parse error)", ex);
            }

            if (this.BattleInfo == null)
                throw new ReplayInvalidChunk1Exception("Invalid JSON Chunk 1");

            #endregion

            if (jsonChunk2 != null)
            {
                #region Parse Chunk 2

                JArray baseArray = null;

                try
                {
                    baseArray = JArray.Parse(jsonChunk2);
                }
                catch (JsonReaderException ex)
                {
                    throw new ReplayInvalidChunk2Exception($"Invalid JSON Chunk 2 (parse error)", ex);
                }

                if (baseArray == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (parse failed)");

                if (baseArray.Count < 2)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array.length < 2)");

                if (baseArray[0]["vehicles"] == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array[0].vehicles is null)");

                if (baseArray[0]["common"] == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array[0].common is null)");


                var vehicleArryDict = baseArray[0]["vehicles"].ToObject<Dictionary<string, List<ReplayPlayer>>>();

                if (vehicleArryDict == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array[0].vehicles parse failed)");

                var vehicleDict = new Dictionary<string, ReplayPlayer>(vehicleArryDict.Count);

                foreach (var playerBattleID in vehicleArryDict.Keys)
                {
                    vehicleDict[playerBattleID] = vehicleArryDict[playerBattleID]?[0];
                }

                this.Common = baseArray[0]["common"].ToObject<CommonItem>();

                if (this.Common == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array[0].common parse failed)");

                var playerInfoDict = baseArray[1].ToObject<Dictionary<string, PlayerItem>>();

                if (playerInfoDict == null)
                    throw new ReplayInvalidChunk2Exception("Invalid JSON Chunk 2 (array[1] parse failed)");

                foreach (var playerBattleID in playerInfoDict.Keys)
                {
                    var vehicleItem = vehicleDict[playerBattleID];

                    vehicleItem?.SetPlayerItem(playerInfoDict[playerBattleID]);
                    vehicleItem?.SetPlayerBattleID(playerBattleID);
                    vehicleItem?.SetReplay(this);
                }

                Vehicles = vehicleDict.Values.Where(v => v.TankTag.ToLower() != "ussr:observer").ToList().AsReadOnly();

                #endregion
            }
            else
            {
                throw new ReplayIncompleteException("Incomplete Replay; Unable to process");
            }


            if (jsonChunk2 != null)
            {
                foreach (var vehicle in Vehicles)
                {
                    if (vehicle.Side == this.Common.WinningSide)
                    {
                        vehicle.SetVictory(true);
                    }
                    else
                    {
                        vehicle.SetVictory(false);
                    }
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Exceptions

        [Serializable]
        public class ReplayException : Exception
        {
            public ReplayException() { }

            public ReplayException(string message) : base(message) { }

            public ReplayException(string message, Exception inner) : base(message, inner) { }
        }

        public class ReplayIncompleteException : ReplayException
        {
            public ReplayIncompleteException() { }

            public ReplayIncompleteException(string message) : base(message) { }

            public ReplayIncompleteException(string message, Exception inner) : base(message, inner) { }
        }

        public class ReplayDataFormatException : ReplayException
        {
            public ReplayDataFormatException() { }

            public ReplayDataFormatException(string message) : base(message) { }

            public ReplayDataFormatException(string message, Exception inner) : base(message, inner) { }
        }

        public class ReplayInvalidChunk1Exception : ReplayException
        {
            public ReplayInvalidChunk1Exception() { }

            public ReplayInvalidChunk1Exception(string message) : base(message) { }

            public ReplayInvalidChunk1Exception(string message, Exception inner) : base(message, inner) { }
        }

        public class ReplayInvalidChunk2Exception : ReplayException
        {
            public ReplayInvalidChunk2Exception() { }

            public ReplayInvalidChunk2Exception(string message) : base(message) { }

            public ReplayInvalidChunk2Exception(string message, Exception inner) : base(message, inner) { }
        }

        public class InvalidMagicNumberReplayException : ReplayException
        {
            public InvalidMagicNumberReplayException() { }

            public InvalidMagicNumberReplayException(string message) : base(message) { }

            public InvalidMagicNumberReplayException(string message, Exception inner) : base(message, inner) { }
        }

        public class LoadedGameVersionMismatchReplayException : ReplayException
        {
            public LoadedGameVersionMismatchReplayException() { }

            public LoadedGameVersionMismatchReplayException(string message) : base(message) { }

            public LoadedGameVersionMismatchReplayException(string message, Exception inner) : base(message, inner) { }
        }

        public class UnsupportedGameVersionReplayException : ReplayException
        {
            public UnsupportedGameVersionReplayException() { }

            public UnsupportedGameVersionReplayException(string message) : base(message) { }

            public UnsupportedGameVersionReplayException(string message, Exception inner) : base(message, inner) { }
        }

        public class UnsupportedGameRegionReplayException : ReplayException
        {
            public UnsupportedGameRegionReplayException() { }

            public UnsupportedGameRegionReplayException(string message) : base(message) { }

            public UnsupportedGameRegionReplayException(string message, Exception inner) : base(message, inner) { }
        }

        public class UnableToDetectWinningTeamReplayException : ReplayException
        {
            public UnableToDetectWinningTeamReplayException() { }

            public UnableToDetectWinningTeamReplayException(string message) : base(message) { }

            public UnableToDetectWinningTeamReplayException(string message, Exception inner) : base(message, inner) { }
        }

        #endregion

        public class BattleInfoItem
        {
            public string ClientVersionFromXml { get; }
            public string ClientVersionFromExe { get; }
            public string GameplayName { get; }
            public string DisplayName { get; }
            public string Name { get; }
            public string Region { get; }
            public int BattleType { get; }

            public string MapDisplayName { get; }
            public string MapName { get; }

            public BattleInfoItem(string clientVersionFromXml, string clientVersionFromExe, string mapDisplayName, string gameplayID, string regionCode, string mapName, int battleType)
            {
                this.ClientVersionFromXml = clientVersionFromXml;
                this.ClientVersionFromExe = clientVersionFromExe;


                this.DisplayName = mapDisplayName;
                this.GameplayName = gameplayID;
                this.Region = regionCode;
                this.Name = mapName;
                this.BattleType = battleType;

                this.MapDisplayName = mapDisplayName;
                this.MapName = mapName;
            }
        }

        public class CommonItem
        {
            public int FinishReason { get; }
            public int Duration { get; }

            [JsonProperty("WinningTeam")]
            public int WinningSide { get; }

            public CommonItem(int finishReason, int duration, int winnerTeam)
            {
                this.FinishReason = finishReason;
                this.Duration = duration;
                this.WinningSide = winnerTeam;
            }
        }

        public class PlayerItem
        {
            public string VehicleType { get; set; }
            public string Name { get; set; }
        }       

    }
}
