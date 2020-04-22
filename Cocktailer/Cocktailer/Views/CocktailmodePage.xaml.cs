
using CarouselView.FormsPlugin.Abstractions;
using Cocktailer.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "setLandscapeMode");
        }

        protected override void OnDisappearing()
        {
            ViewModel.CloseBluetoothConnection();
            MessagingCenter.Send(this, "setPortraitMode");
            base.OnDisappearing();
        }
    }
}