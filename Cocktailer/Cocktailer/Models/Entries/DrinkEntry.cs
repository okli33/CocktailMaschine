using Cocktailer.Models.DataManagement;

namespace Cocktailer.Models.Entries
{
    public class DrinkEntry : IAmSaveable
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
    }
}
