using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Recipes
{
    public class EditRecipeViewModel : BaseViewModel<RecipeEntry>
    {
        public static int GetIndex(Ingredient ing) => 
            AvailableDrinks.IndexOf(ing.Drink.Brand 
                + "/" + ing.Drink.Name + "," + ing.Drink.Percentage + "%"); 
        private RecipeEntry entry;
        public RecipeEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                OnPropertyChanged();
            }
        }
        public static ObservableCollection<string> AvailableDrinks { get; set; }
        private string originalName;
        private ObservableCollection<Ingredient> ingredients;
        public ObservableCollection<Ingredient> Ingredients
        {
            get => ingredients;
            set
            {
                ingredients = value;
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
                OnPropertyChanged();
            }
        }
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

        IMemoryService memService;
        IAlertMessageService alertService;
        public EditRecipeViewModel(INavService navService, IMemoryService memory,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService;
            memService = memory;
        }

        public override async void Init(RecipeEntry entry)
        {
            var Drinks = await memService.GetAvailable<DrinkEntry>();
            AvailableDrinks = new ObservableCollection<string>(Drinks
                .Select(x => x.Brand + "/" + x.Name + "," + x.Percentage + "%").ToList());
            DrinkList.AvailableDrinks = Drinks.ToList() ;
            Entry = entry;
            originalName = entry.Name;
            Name = entry.Name;
            Ingredients = new ObservableCollection<Ingredient>(entry.Ingredients);
            Percentage = entry.Percentage;
            
        }

        private Command addIngredientCommand;
        public Command AddIngredientCommand => addIngredientCommand ??
            (addIngredientCommand = new Command(() => AddIngredient()));
        private void AddIngredient()
        {
            var ing = Ingredients;
            ing.Add(new Ingredient()
            {
                Amount = 0,
                Drink = new DrinkEntry()
            }) ;
            Ingredients = ing;
        }

        private Command deleteLastIngredientCommand;
        public Command DeleteLastIngredientCommand => deleteLastIngredientCommand ??
            (deleteLastIngredientCommand = new Command(() => DeleteIngredient()));

        private void DeleteIngredient()
        {
            var ing = Ingredients;
            ing.RemoveAt(ing.Count - 1);
            Ingredients = ing;
        }

        private Command deleteCommand;
        public Command DeleteCommand => deleteCommand ??
            (deleteCommand = new Command(async () => await Delete()));

        private async Task Delete()
        {
            try
            {
                await memService.Delete<RecipeEntry>(originalName);
                NavService.ClearBackStack();
                await NavService.NavigateTo<MainViewModel>();
                await NavService.NavigateTo<RecipesViewModel>();
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }
        }

        private void RefreshPercentage()
        {
            var totalAmount = Ingredients.ToList().Select(x => x.Amount).Sum();
            var alcAmount = Ingredients.ToList().Select(x => x.Drink?.Percentage * x.Amount).Sum();
            Percentage = Math.Round(alcAmount.GetValueOrDefault() / totalAmount);
        }

        private Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(async ()
            => await SaveRecipe()));
        private async Task SaveRecipe()
        {
            RefreshPercentage();
            var oEntry = Entry;
            var vEntry = new RecipeEntry
            {
                Ingredients = Ingredients.ToList(),
                Name = Name,
                Percentage = Percentage
            };
            try
            {
                if (originalName == Name)
                    await memService.Save(vEntry, Name);
                else
                {
                    await memService.Delete<RecipeEntry>(originalName);
                    await memService.Save(Entry, Name);
                }
                NavService.ClearBackStack();
                await NavService.NavigateTo<MainViewModel>();
                await NavService.NavigateTo<RecipesViewModel>();
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }            
        }
    }
}
