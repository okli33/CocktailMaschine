using Cocktailer.Services;
using Cocktailer.ViewModels;
using Cocktailer.ViewModels.Configurations;
using Cocktailer.ViewModels.DataExchange;
using Cocktailer.ViewModels.Recipes;
using Cocktailer.Views;
using Cocktailer.Views.Configurations;
using Cocktailer.Views.DataExchange;
using Cocktailer.Views.Recipes;
using Ninject.Modules;

namespace Cocktailer.Modules
{
    public class CocktailerNavModule : NinjectModule
    {
        public override void Load()
        {
            var navService = new XamarinFormsNavService();
            navService.RegisterViewMapping(typeof(MainViewModel), typeof(MainPage));

            navService.RegisterViewMapping(typeof(CocktailModeViewModel), typeof(CocktailmodePage));
            navService.RegisterViewMapping(typeof(SelectConfigurationViewModel), typeof(SelectConfigurationView));

            navService.RegisterViewMapping(typeof(DrinksViewModel), typeof(DrinksPage));
            navService.RegisterViewMapping(typeof(DrinkDetailViewModel), typeof(DrinkDetailPage));
            navService.RegisterViewMapping(typeof(NewDrinkViewModel), typeof(NewDrinkPage));
            navService.RegisterViewMapping(typeof(EditDrinkViewModel), typeof(EditDrinkPage));
            
            navService.RegisterViewMapping(typeof(ConfigurationsViewModel), typeof(ConfigurationsPage));
            navService.RegisterViewMapping(typeof(ConfigurationDetailViewModel), typeof(ConfigurationDetailPage));
            navService.RegisterViewMapping(typeof(NewConfigurationViewModel), typeof(NewConfigurationPage));
            navService.RegisterViewMapping(typeof(EditConfigurationViewModel), typeof(EditConfigurationPage));
            
            navService.RegisterViewMapping(typeof(RecipesViewModel), typeof(RecipesPage));
            navService.RegisterViewMapping(typeof(NewRecipeViewModel), typeof(NewRecipePage));
            navService.RegisterViewMapping(typeof(RecipeDetailViewModel), typeof(RecipeDetailPage));
            navService.RegisterViewMapping(typeof(EditRecipeViewModel), typeof(EditRecipePage));

            navService.RegisterViewMapping(typeof(ModeSelectViewModel), typeof(ModeSelectPage));
            navService.RegisterViewMapping(typeof(LogViewModel), typeof(LogView));
            navService.RegisterViewMapping(typeof(ExportViewModel), typeof(ExportView));
            navService.RegisterViewMapping(typeof(ImportViewModel), typeof(ImportView));

            navService.RegisterViewMapping(typeof(BluetoothViewModel), typeof(BluetoothView));

            Bind<INavService>().ToMethod(x => navService).InSingletonScope();
        }
    }
}
