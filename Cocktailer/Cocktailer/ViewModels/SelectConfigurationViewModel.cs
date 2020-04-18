using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class SelectConfigurationViewModel : BaseValidationViewModel
    {
        private ObservableCollection<ConfigurationEntry> configurations;
        public ObservableCollection<ConfigurationEntry> Configurations
        {
            get => configurations;
            set
            {
                configurations = value;
                OnPropertyChanged();
            }
        }
        private ConfigurationEntry selectedConfiguration;
        public ConfigurationEntry SelectedConfiguration
        {
            get => selectedConfiguration;
            set
            {
                selectedConfiguration = value;
                Validate(() => selectedConfiguration != null, "Konfiguration muss ausgewählt sein");
                OnPropertyChanged();
                NextPageCommand.ChangeCanExecute();
            }
        }
        IMemoryService memService;
        IAlertMessageService alertService;
        IBluetoothCommunicationService btService;
        public SelectConfigurationViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService, IBluetoothCommunicationService btService) : base(navService)
        {
            this.memService = memService;
            this.alertService = alertService;
            this.btService = btService;
        }

        public override async void Init()
        { 
            SelectedConfiguration = null;
            try
            {
                Configurations = new ObservableCollection<ConfigurationEntry>
                    (await memService.GetAvailable<ConfigurationEntry>());
            }
            catch
            {
                Configurations = new ObservableCollection<ConfigurationEntry>();
                await alertService.ShowErrorMessage("Fehler beim Lesen der Daten, versuch's nochmal");
            }
        }

        private Command nextPageCommand;
        public Command NextPageCommand => nextPageCommand ?? (nextPageCommand =
            new Command(async () => await NextPage(), CanGoForward));
           
        private async Task NextPage()
        {
            NavService.ClearBackStack();
            await NavService.NavigateTo<CocktailModeViewModel, ConfigurationEntry>
                (SelectedConfiguration);
            NavService.ClearBackStack();
        }
        private bool CanGoForward() => SelectedConfiguration != null && !HasErrors;


    }
}
