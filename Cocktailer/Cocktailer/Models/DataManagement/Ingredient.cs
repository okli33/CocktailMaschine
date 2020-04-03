using Cocktailer.Models.Entries;
using System.Collections.Generic;
using System.Linq;

namespace Cocktailer.Models.DataManagement
{
    public class Ingredient
    {
        public int Amount { get; set; }
        public DrinkEntry Drink { get; set; }
    }
}
