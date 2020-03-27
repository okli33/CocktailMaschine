using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class DrinksViewModel : BaseViewModel<IMemoryService>
    {
        private ObservableCollection<DrinkEntry> drinkEntries;
        public ObservableCollection<DrinkEntry> DrinkEntries
        {
            get => drinkEntries;
            set
            {
                drinkEntries = value;
                OnPropertyChanged();
            }
        }

        private IMemoryService MemService;

        public DrinksViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            DrinkEntries = new ObservableCollection<DrinkEntry>();
            MemService = memService;
        }

        public override void Init()
        {
            LoadDrinks();
        }

        private async void LoadDrinks()
        {
            IsBusy = true;
            DrinkEntries.Clear();

            DrinkEntries = new ObservableCollection<DrinkEntry>(
                await MemService.GetAvailable<DrinkEntry>());
            
            IsBusy = false;
        }

        public Command<DrinkEntry> ViewCommand => new Command<DrinkEntry>(async entry =>
            await NavService.NavigateTo<DrinkDetailViewModel, DrinkEntry>(entry));

        public Command NewCommand => new Command(async () =>
            await NavService.NavigateTo<NewDrinkViewModel>());

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(LoadDrinks));
    }
}
