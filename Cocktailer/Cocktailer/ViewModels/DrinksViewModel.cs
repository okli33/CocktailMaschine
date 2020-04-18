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
        private IAlertMessageService alertService;
        public DrinksViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService) : base(navService)
        {
            DrinkEntries = new ObservableCollection<DrinkEntry>();
            MemService = memService;
            this.alertService = alertService;
        }

        public override void Init()
        {
            LoadDrinks();
        }

        private async void LoadDrinks()
        {
            IsBusy = true;
            DrinkEntries.Clear();
            try
            {
                DrinkEntries = new ObservableCollection<DrinkEntry>(
                    await MemService.GetAvailable<DrinkEntry>());
            }
            catch
            {
                await alertService.ShowErrorMessage("Fehler beim Laden der Daten, versuch's nochmal");
            }
            finally { IsBusy = false; }            
        }

        public Command<DrinkEntry> ViewCommand => new Command<DrinkEntry>(async entry =>
            await NavService.NavigateTo<DrinkDetailViewModel, DrinkEntry>(entry));

        public Command NewCommand => new Command(async () =>
            await NavService.NavigateTo<NewDrinkViewModel>());

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(LoadDrinks));
    }
}
