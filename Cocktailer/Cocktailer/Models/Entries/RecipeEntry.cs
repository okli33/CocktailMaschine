using Cocktailer.Models.DataManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.Entries
{
    public class RecipeEntry
    {
        public string Name { get; set; }
        public double Percentage { get; set; }
        public List<Ingredient> Ingredients { get; set; } 
    }
}
