using Cocktailer.Models.Entries;

namespace Cocktailer.Models.DataManagement
{
    public class Ingredient : IAmSaveable
    {
        public int Amount;
        public DrinkEntry Drink;

        public Ingredient() { }
        public Ingredient(string name, int amount)
        {
            Amount = amount;
            Drink = new DrinkEntry { Name = name };
        }
        public Ingredient(string name, int amount, string brand)
        {
            Drink = new DrinkEntry { Name = name, Brand = brand };           
            amount = Amount;
        }
        public Ingredient (DrinkEntry drink, int amount)
        {
            Amount = amount;
            Drink = drink;
        }

    }
}
