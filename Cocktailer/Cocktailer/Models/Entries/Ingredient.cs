namespace Cocktailer.Models.Entries 
{ 
    public class Ingredient
    {
        public int Amount { get; set; }
        public DrinkEntry Drink { get; set; }
    }
}
