using Cocktailer.Services;
using Cocktailer.ViewModels;
using Cocktailer.Views;
using Xamarin.Forms;

namespace Cocktailer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            var mainPage = new NavigationPage(new MainPage());
            var navService = DependencyService.Get<INavService>() as XamarinFormsNavService;

            navService.XamarinFormsNav = mainPage.Navigation;
            navService.RegisterViewMapping(typeof(MainViewModel), typeof(MainPage));
            navService.RegisterViewMapping(typeof(DrinksViewModel), typeof(DrinksPage));
            navService.RegisterViewMapping(typeof(DrinkDetailViewModel), typeof(DrinkDetailPage));
            navService.RegisterViewMapping(typeof(NewDrinkViewModel), typeof(NewDrinkPage));

            MainPage = mainPage;

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
