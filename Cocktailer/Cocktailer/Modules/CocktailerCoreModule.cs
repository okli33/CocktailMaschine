using Cocktailer.Services;
using Cocktailer.ViewModels;
using Cocktailer.ViewModels.Configurations;
using Cocktailer.ViewModels.Recipes;
using Cocktailer.Views.Configurations;
using Ninject.Modules;

namespace Cocktailer.Modules
{
    public class CocktailerCoreModule : NinjectModule
    {
        public override void Load()
        {
            IAlertMessageService alertMessageService = new AlertMessageService();
            Bind<MainViewModel>().ToSelf();
            Bind<DrinkList>().ToSelf().InSingletonScope();

            Bind<DrinksViewModel>().ToSelf();
            Bind<NewDrinkViewModel>().ToSelf();
            Bind<DrinkDetailViewModel>().ToSelf();
            Bind<EditDrinkViewModel>().ToSelf();

            Bind<RecipeDetailViewModel>().ToSelf();
            Bind<RecipesViewModel>().ToSelf();
            Bind<NewRecipeViewModel>().ToSelf();
            Bind<EditRecipeViewModel>().ToSelf();

            Bind<ConfigurationDetailViewModel>().ToSelf();
            Bind<ConfigurationsViewModel>().ToSelf();
            Bind<EditConfigurationViewModel>().ToSelf();
            Bind<NewConfigurationViewModel>().ToSelf();

            Bind<CocktailModeViewModel>().ToSelf();
            Bind<SelectConfigurationViewModel>().ToSelf();

            Bind<IAlertMessageService>().ToMethod(x => alertMessageService).InSingletonScope();
        }
        

    }
}
