using System.Collections.Generic;

namespace Cocktailer.Models.Entries
{
    public class SpotMaker
    {
        public static Spot MakeSpot(int x, int y, DrinkEntry drink = null)
        {
            if (drink == null)
                return new Spot(x, y);
            return new Spot(x, y, drink);
        }

        public static List<Spot> CreateSpotList(int width, int height)
        {
            var spotlist = new List<Spot>();
            for (int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++)
                {
                    spotlist.Add(MakeSpot(x, y, null));
                }
            }
            return spotlist;
        } 
    }
}
