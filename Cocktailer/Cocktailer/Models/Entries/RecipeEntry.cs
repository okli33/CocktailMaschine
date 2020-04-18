using System.Collections.Generic;

namespace Cocktailer.Models.Entries
{
    public class RecipeEntry : IEntry
    {
        public string Name { get; set; }
        public double Percentage { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
