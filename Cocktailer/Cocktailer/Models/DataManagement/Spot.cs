using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class Spot
    {
        public int X;
        public int Y;
        public DrinkEntry Drink;
        public int Amount;

        public Spot(int x, int y, DrinkEntry drink)
        {
            X = x;
            Y = y;
            Drink = drink;
        }
        public Spot(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
