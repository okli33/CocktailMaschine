using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class NewDrinkViewModel : BaseValidationViewModel
    {
        IMemoryService MemoryService;
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
                Validate(() => !double.IsNaN(percentage) && percentage >= 0
                    && percentage <= 100, "Percentage has to be a number between 0 and 100");
                OnPropertyChanged();
                SaveDrink.ChangeCanExecute();
            }
        }
        public NewDrinkViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            MemoryService = memService;
        }
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

            IsBusy = true;
            try
            {
                await MemoryService.Save<DrinkEntry>(newItem, $"{Brand}-{Name}-{Percentage}");
            }
            finally
            {
                IsBusy = false;
            }
            await NavService.GoBack();
        }
        bool CanSave() => !string.IsNullOrWhiteSpace(Name) && !double.IsNaN(Percentage) && Percentage >= 0 
             && Percentage <= 100 && !HasErrors;
    }
}
