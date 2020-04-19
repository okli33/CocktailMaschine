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
        public static ObservableCollection<string> AvailableDrinks { get; set; }
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
       

        public IMemoryService memService;
        private IAlertMessageService alertService;
        public NewConfigurationViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService;
            this.memService = memService;
        }

        public override async void Init()
        {
            try
            {
                var Drinks = await memService.GetAvailable<DrinkEntry>();
                AvailableDrinks = new ObservableCollection<string>(Drinks
                .Select(x => x.Brand + "/" + x.Name + "," + x.Percentage + "%").ToList());
                DrinkList.AvailableDrinks = Drinks.ToList();
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
                await memService.Save(config, Name);
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
