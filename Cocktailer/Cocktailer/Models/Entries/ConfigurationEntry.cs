using System.Collections.Generic;
using System.Linq;

namespace Cocktailer.Models.Entries
{
    public class ConfigurationEntry : IEntry
    {
        public List<Spot> Spots { get; set; }
        public string Name { get; set; }

        private string drinkString;
        public string DrinkString
        {
            get
            {
                try
                {
                    var drinkNames = Spots.Select(x => $"{x.Drink.Brand} {x.Drink.Name}")
                        .Where(x => !string.IsNullOrEmpty(x));
                    drinkString = $"{string.Join(", ", drinkNames.Take(3))}, ...";
                    return drinkString;
                }
                catch
                {
                    return "";
                }
                
            }
            set { drinkString = value; }
        }

        public ConfigurationEntry()
        {
        }

        public ConfigurationEntry(string name)
        {
            Name = name;
            Spots = new List<Spot>();
        }

        public ConfigurationEntry(string name, List<Spot> spots)
        {
            Spots = spots;
        }
    }
}
