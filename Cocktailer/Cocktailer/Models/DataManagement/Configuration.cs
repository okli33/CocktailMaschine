using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.MemoryManagement;
using System.Collections.Generic;

namespace Cocktailer.Models.DataManagement
{
    public class Configuration : IAmSaveable
    {
        public List<Spot> Spots;
        public string Name;

        public Configuration(string name)
        {
            Name = name;
            Spots = new List<Spot>();
        }

        public Configuration(string name, List<Spot> spots)
        {
            Spots = spots;
        }
    }
}
