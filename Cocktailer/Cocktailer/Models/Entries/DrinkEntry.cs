using Cocktailer.Models.DataManagement;

namespace Cocktailer.Models.Entries
{
    public class DrinkEntry : IEntry
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }

        public override string ToString()
        {
            return $"{Brand}/{Name}, {Percentage}%"; 
        }
    }
}
