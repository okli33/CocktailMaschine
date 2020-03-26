using Cocktailer.Services;
using Cocktailer.ViewModels;
using Cocktailer.Views;
using Ninject.Modules;

namespace Cocktailer.Modules
{
    public class CocktailerNavModule : NinjectModule
    {
        public override void Load()
        {
            var navService = new XamarinFormsNavService();
            navService.RegisterViewMapping(typeof(MainViewModel), typeof(MainPage));
            navService.RegisterViewMapping(typeof(DrinksViewModel), typeof(DrinksPage));
            navService.RegisterViewMapping(typeof(DrinkDetailViewModel), typeof(DrinkDetailPage));
            navService.RegisterViewMapping(typeof(NewDrinkViewModel), typeof(NewDrinkPage));

            Bind<INavService>().ToMethod(x => navService).InSingletonScope();
        }
    }
}
