using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.MemoryManagement;

namespace Cocktailer.Models.DataManagement
{
    public class Drink : IAmSaveable
    {
        public string Name;
        public string Brand;
        public Drink (string name, string brand)
        {
            Name = name;
            Brand = brand;
        }
        public Drink(string name)
        {
            Name = name;
        }
    }
}
