using System.Collections.Generic;
using System.Linq;

namespace Cocktailer.Models.Entries
{
    public class RecipeEntry : IEntry
    {
        public string Name { get; set; }
        public double Percentage { get; set; }
        public string NameString
        {
            get => $"{Name} ({Percentage}%)";
        }
        public List<Ingredient> Ingredients { get; set; }
        public override string ToString()
        {
            return Name + ":" + string
                .Concat(Ingredients.Select(x => x.Drink.ToString() + "," + x.Amount.ToString() + ";"));
        }
    }
}
