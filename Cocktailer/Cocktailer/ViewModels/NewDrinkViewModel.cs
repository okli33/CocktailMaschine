using Cocktailer.Models.Entries;
using Cocktailer.Models.MemoryManagement;
using Cocktailer.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class NewDrinkViewModel : BaseValidationViewModel
    {
        string brand;
        public string Brand
        {
            get => brand;
            set
            {
                brand = value;
                OnPropertyChanged();
            }
        }
        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                Validate(() => !string.IsNullOrWhiteSpace(name), "Name must be provided");
                OnPropertyChanged();
                SaveDrink.ChangeCanExecute();
            }
        }

        double percentage;
        public double Percentage
        {
            get => percentage;
            set
            {
                percentage = value;
                Validate(() => !double.IsNaN(percentage), "Percentage has to be a number");
                OnPropertyChanged();
                SaveDrink.ChangeCanExecute();
            }
        }
        public NewDrinkViewModel(INavService navService) : base(navService) { }
        public override void Init()
        {
            
        }

        Command saveDrink;
        public Command SaveDrink => saveDrink ?? (saveDrink = new Command(async () => await 
            Save(), CanSave));

        async Task Save()
        {
            var newItem = new DrinkEntry
            {
                Brand = Brand,
                Name = Name,
                Percentage = Percentage
            };
            var memManager = new MemoryManager<DrinkEntry>();
            if (Brand != null) { }
                //memManager.Save(newItem, Brand + Name);
            else { }
            //memManager.Save(newItem, Name);
            await NavService.GoBack();
        }
        bool CanSave() => !string.IsNullOrWhiteSpace(Name) && !HasErrors;
    }
}
