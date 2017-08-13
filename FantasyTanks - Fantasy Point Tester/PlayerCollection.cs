using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTanks___Fantasy_Point_Tester
{
    public class PlayerCollection
    {
        public string Title { get; }
        public IReadOnlyList<ReplayPlayer> Vehicles { get; }

        public OverallData Side1Overall { get; }
        public OverallData Side2Overall { get; }
        public OverallData Overall { get; }

        public PlayerCollection(Replay replay)
        {
            this.Vehicles = replay.Vehicles;
            this.Title = replay.Path;
            this.Side1Overall = new OverallData(this.Vehicles.Where(v => v.Side == 1));
            this.Side2Overall = new OverallData(this.Vehicles.Where(v => v.Side == 2));
            this.Overall = new OverallData(this.Vehicles);
        }

        public PlayerCollection(string title, IReadOnlyList<ReplayPlayer> vehicles)
        {
            this.Title = title;
            this.Vehicles = vehicles;
            this.Overall = new OverallData(this.Vehicles);
        }

        public class OverallData
        {
            public double OFP_Total { get; }
            public double OFP_Average { get; }
            public double NFP_Total { get; }
            public double NFP_Average { get; }

            public OverallData(IEnumerable<ReplayPlayer> vehicles)
            {
                this.OFP_Total = vehicles.Sum(v => v.OldFantasyPoints);
                this.OFP_Average = vehicles.Average(v => v.OldFantasyPoints);
                this.NFP_Total = vehicles.Sum(v => Math.Max(v.NewFantasyPoints, 0));
                this.NFP_Average = vehicles.Average(v => Math.Max(v.NewFantasyPoints, 0));
            }
        }
    }
}
