using Cocktailer.Models.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocktailer.Models.Entries
{
    public class RecipeEntry : IEntry
    {
        public string Name { get; set; }
        public double Percentage { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
