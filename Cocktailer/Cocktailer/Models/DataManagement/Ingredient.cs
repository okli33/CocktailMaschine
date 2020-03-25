namespace Cocktailer.Models.DataManagement
{
    public class Ingredient : IAmSaveable
    {
        public int Amount;
        public Drink Drink;

        public Ingredient(string name, int amount)
        {
            Amount = amount;
            Drink = new Drink(name);
        }
        public Ingredient(string name, int amount, string brand)
        {
            Drink = new Drink(name, brand);           
            amount = Amount;
        }
        public Ingredient (Drink drink, int amount)
        {
            Amount = amount;
            Drink = drink;
        }

    }
}
