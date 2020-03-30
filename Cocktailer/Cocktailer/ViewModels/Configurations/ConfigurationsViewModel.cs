using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.Views.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Configurations
{
    public class ConfigurationsViewModel : BaseViewModel
    {
        private ObservableCollection<ConfigurationEntry> configurationEntries;
        public ObservableCollection<ConfigurationEntry> ConfigurationEntries
        {
            get => configurationEntries;
            set
            {
                configurationEntries = value;
                OnPropertyChanged();
            }
        }

        IMemoryService memoryService;

        public ConfigurationsViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            memoryService = memService;
        }
        
        public override async void Init()
        {
            ConfigurationEntries = new ObservableCollection<ConfigurationEntry>();
            await LoadConfigs();
        }

        private async Task LoadConfigs()
        {
            IsBusy = true;
            ConfigurationEntries = new ObservableCollection<ConfigurationEntry>(
                await memoryService.GetAvailable<ConfigurationEntry>());
            IsBusy = false;
        }

        public Command<ConfigurationEntry> ViewCommand => new Command<ConfigurationEntry>(entry => NavToDetail(entry));
            
        private async void NavToDetail(ConfigurationEntry entry)
        {
            await NavService.NavigateTo<ConfigurationDetailViewModel, ConfigurationEntry>(entry);
        }

        public Command NewCommand => new Command(async () => await NavService.NavigateTo<NewConfigurationViewModel>());

        public Command RefreshCommand => new Command(async () => await LoadConfigs());
    }
}
