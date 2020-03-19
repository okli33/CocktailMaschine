using Cocktailer.Models.DataManagement;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class SpotMaker
    {
        public static Spot MakeSpot(int x, int y, Drink drink = null)
        {
            if (drink == null)
                return new Spot(x, y);
            return new Spot(x, y, drink);
        }
    }
}
