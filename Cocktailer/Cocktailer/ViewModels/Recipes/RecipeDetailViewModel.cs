using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Recipes
{
    public class RecipeDetailViewModel : BaseViewModel<RecipeEntry>
    {
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
        public RecipeDetailViewModel(INavService navService) : base(navService)
        {
        }
        public override void Init(RecipeEntry entry)
        { 
            Name = entry.Name;
            Ingredients = new ObservableCollection<Ingredient>(entry.Ingredients);
            Percentage = entry.Percentage;
        }
        
        public Command EditCommand => new Command(async () => await Edit());

        public async Task Edit()
        {
            var entry = new RecipeEntry()
            {
                Ingredients = Ingredients.ToList(),
                Name = Name,
                Percentage = Percentage
            };
            await NavService.NavigateTo<EditRecipeViewModel, RecipeEntry>(entry);
        }

          
    }
}
