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
    public partial class EditRecipePage : ContentPage
    {
        EditRecipeViewModel ViewModel => BindingContext as EditRecipeViewModel;
        public EditRecipePage()
        {
            InitializeComponent();

        }
    }
}