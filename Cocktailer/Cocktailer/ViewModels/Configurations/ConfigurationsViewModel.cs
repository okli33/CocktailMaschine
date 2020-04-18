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
        IAlertMessageService alertService;

        public ConfigurationsViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService) : base(navService)
        {
            memoryService = memService;
            this.alertService = alertService;
        }
        
        public override async void Init()
        {
            ConfigurationEntries = new ObservableCollection<ConfigurationEntry>();
            await LoadConfigs();
        }

        private async Task LoadConfigs()
        {
            IsBusy = true;
            try
            {
                ConfigurationEntries = new ObservableCollection<ConfigurationEntry>(
                    await memoryService.GetAvailable<ConfigurationEntry>());
            }
            catch
            {
                await alertService
                    .ShowErrorMessage("Daten konnten nicht geladen werden, versuchs nochmal");
            }
            finally 
            {
                IsBusy = false;
            }
            
        }

        public Command<ConfigurationEntry> ViewCommand => new Command<ConfigurationEntry>(entry => NavToDetail(entry));
            
        private async void NavToDetail(ConfigurationEntry entry)
        {
            try
            {
                await NavService.NavigateTo<ConfigurationDetailViewModel, ConfigurationEntry>(entry);
            }
            catch (Exception)
            {
                await alertService.ShowFailedNavigationMessage();
                await NavService.NavigateTo<MainViewModel>();
                NavService.ClearBackStack();
            }
        }

        public Command NewCommand => new Command(async () => await NavService.NavigateTo<NewConfigurationViewModel>());

        public Command RefreshCommand => new Command(async () => await LoadConfigs());
    }
}
