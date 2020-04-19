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
        IAlertMessageService alertService;
        public RecipesViewModel(INavService navService, IMemoryService mem,
            IAlertMessageService alertService) : base(navService)
        {
            memService = mem;
            this.alertService = alertService;
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

        public Command DeleteSingleCommand => new Command(async (value) => await
            DeleteSingle((RecipeEntry)value));
        private async Task DeleteSingle(RecipeEntry entry)
        {
            if (!await memService.Delete<RecipeEntry>(entry.Name))
            {
                await alertService.ShowErrorMessage($"Fehler beim Löschen von {entry.Name}");
            }
            await LoadRecipes();
        }
    }
}
