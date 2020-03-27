using Cocktailer.Services;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(INavService navService) : base(navService) { }

        public Command SuffCommand => new Command(async () => await NavService
            .NavigateTo<CocktailModeViewModel>());
        public Command RecipesCommand => new Command(async () => await NavService
            .NavigateTo<DrinksViewModel>());
        public Command ConfigurationsCommand => new Command(async () => await NavService
            .NavigateTo<DrinksViewModel>());
        public Command DrinksCommand => new Command(async () =>await NavService
            .NavigateTo<DrinksViewModel>());
    }
}
