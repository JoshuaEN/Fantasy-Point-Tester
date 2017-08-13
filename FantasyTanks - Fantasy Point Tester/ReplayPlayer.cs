using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTanks___Fantasy_Point_Tester
{
    public class ReplayPlayer
    {
        [Browsable(false)]
        public Replay Replay { get; private set; }

        [Browsable(false)]
        public int PreBattleID { get; private set; }

        private Tank tankCache;
        [Browsable(false)]
        public Tank Tank
        {
            get
            {
                if (this.tankCache == null && this.TankTag != null && this.Replay != null)
                {
                    var v = this.Replay.BattleInfo.ClientVersionFromExe.Split('.');
                    int major;
                    int minior;
                    int patch;
                    Tanks.Version version = Tanks.Version.Version9_19_1;

                    if (int.TryParse(v[1], out major) && int.TryParse(v[2], out minior) && int.TryParse(v[3], out patch))
                    {
                        if(major >= 9)
                        {
                            if(minior >= 19 && patch >= 1)
                            {
                                version = Tanks.Version.Version9_19_1;
                            }
                            else
                            {
                                version = Tanks.Version.Version9_19_or_lower;
                            }
                        }
                        else
                        {
                            version = Tanks.Version.Version9_17_or_lower;
                        }
                        this.tankCache = Tanks.FindByTag(version, this.TankTag);
                    }
                }

                return this.tankCache;
            }
        }

        [Category("1. General")]
        [DisplayName("Account ID")]
        public int AccountDBID { get; }
        private string accountName;
        [Category("1. General")]
        [DisplayName("Account Name")]
        public string AccountName
        {
            get { return this.accountName; }
            private set
            {
                this.accountName = value;
            }
        }
        [Browsable(false)]
        public string TankTag { get; private set; }

        [Category("1. General")]
        [DisplayName("Tank")]
        public string TankDisplayName { get { return this.Tank?.Name; } }
        public int Side { get; }

        [Category("Other")]
        [DisplayName("Killer ID")]
        public int KillerID { get; protected set; }

        [Category("Other")]
        [DisplayName("Death Reason")]
        public int DeathReason { get; protected set; }

        [Category("1. General")]
        [DisplayName("Victory")]
        public bool Victory { get; protected set; }

        [JsonProperty("tanks_spotted")]
        [Category("4. General Stats")]
        public int Spotted { get; protected set; }

        [JsonProperty("damaged_tanks")]
        [Category("4. General Stats")]
        public int Damaged { get; protected set; }

        [JsonProperty("damage")]
        [Category("4. General Stats")]
        [DisplayName("Damage")]
        public int DamageDealt { get; protected set; }

        [JsonProperty("sniper_damage")]
        [Category("4. General Stats")]
        [DisplayName("Sniper Damage")]
        public int SniperDamageDealt { get; protected set; }

        [JsonProperty("tracking_assisted")]
        [Category("4. General Stats")]
        [DisplayName("Assisted Damage (Tracking)")]
        public int DamageAssistedTrack { get; protected set; }

        [JsonProperty("spotting_assisted")]
        [Category("4. General Stats")]
        [DisplayName("Assisted Damage (Spotting)")]
        public int DamageAssistedRadio { get; protected set; }

        [JsonProperty("damage_received")]
        [Category("4. General Stats")]
        [DisplayName("Damage Received")]
        public int DamageReceived { get; protected set; }

        [JsonProperty("potential_damage_received")]
        [Category("4. General Stats")]
        [DisplayName("Potential Damage Received")]
        public int PotentialDamageReceived { get; protected set; }

        [JsonProperty("damage_blocked_by_armor")]
        [Category("4. General Stats")]
        [DisplayName("Damage Blocked by Armor")]
        public int DamageBlockedByArmor { get; protected set; }

        [JsonProperty("damageReceivedFromInvisibles")]
        public int DamageReceivedFromInvisibles { get; protected set; }

        [JsonProperty("kills")]
        [Category("4. General Stats")]
        public int Kills { get; protected set; }

        [JsonProperty("remaining_health")]
        [Category("4. General Stats")]
        [DisplayName("Remaining Health")]
        public int Health { get; protected set; }

        [JsonProperty("capture_points")]
        [Category("4. General Stats")]
        [DisplayName("Capture Points")]
        public int CapturePoints { get; protected set; }

        [JsonProperty("defense_points")]
        [Category("4. General Stats")]
        [DisplayName("Defense Points")]
        public int DroppedCapturePoints { get; protected set; }

        [JsonProperty("shots_fired")]
        [Category("4. General Stats")]
        [DisplayName("Shots Fired")]
        public int Shots { get; protected set; }

        [JsonProperty("shots_hit")]
        [Category("4. General Stats")]
        [DisplayName("Shots Hit")]
        public int DirectHits { get; protected set; }

        [JsonProperty("shots_pened")]
        [Category("4. General Stats")]
        [DisplayName("Shots Penetrated")]
        public int Piercings { get; protected set; }

        [JsonProperty("explosionHits")]
        [Category("4. General Stats")]
        [DisplayName("Explosion Hits")]
        public int ExplosionHits { get; protected set; }

        [JsonProperty("pens_taken")]
        [Category("4. General Stats")]
        [DisplayName("Penetrated Hits Received")]
        public int PiercingsReceived { get; protected set; }

        [JsonProperty("total_hits_taken")]
        [Category("4. General Stats")]
        [DisplayName("Total Hits Received")]
        public int DirectHitsReceived { get; protected set; }

        [JsonProperty("he_hits_taken")]
        [Category("4. General Stats")]
        [DisplayName("HE Hits Received")]
        public int ExplosionHitsReceived { get; protected set; }

        [JsonProperty("no_damage_hits_taken")]
        [Category("4. General Stats")]
        [DisplayName("No Damage Hits Received")]
        public int NoDamageDirectHitsReceived { get; protected set; }

        [JsonProperty("distance_traveled")]
        [Category("4. General Stats")]
        [DisplayName("Distance Traveled")]
        public int Mileage { get; protected set; }

        [JsonProperty("lifetime")]
        [Category("4. General Stats")]
        [DisplayName("Lifetime (in seconds)")]
        public int LifeTime { get; protected set; }

        [JsonProperty("team_damage")]
        [Category("4. General Stats")]
        [DisplayName("Team Damage")]
        public int TeamDamageDealt { get; protected set; }

        [JsonProperty("team_kills")]
        [Category("4. General Stats")]
        [DisplayName("Team Kills")]
        public int TeamKills { get; protected set; }

        [JsonProperty("stunned")]
        public int StunnedTanks { get; protected set; }

        [JsonProperty("damageAssistedStun")]
        public int DamageAssistedStun { get; protected set; }

        [JsonProperty("stunDuration")]
        public double StunDuration { get; protected set; }

        [JsonProperty("stunNum")]
        public int StunTimes { get; protected set; }

        [JsonProperty("xp")]
        public int XP { get; protected set; }

        public double HitRate
        {
            get
            {
                var hit_rate = (double)this.DirectHits / (double)this.Shots;

                if (double.IsNaN(hit_rate) || double.IsInfinity(hit_rate))
                    hit_rate = 0.0d;
                return hit_rate;
            }
        }

        public double PenRate
        {
            get
            {
                var pen_rate = (double)this.Piercings / (double)this.DirectHits;

                if (double.IsNaN(pen_rate) || double.IsInfinity(pen_rate))
                    pen_rate = 0.0d;
                return pen_rate;
            }
        }

        public double DamageRatio
        {
            get
            {
                var damage_ratio_damage_received = (double)this.DamageReceived;

                if (this.DamageReceived == 0)
                    damage_ratio_damage_received = 1;

                var damage_ratio = (double)this.DamageDealt / damage_ratio_damage_received;

                return damage_ratio;
            }
        }

        private const bool OFP_NORMALIZE_CAPTURE_POINTS = true;

        private double? oldFantasyPoints = null;
        public double OldFantasyPoints
        {
            get
            {
                if (this.oldFantasyPoints.HasValue == false)
                {
                    var hit_rate = (double)this.DirectHits / (double)this.Shots;
                    var pen_rate = (double)this.Piercings / (double)this.DirectHits;

                    var damage_ratio_damage_received = (double)this.DamageReceived;

                    if (this.DamageReceived == 0)
                        damage_ratio_damage_received = 1;

                    var damage_ratio = (double)this.DamageDealt / damage_ratio_damage_received;

                    if (double.IsNaN(hit_rate) || double.IsInfinity(hit_rate))
                        hit_rate = 0.0d;
                    if (double.IsNaN(pen_rate) || double.IsInfinity(pen_rate))
                        pen_rate = 0.0d;
                    if (double.IsNaN(damage_ratio) || double.IsInfinity(damage_ratio))
                        damage_ratio = 0.0d;

                    if (damage_ratio > 3.0)
                        damage_ratio = 3.0;

                    this.oldFantasyPoints = Math.Round(
                        this.Kills * 2.0d +
                        this.DamageDealt * 0.005d +
                        this.DamageAssistedRadio * 0.01d +
                        this.DamageAssistedTrack * 0.005d +
                        this.Damaged * 0.4d +
                        this.CapturePoints * (OFP_NORMALIZE_CAPTURE_POINTS ? (this.Victory ? 0.2d : 0) : 0.8d )+
                        this.DroppedCapturePoints * (OFP_NORMALIZE_CAPTURE_POINTS ? (this.Victory ? 0.2d : 0) : 0.8d) +
                        this.Mileage * 0.002d +
                        this.NoDamageDirectHitsReceived * 0.3d +
                        this.Spotted * 1.0d +
                        hit_rate * 8.0d +
                        pen_rate * 4.0d +
                        damage_ratio * 8.0d +
                        (this.Victory ? 1.0d : 0.0d) +
                        (DeathReason == -1 ? this.PotentialDamageReceived * 0.003d : 0.0d)
                        , 4);

                    this.OFP_KillsPoints = (this.Kills * 2.0d);
                    this.OFP_DamagePoints = (this.DamageDealt * 0.005d);
                    this.OFP_DamageAssistedRadioPoints = (this.DamageAssistedRadio * 0.01d);
                    this.OFP_DamageAssistedTrackPoints = (this.DamageAssistedTrack * 0.005d);
                    this.OFP_DamagedPoints = (this.Damaged * 0.4d);
                    this.OFP_CapturePointsPoints = (this.CapturePoints * (OFP_NORMALIZE_CAPTURE_POINTS ? (this.Victory ? 0.2d : 0) : 0.8d));
                    this.OFP_DefensePointsPoints = (this.DroppedCapturePoints * (OFP_NORMALIZE_CAPTURE_POINTS ? (this.Victory ? 0.2d : 0) : 0.8d));
                    this.OFP_MilagePoints = (this.Mileage * 0.002d);
                    this.OFP_NoDamageDirectHitsReceivedPoints = (this.NoDamageDirectHitsReceived * 0.3d);
                    this.OFP_SpottedPoints = (this.Spotted * 1.0d);
                    this.OFP_HitRatePoints = (hit_rate * 8.0d);
                    this.OFP_PenRatePoints = (pen_rate * 4.0d);
                    this.OFP_DamageRatioPoints = (damage_ratio * 8.0d);
                    this.OFP_VictoryPoints = ((this.Victory ? 1.0d : 0.0d));
                    this.OFP_PotentialDamagePoints = ((DeathReason == -1 ? this.PotentialDamageReceived * 0.003d : 0.0d));

                    this.OFP_CalculateMinMax();
                }
                return this.oldFantasyPoints.Value;
            }
        }
        public double OFP_KillsPoints { get; set; }
        public double OFP_DamagePoints { get; set; }
        public double OFP_DamageAssistedTrackPoints { get; set; }
        public double OFP_DamageAssistedRadioPoints { get; set; }
        public double OFP_DamagedPoints { get; set; }
        public double OFP_CapturePointsPoints { get; set; }
        public double OFP_DefensePointsPoints { get; set; }
        public double OFP_MilagePoints { get; set; }
        public double OFP_NoDamageDirectHitsReceivedPoints { get; set; }
        public double OFP_SpottedPoints { get; set; }
        public double OFP_HitRatePoints { get; set; }
        public double OFP_PenRatePoints { get; set; }
        public double OFP_DamageRatioPoints { get; set; }
        public double OFP_VictoryPoints { get; set; }
        public double OFP_PotentialDamagePoints { get; set; }
        public double OFP_Min { get; set; }
        public double OFP_Max { get; set; }

        public void OFP_CalculateMinMax()
        {
            var OFP_PointsList = new List<double>()
            {
                this.OFP_CapturePointsPoints,
                this.OFP_DamageAssistedRadioPoints,
                this.OFP_DamageAssistedTrackPoints,
                this.OFP_DamagedPoints,
                this.OFP_DamagePoints,
                this.OFP_DamageRatioPoints,
                this.OFP_DefensePointsPoints,
                this.OFP_HitRatePoints,
                this.OFP_KillsPoints,
                this.OFP_MilagePoints,
                this.OFP_NoDamageDirectHitsReceivedPoints,
                this.OFP_PenRatePoints,
                this.OFP_PotentialDamagePoints,
                this.OFP_SpottedPoints,
                this.OFP_VictoryPoints
            };

            this.OFP_Min = OFP_PointsList.Min();
            this.OFP_Max = OFP_PointsList.Max();
        }

        private const double EXPECTED_DAMAGE_MULTI = 21;

        private const double DAMAGE_DIFFICULTY_MOD_MULTI = 0.5;

        private const double EXPECTED_KILLS_MULTI = 5;

        private const double EXPECTED_SPOTS_MULTI = 14;

        private const double EXPECTED_PEN_RATE_MULTI = 14;

        private const double SPOTTING_ASSISTED_MULTI = 21;
        private const double TRACKING_ASSISTED_MULTI = 21;
        private const double STUN_ASSISTED_MULTI = 21;

        private const double METRIC_POINT_LIMIT = 84.0;
        private const double METRIC_SCALE_MULTI = 1.0;//2.755;
        private const double METRIC_COMMON_MULTI = 5.0;

        private double? newFantasyPoints = null;
        public double NewFantasyPoints
        {
            get
            {
                if (newFantasyPoints.HasValue == false)
                {
                    var players = this.Replay.Vehicles;
                    var sidePlayers = players.Where(p => p.Side == this.Side);
                    var otherSidePlayers = players.Where(p => p.Side != this.Side);

                    #region Define Variables

                    var maxHealth = (double)this.Tank.MaxHealth;
                    var maxHealthForExpected = maxHealth;

                    if (this.Tank.TypeTag == "SPG")
                        maxHealthForExpected = maxHealthForExpected * 3.5;

                    var maxTankTier = this.Replay.Vehicles.Max(p => p.Tank.Tier);

                    var expectedPenRate = 0.75 - ((maxTankTier - this.Tank.Tier) * 0.05);

                    if (expectedPenRate < 0.65)
                        expectedPenRate = 0.65;

                    var otherSideMaxHealth = (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth);
                    var sideCount = (double)sidePlayers.Count();
                    var otherSideCount = (double)otherSidePlayers.Count();


                    double sideKillsPercentOfMax = 0;
                    if (otherSideCount > 0)
                        sideKillsPercentOfMax = this.Kills / otherSideCount;

                    var killedByPlayer = otherSidePlayers.Where(p => p.KillerID == this.PreBattleID);

                    double playerKillTierTotal = 0;
                    foreach(var player in killedByPlayer)
                    {
                        playerKillTierTotal += player.Tank.Tier;
                    }

                    var sideDefenseTotal = (double)sidePlayers.Sum(p => p.DroppedCapturePoints);

                    var sideAverageLifetime = (double)sidePlayers.Where(p => p.AccountDBID != this.AccountDBID).Average(p => p.LifeTime);
                    double sideStdevLifetime = 0;
                    {
                        var count = (double)sidePlayers.Where(p => p.AccountDBID != this.AccountDBID).Count();
                        var sum = 0d;
                        foreach(var p in sidePlayers.Where(p => p.AccountDBID != this.AccountDBID))
                        {
                            sum += Math.Pow(p.LifeTime - sideAverageLifetime, 2);
                        }
                        sideStdevLifetime = Math.Sqrt(sum / (count - 1));
                    }

                    var expectedLifetimeMinimum = sideAverageLifetime - sideStdevLifetime;
                    var expectedLifetimeMaximum = sideAverageLifetime + sideStdevLifetime;

                    double playerPercentOfTeam = (double)Tank.Tier / (double)sidePlayers.Sum(p => p.Tank.Tier);

                    #endregion

                    #region Calculate Points

                    double killsPoints = 0;
                    double killsByTankLevel = this.Kills * this.Tank.Tier;
                    double adjustedDamage = this.DamageDealt;
                    double adjustedDamageLessSniper = adjustedDamage - (this.SniperDamageDealt * 0.5);

                    if (this.Tank.TypeTag == "SPG")
                        adjustedDamageLessSniper = adjustedDamage;

                    double playerTanksSpottedPoints = 0;

                    if (otherSideCount > 0 && this.Spotted > 0)
                    {
                        double adjustedMilage = (double)this.Mileage;
                        if (adjustedMilage > 4000)
                            adjustedMilage = 4000;
                        playerTanksSpottedPoints = NFP_CalculateLimitedPow(adjustedMilage / 4000.0) * NFP_CalculateLimitedPow(this.Spotted / otherSideCount / playerPercentOfTeam) * METRIC_COMMON_MULTI * 0.25 * METRIC_SCALE_MULTI;
                    }
                    if (playerTanksSpottedPoints > METRIC_POINT_LIMIT)
                        playerTanksSpottedPoints = METRIC_POINT_LIMIT;

                    double damageBlockedByArmorPoints = 0;

                    if (maxHealthForExpected > 0)
                    {
                        damageBlockedByArmorPoints = NFP_CalculateLimitedPow(this.DamageBlockedByArmor / maxHealthForExpected) * METRIC_COMMON_MULTI * 0.75 * METRIC_SCALE_MULTI;
                    }
                    if (damageBlockedByArmorPoints > METRIC_POINT_LIMIT)
                        damageBlockedByArmorPoints = METRIC_POINT_LIMIT;


                    double penRatePoints = 0;

                    if (expectedPenRate > 0)
                    {
                        penRatePoints = NFP_CalculateLimitedPow(this.PenRate / expectedPenRate) * NFP_CalculateLimitedPow(this.Piercings * 0.265) * METRIC_COMMON_MULTI * 0.5 * METRIC_SCALE_MULTI;
                    }

                    if (penRatePoints > METRIC_POINT_LIMIT)
                        penRatePoints = METRIC_POINT_LIMIT;

                    double defensePoints = 0;

                    if (this.DroppedCapturePoints > 0 && this.Victory == true)
                        defensePoints = (Math.Pow(2, (double)this.DroppedCapturePoints / sideDefenseTotal) - 1) * Math.Min(sideDefenseTotal, 100) * 0.15 * METRIC_SCALE_MULTI;
                    if (defensePoints > METRIC_POINT_LIMIT)
                        defensePoints = METRIC_POINT_LIMIT;


                    double damagePercentOfMax = adjustedDamage / (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth);

                    double damagePercentOfMaxPoints = NFP_CalculateLimitedPow(damagePercentOfMax / playerPercentOfTeam) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;
                    if (damagePercentOfMaxPoints > METRIC_POINT_LIMIT)
                        damagePercentOfMaxPoints = METRIC_POINT_LIMIT;

                    double damageToHealthRatio = adjustedDamageLessSniper / maxHealthForExpected;
                    double damageRatioPoints = NFP_CalculateLimitedPow(damageToHealthRatio) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;
                    if (damageRatioPoints > METRIC_POINT_LIMIT)
                        damageRatioPoints = METRIC_POINT_LIMIT;

                    double overallDamagePoints = damagePercentOfMaxPoints + damageRatioPoints;


                    double otherSideMaxHealthLessPlayerDamage = (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth) - this.DamageDealt;

                    double spottingAssistDamagePercentOfMax = (double)this.DamageAssistedRadio / otherSideMaxHealthLessPlayerDamage;
                    double spottedAssistedPoints = NFP_CalculateLimitedPow(spottingAssistDamagePercentOfMax / playerPercentOfTeam) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;
                    if (spottedAssistedPoints > METRIC_POINT_LIMIT)
                        spottedAssistedPoints = METRIC_POINT_LIMIT;

                    double trackingAssistDamagePercentOfMax = (double)this.DamageAssistedTrack / otherSideMaxHealthLessPlayerDamage;
                    double trackingAssistedPoints = NFP_CalculateLimitedPow(trackingAssistDamagePercentOfMax / playerPercentOfTeam) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;
                    if (trackingAssistedPoints > METRIC_POINT_LIMIT)
                        trackingAssistedPoints = METRIC_POINT_LIMIT;

                    double stunAssistDamagePercentOfMax = (double)this.DamageAssistedStun / otherSideMaxHealthLessPlayerDamage;
                    double stunAssistedPoints = NFP_CalculateLimitedPow(stunAssistDamagePercentOfMax / playerPercentOfTeam) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;
                    if (stunAssistedPoints > METRIC_POINT_LIMIT)
                        stunAssistedPoints = METRIC_POINT_LIMIT;



                    double killsToPlayersRatioPoints = NFP_CalculateLimitedPow(sideKillsPercentOfMax / playerPercentOfTeam) * NFP_CalculateLimitedPow(playerKillTierTotal / killsByTankLevel) * METRIC_COMMON_MULTI * 0.25 * METRIC_SCALE_MULTI;
                    if (killsToPlayersRatioPoints > METRIC_POINT_LIMIT)
                        killsToPlayersRatioPoints = METRIC_POINT_LIMIT;

                    killsPoints = CorrectNaN(killsToPlayersRatioPoints);

                    double playerLifetimePoints = 0;

                    var playerTimeBasedPoints =
                                                overallDamagePoints +
                                                spottedAssistedPoints +
                                                trackingAssistedPoints +
                                                stunAssistedPoints +
                                                damageBlockedByArmorPoints +
                                                killsPoints;

                    if((double)this.LifeTime < expectedLifetimeMinimum)
                    {
                        var lifetimePercentOfMinimum = (double)this.LifeTime / expectedLifetimeMinimum;

                        if (lifetimePercentOfMinimum < 0)
                            lifetimePercentOfMinimum = 0;

                        lifetimePercentOfMinimum = 1.0 - lifetimePercentOfMinimum;

                        if (lifetimePercentOfMinimum > 0.25)
                            lifetimePercentOfMinimum = 0.25;

                        playerLifetimePoints = playerTimeBasedPoints * lifetimePercentOfMinimum;
                    }
                    else if((double)this.LifeTime > expectedLifetimeMaximum)
                    {
                        var lifetimePercentOfMaximum = expectedLifetimeMaximum / (double)this.LifeTime;

                        if (lifetimePercentOfMaximum < 0)
                            lifetimePercentOfMaximum = 0;

                        lifetimePercentOfMaximum = 1.0 - lifetimePercentOfMaximum;

                        if (lifetimePercentOfMaximum > 0.25)
                            lifetimePercentOfMaximum = 0.25;

                        playerLifetimePoints = -(playerLifetimePoints * lifetimePercentOfMaximum);
                    }

                    #endregion


                    this.newFantasyPoints = Math.Round(
                        overallDamagePoints +
                        damageBlockedByArmorPoints +
                        spottedAssistedPoints +
                        trackingAssistedPoints +
                        stunAssistedPoints +
                        killsPoints  +
                        playerTanksSpottedPoints +
                        penRatePoints +
                        playerLifetimePoints +
                        defensePoints,
                        4)
                        ;

                    double fullDamageRatioPoints = NFP_CalculateLimitedPow(adjustedDamage / maxHealthForExpected) * METRIC_COMMON_MULTI * METRIC_SCALE_MULTI;

                    this.NFP_DamageBlockedByArmorPoints = damageBlockedByArmorPoints;

                    this.NFP_DamagePoints = damagePercentOfMaxPoints + damageRatioPoints;
                    this.NFP_SpottedAssistedPoints = spottedAssistedPoints;
                    this.NFP_TrackAssistedPoints = trackingAssistedPoints;
                    this.NFP_StunAssistedPoints = stunAssistedPoints;

                    this.NFP_SniperDamagePoints = (damageRatioPoints - fullDamageRatioPoints);

                    this.NFP_KillsPoints = killsPoints;
                    this.NFP_SpottedPoints = playerTanksSpottedPoints;
                    this.NFP_PenRatePoints = penRatePoints;
                    this.NFP_PlayerLifetimePoints = playerLifetimePoints;
                    
                    this.NFP_DefensePointsPoints = defensePoints;

                    this.NFP_CalculateMinMax();
                    this.NFP_SideDamagePercentOfOtherSideTotalHealth = (this.DamageDealt / (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth)) / (maxHealthForExpected / (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth));
                    this.NFP_SideDamagePercentOfMax = this.DamageDealt / (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth);
                    this.NFP_OtherSideMaxHealthPercent = maxHealthForExpected / (double)otherSidePlayers.Sum(p => p.Tank.MaxHealth);
                    this.NFP_SideDamagePercentOfMaxPoints = damagePercentOfMaxPoints;
                    this.NFP_DamageRatioPoints = damageRatioPoints;

                    this.NFP_KillsTierVsPlayerTier = playerKillTierTotal / killsByTankLevel;
                    this.NFP_KillsToPlayersRatio = sideKillsPercentOfMax;
                    this.NFP_KillsToPlayersRatioPoints = killsToPlayersRatioPoints;

                    this.NFP_ExpectedPenRate = expectedPenRate;
                    this.NFP_PenRateExpectedPenRateRatio = (this.PenRate / expectedPenRate);

                    this.NFP_PlayerLifetimeTeamAverage = sideAverageLifetime;
                    this.NFP_PlayerLifetimeMinRange = expectedLifetimeMinimum;
                    this.NFP_PlayerLifetimeMaxRange = expectedLifetimeMaximum;
                }
                return this.newFantasyPoints.Value;
            }
        }

        private const double NFP_LIMITED_POW_CUTOFF = 3;

        private double NFP_CalculateLimitedPow(double ratio)
        {
            if(ratio > NFP_LIMITED_POW_CUTOFF)
            {
                return Math.Pow(NFP_LIMITED_POW_CUTOFF, 1.262) + ((ratio - NFP_LIMITED_POW_CUTOFF) * 1.3334);
            } else
            {
                return Math.Pow(ratio, 1.262);
            }
        }

        public double NFP_DamagePoints { get; set; }
        public double NFP_SniperDamagePoints { get; set; }
        public double NFP_SpottedAssistedPoints { get; set; }
        public double NFP_TrackAssistedPoints { get; set; }
        public double NFP_DamagedPoints { get; set; }
        public double NFP_KillsPoints { get; set; }
        public double NFP_SpottedPoints { get; set; }
        public double NFP_PenRatePoints { get; set; }
        public double NFP_DamageBlockedByArmorPoints { get; set; }
        public double NFP_PlayerLifetimePoints { get; set; }
        
        public double NFP_DefensePointsPoints { get; set; }
        public double NFP_StunAssistedPoints { get; set; }

        public double NFP_Min { get; set; }
        public double NFP_Max { get; set; }

        public double NFP_SideDamagePercent { get; set; }
        public double NFP_SideTotalDamage { get; set; }
        public double NFP_SideMaxHealthPercent { get; set; }
        public double NFP_SideTotalHealth { get; set; }
        public double NFP_OtherSideTotalHealth { get; set; }

        public double NFP_SideDamagePercentOfOtherSideTotalHealth { get; set; }
        public double NFP_SideDamagePercentOfMax { get; set; }
        public double NFP_OtherSideMaxHealthPercent { get; set; }
        public double NFP_SideDamagePercentOfMaxPoints { get; set; }
        public double NFP_DamageRatioPoints { get; set; }

        public double NFP_KillsTierVsPlayerTier { get; set; }
        public double NFP_KillsToPlayersRatio { get; set; }
        public double NFP_KillsTierVsPlayerTierPoints { get; set; }
        public double NFP_KillsToPlayersRatioPoints { get; set; }

        public double NFP_ExpectedPenRate { get; set; }
        public double NFP_PenRateExpectedPenRateRatio { get; set; }

        public double NFP_PlayerLifetimeTeamAverage { get; set; }
        public double NFP_PlayerLifetimeMinRange { get; set; }
        public double NFP_PlayerLifetimeMaxRange { get; set; }

        public void NFP_CalculateMinMax()
        {
            var NFP_PointsList = new List<double>()
            {
                this.NFP_DamageBlockedByArmorPoints,
                this.NFP_DamagedPoints,
                this.NFP_DamagePoints,
                this.NFP_SniperDamagePoints,
                this.NFP_DefensePointsPoints,
                this.NFP_KillsPoints,
                this.NFP_PenRatePoints,
                this.NFP_PlayerLifetimePoints,
                this.NFP_SpottedAssistedPoints,
                this.NFP_SpottedPoints,
                this.NFP_StunAssistedPoints,
                this.NFP_TrackAssistedPoints
            };

            this.NFP_Min = NFP_PointsList.Min();
            this.NFP_Max = NFP_PointsList.Max();
        }

        [JsonConstructor]
        public ReplayPlayer(
            int accountDBID,
            int team,

            int spotted,

            int damaged,
            int damageDealt,
            int sniperDamageDealt,
            int damageAssistedTrack,
            int damageAssistedRadio,

            int damageReceived,
            int potentialDamageReceived,
            int damageBlockedByArmor,

            int kills,

            int health,

            int capturePoints,
            int droppedCapturePoints,

            int shots,
            int directHits,
            int piercings,
            int explosionHits,

            int piercingsReceived,
            int directHitsReceived,
            int explosionHitsReceived,
            int noDamageDirectHitsReceived,

            int deathReason,
            int killerID,

            int mileage,
            int lifeTime,

            int tDamageDealt,
            int tKills,

            int stunned,
            int damageAssistedStun,
            double stunDuration,
            int stunNum,

            int xp
        )
        {
            this.AccountDBID = accountDBID;
            this.Side = team;

            this.Spotted = spotted;

            this.Damaged = damaged;
            this.DamageDealt = damageDealt;
            this.SniperDamageDealt = sniperDamageDealt;
            this.DamageAssistedTrack = damageAssistedTrack;
            this.DamageAssistedRadio = damageAssistedRadio;

            this.DamageReceived = damageReceived;
            this.PotentialDamageReceived = potentialDamageReceived;
            this.DamageBlockedByArmor = damageBlockedByArmor;

            this.Kills = kills;

            this.Health = health;

            this.CapturePoints = capturePoints;
            this.DroppedCapturePoints = droppedCapturePoints;

            this.Shots = shots;
            this.DirectHits = directHits;
            this.Piercings = piercings;
            this.ExplosionHits = explosionHits;

            this.PiercingsReceived = piercingsReceived;
            this.DirectHitsReceived = directHitsReceived;
            this.ExplosionHitsReceived = explosionHitsReceived;
            this.NoDamageDirectHitsReceived = noDamageDirectHitsReceived;

            this.DeathReason = deathReason;
            this.KillerID = killerID;

            this.Mileage = mileage;
            this.LifeTime = lifeTime;

            this.TeamDamageDealt = tDamageDealt;
            this.TeamKills = tKills;

            this.StunnedTanks = stunned;
            this.StunDuration = stunDuration;
            this.StunTimes = stunNum;
            this.DamageAssistedStun = damageAssistedStun;

            this.XP = xp;
        }

        internal void SetPlayerItem(Replay.PlayerItem item)
        {
            this.AccountName = item.Name;
            this.TankTag = item.VehicleType;
        }

        internal void SetPlayerBattleID(string playerBattleID)
        {
            int result;
            if (int.TryParse(playerBattleID, out result))
                this.PreBattleID = result;
        }

        internal void SetVictory(bool victory)
        {
            this.Victory = victory;
        }

        public void SetReplay(Replay replay)
        {
            this.Replay = replay;
        }

        public void SetFantasyPoints(double oldFantasyPoints, double newFantasyPoints)
        {
            this.oldFantasyPoints = oldFantasyPoints;
            this.newFantasyPoints = newFantasyPoints;
        }

        private Dictionary<int, Dictionary<string, double>> GetStatsBySide()
        {
            var statsBySide = new Dictionary<int, Dictionary<string, double>>()
                {
                    { 1, new Dictionary<string, double>() },
                    { 2, new Dictionary<string, double>() }
                };

            foreach (var side in statsBySide.Values)
            {
                side["maxHealth"] = 0;
                side["damage"] = 0;
                side["kills"] = 0;
                side["tracking_assisted"] = 0;
                side["spotting_assisted"] = 0;
                side["players"] = 0;
            }

            foreach (var vehicle in this.Replay.Vehicles)
            {
                statsBySide[vehicle.Side]["maxHealth"] += vehicle.Tank.MaxHealth.Value;
                statsBySide[vehicle.Side]["damage"] += vehicle.DamageDealt;
                statsBySide[vehicle.Side]["kills"] += vehicle.Kills;
                statsBySide[vehicle.Side]["tracking_assisted"] += vehicle.DamageAssistedTrack;
                statsBySide[vehicle.Side]["spotting_assisted"] += vehicle.DamageAssistedRadio;
                statsBySide[vehicle.Side]["players"] += 1;
            }

            return statsBySide;
        }

        private double CorrectNaN(double i)
        {
            if (double.IsNaN(i) || double.IsInfinity(i))
                return 0d;
            else
                return i;
        }
    }
}
