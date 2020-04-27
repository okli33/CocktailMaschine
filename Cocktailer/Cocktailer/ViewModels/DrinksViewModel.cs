using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
                var entries = await MemService.GetAvailable<DrinkEntry>();
                entries = entries.OrderBy(x => x.ToString().Trim().Replace("/","")).ToList();
                DrinkEntries = new ObservableCollection<DrinkEntry>(entries);
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

        public Command DeleteSingleCommand => new Command(async (value) => await
            DeleteSingle((DrinkEntry)value));
        private async Task DeleteSingle(DrinkEntry entry)
        {
            if (!await MemService.Delete<DrinkEntry>($"{entry.Brand}-{entry.Name}-{entry.Percentage}"))
            {
                await alertService.ShowErrorMessage($"Fehler beim Löschen von {entry.Name}");
            }
            LoadDrinks();
        }
    }
}
