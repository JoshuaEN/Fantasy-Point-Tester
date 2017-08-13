using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTanks___Fantasy_Point_Tester
{
    public class Tank
    {
        public int ID { get; }
        public string Tag { get; }
        public string Name { get; }
        public string ShortName { get; }

        public int Tier { get; }

        public string Nation { get; }
        public string NationTag { get; }

        public string Type { get; }
        public string TypeTag { get; }

        public int? MaxHealth { get; }

        public Tank(int id, string tag, string name, string short_name, int level, string nation_name, string nation_tag, string type_name, string type_tag, List<int> maxHealths)
        {
            this.ID = id;
            this.Tag = tag;
            this.Name = name;
            this.ShortName = short_name;

            this.Tier = level;

            this.Nation = nation_name;
            this.NationTag = nation_tag;

            this.Type = type_name;
            this.TypeTag = type_tag;

            this.MaxHealth = maxHealths?.Max();
        }
    }
}
