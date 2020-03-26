using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class SpotMaker
    {
        public static Spot MakeSpot(int x, int y, DrinkEntry drink = null)
        {
            if (drink == null)
                return new Spot(x, y);
            return new Spot(x, y, drink);
        }
    }
}
