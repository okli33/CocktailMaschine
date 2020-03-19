using Cocktailer.Models.DataManagement;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class Spot
    {
        public int X;
        public int Y;
        public Drink Drink;
        public int Amount;

        public Spot(int x, int y, Drink drink)
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
