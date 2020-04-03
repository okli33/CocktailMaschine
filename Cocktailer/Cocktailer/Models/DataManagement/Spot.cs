using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;
using System.Collections.Generic;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class Spot
    {
        public int[] Position { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DrinkEntry Drink { get; set; }
        public int Amount { get; set; }

        public Spot(int x, int y, DrinkEntry drink)
        {
            X = x;
            Y = y;
            Position = new int[] { X, Y };
            Drink = drink;
        }
        public Spot(int x, int y)
        {
            X = x;
            Y = y;
            Position = new int[] { X, Y };
        }

        public Spot()
        {
        }
    }
}
