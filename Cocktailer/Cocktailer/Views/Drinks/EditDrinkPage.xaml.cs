using Cocktailer.Models.Entries;
using Cocktailer.ViewModels;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDrinkPage : ContentPage
    {
        EditDrinkViewModel ViewModel => BindingContext as EditDrinkViewModel;
        public EditDrinkPage()
        {
            InitializeComponent();
        }
    }
}