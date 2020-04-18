using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Recipes
{
    public class NewRecipeViewModel : BaseValidationViewModel
    {
        public static ObservableCollection<string> AvailableDrinks { get; set; }
        private double percentage;
        public double Percentage
        {
            get => percentage;
            set 
            { 
                percentage = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                Validate(() => !string.IsNullOrEmpty(name), "Name darf nicht leer sein");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        private ObservableCollection<Ingredient> ingredients;
        public ObservableCollection<Ingredient> Ingredients
        {
            get => ingredients;
            set
            {
                ingredients = value;
                Validate(() => ingredients.Count() > 0, "Zutatenliste darf nicht leer sein");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        private IMemoryService memService;
        IAlertMessageService alertService;
        public NewRecipeViewModel(INavService navService, IMemoryService memoryService,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService;
            memService = memoryService;
        }

        public override async void Init()
        {
            Ingredients = new ObservableCollection<Ingredient>();
            AvailableDrinks = new ObservableCollection<string>((await memService
                .GetAvailable<DrinkEntry>())
                .Select(x => x.Brand + "/" + x.Name + "," + x.Percentage + "%").ToList());
        }
        private void CalculatePercentage()
        {
            var totalAmount = Ingredients.Select(x => x.Amount).Sum();
            var totalAlcohol = Ingredients.Select(x => x.Drink.Percentage * x.Amount).Sum();
            Percentage = totalAlcohol / totalAmount;
        }
        Command saveCommand;
        public Command SaveCommand =>saveCommand ?? 
            (saveCommand = new Command(async () => await SaveRecipe(), CanSave));

        bool CanSave () => !string.IsNullOrEmpty(Name) && ingredients.Count > 0 && !HasErrors;
        async Task SaveRecipe()
        {
            if (Ingredients.Where(x => x.Drink == null).Any())
            {
                await Application.Current.MainPage.
                    DisplayAlert("Fehler beim Speichern",
                    "Kann nicht speichern wenn ein Getränk leer ist", "OK");
                return;
            }                
            CalculatePercentage();
            var newEntry = new RecipeEntry()
            {
                Name = Name,
                Ingredients = Ingredients.ToList(),
                Percentage = Percentage
            };
            try
            {
                IsBusy = true;
                await memService.Save(newEntry, Name);
                
                NavService.ClearBackStack();
                await NavService.NavigateTo<MainViewModel>();
                await NavService.NavigateTo<RecipesViewModel>();
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }
            finally
            {
                IsBusy = false;
            }
        }
        Command addIngredientCommand;
        public Command AddIngredientCommand => addIngredientCommand ??
            (addIngredientCommand = new Command( () => AddEmptyIngredient()));

        private void AddEmptyIngredient()
        {
            var ing = Ingredients;
            ing.Add(new Ingredient()
            {
                Drink = new DrinkEntry(),
                Amount = 0
            });
            Ingredients = ing;
           
        }

        private Command deleteIngredientCommand;
        public Command DeleteIngredientCommand => deleteIngredientCommand ??
            (deleteIngredientCommand = new Command(() => DeleteIngredient()));
        private void DeleteIngredient()
        {
            var ing = Ingredients;
            ing.RemoveAt(ing.Count - 1);
            Ingredients = ing;
        }
    }
}
