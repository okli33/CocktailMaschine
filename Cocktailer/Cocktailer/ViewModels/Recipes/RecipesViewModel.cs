using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Recipes
{
    public class RecipesViewModel : BaseViewModel
    { 
        private ObservableCollection<RecipeEntry> recipeEntries;
        public ObservableCollection<RecipeEntry> RecipeEntries
        {
            get => recipeEntries; set
            {
                recipeEntries = value;
                OnPropertyChanged();
            }
        }
        IMemoryService memService;
        public RecipesViewModel(INavService navService, IMemoryService mem) : base(navService)
        {
            memService = mem;
        }

        public override async void Init()
        {
            await LoadRecipes();
        }

        public async Task LoadRecipes()
        {
            IsBusy = true;
            try
            {
                RecipeEntries = new ObservableCollection<RecipeEntry>(
                    await memService.GetAvailable<RecipeEntry>());
            }
            catch
            {
                RecipeEntries = new ObservableCollection<RecipeEntry>();
            }
            finally
            {
                IsBusy = false;
            }
        } 

        public Command RefreshCommand => new Command(async () =>
           await LoadRecipes());

        public Command DetailCommand => new Command<RecipeEntry>(async (entry) =>
            await NavService.NavigateTo<RecipeDetailViewModel, RecipeEntry>(entry));

        public Command NewCommand => new Command(async () =>
           await NavService.NavigateTo<NewRecipeViewModel>());
    }
}
