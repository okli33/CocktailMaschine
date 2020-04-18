
using Cocktailer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cocktailer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CocktailmodePage : ContentPage
    {
        CocktailModeViewModel ViewModel => BindingContext as CocktailModeViewModel;
        public CocktailmodePage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            ViewModel.CloseBluetoothConnection();
            base.OnDisappearing();
        }
    }
}