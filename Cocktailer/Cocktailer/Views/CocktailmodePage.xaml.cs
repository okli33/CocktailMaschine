using Cocktailer.Models.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CocktailmodePage : ContentPage
    {
        public CocktailmodePage()
        {
            InitializeComponent();

            var items = new List<RecipeEntry>()
            {
                new RecipeEntry()
                {
                    Name = "Sex on the Beach",
                    Ingredients = "Vodka, Orangensaft, Cranberrysaft",
                    Percentage = 5.9
                },
                new RecipeEntry()
                {
                    Name = "Rum Cola",
                    Ingredients = "Rum Cola",
                    Percentage = 16
                },
                new RecipeEntry()
                {
                    Name = "Mojito",
                    Ingredients = "Rum, Zitronensaft",
                    Percentage = 13
                },
            };

            cocktails.ItemsSource = items;
        }
    }
}