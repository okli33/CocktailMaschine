using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Configurations
{
    public class NewConfigurationViewModel : BaseValidationViewModel
    {
        public static ObservableCollection<DrinkEntry> AvailableDrinks { get; set; }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                Validate(() => !string.IsNullOrEmpty(name), "Name darf nicht \"\" sein");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }
        private ObservableCollection<Spot> spotList;
        public ObservableCollection<Spot> SpotList
        {
            get => spotList;
            set
            {
                spotList = value;
                OnPropertyChanged();
            }
        }
       

        public IMemoryService memoryService;
        private IAlertMessageService alertService;
        public NewConfigurationViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService;
            memoryService = memService;
        }

        public override async void Init()
        {
            try
            {
                AvailableDrinks = new ObservableCollection<DrinkEntry>(await
                    memoryService.GetAvailable<DrinkEntry>());
            }
            catch
            {
                await alertService.ShowErrorMessage("Fehler beim Lesen von Daten, versuch's nochmal");
            }
            //(4,4) for current config, maybe make configurable later on
            SpotList = new ObservableCollection<Spot>(SpotMaker.CreateSpotList(4, 4));
            
        }
        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(async () =>
            await SaveConfiguration(), CanSave));

        async Task SaveConfiguration()
        {
            ConfigurationEntry config = new ConfigurationEntry()
            {
                Name = Name,
                Spots = SpotList.ToList()
            };
            try
            {
                await memoryService.Save(config, Name);
                await NavService.GoBack();
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }
        }

        bool CanSave() => !string.IsNullOrEmpty(Name) 
            //&& SpotList.Select(x => x.Drink).Where(x => !string.IsNullOrEmpty(x.Name)).Any()
            && !HasErrors;
        
    }
}
