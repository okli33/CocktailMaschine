using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktailer.Models.DataManagement
{
    public class Recipe : IAmSaveable
    {
        public string Name;
        public List<Ingredient> Ingredients;

        public Recipe(List<Ingredient> ingredients, string name)
        {
            Name = name;
            Ingredients = ingredients;
        }
        public Recipe() { }

    }
}
