using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;
using System.Collections.Generic;
using System.Linq;

namespace Cocktailer.Models.ConfigurationManagement
{
    public class SpotMaker
    {
        public static Spot MakeSpot(int x, int y, DrinkEntry drink = null, 
            List<DrinkEntry> drinkEntries = null)
        {
            if (drink == null)
                return new Spot(x, y) { DrinkEntries = drinkEntries.Select(val => val.ToString()).ToList()};
            return new Spot(x, y, drink) { DrinkEntries = drinkEntries.Select(val => val.ToString()).ToList() };
        }

        public static List<Spot> CreateSpotList(int width, int height, List<DrinkEntry> drinks)
        {
            var spotlist = new List<Spot>();
            for (int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++)
                {
                    spotlist.Add(MakeSpot(x, y, null, drinks));
                }
            }
            return spotlist;
        } 
    }
}
