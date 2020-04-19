using Cocktailer.ViewModels.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views.Recipes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRecipePage : ContentPage
    {
        NewRecipeViewModel ViewModel => BindingContext as NewRecipeViewModel;
        public NewRecipePage()
        {
            InitializeComponent();
        }
    }
}