using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.Views.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                var entries = await memoryService.GetAvailable<ConfigurationEntry>();
                entries = entries.OrderBy(x => x.Name).ToList();
                ConfigurationEntries = new ObservableCollection<ConfigurationEntry>(entries);
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
            }
        }

        public Command NewCommand => new Command(async () => await NavService.NavigateTo<NewConfigurationViewModel>());

        public Command RefreshCommand => new Command(async () => await LoadConfigs());

        public Command DeleteSingleCommand => new Command(async (value) => await
            DeleteSingle((ConfigurationEntry)value));
        private async Task DeleteSingle(ConfigurationEntry entry)
        {
            if (!await memoryService.Delete<ConfigurationEntry>(entry.Name))
            {
                await alertService.ShowErrorMessage($"Fehler beim Löschen von {entry.Name}");
            }
            Init();
        }
    }
}
