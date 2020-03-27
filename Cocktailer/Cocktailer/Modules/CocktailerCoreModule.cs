using Cocktailer.ViewModels;
using Ninject.Modules;

namespace Cocktailer.Modules
{
    public class CocktailerCoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<MainViewModel>().ToSelf();
            Bind<DrinksViewModel>().ToSelf();
            Bind<NewDrinkViewModel>().ToSelf();
            Bind<DrinkDetailViewModel>().ToSelf();
            Bind<CocktailModeViewModel>().ToSelf();
        }
        

    }
}
