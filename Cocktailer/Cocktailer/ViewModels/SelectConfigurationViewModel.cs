using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Microsoft.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        public SelectConfigurationViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            this.memService = memService;
        }

        public override async void Init()
        {
            Configurations = new ObservableCollection<ConfigurationEntry>
                (await memService.GetAvailable<ConfigurationEntry>());
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
