using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrinksPage : ContentPage
    {
        public DrinksPage()
        {
            InitializeComponent();
            BindingContext = new DrinksViewModel(DependencyService.Get<INavService>());
        }      
    }
}