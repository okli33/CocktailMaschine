using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Configurations
{
    public class ConfigurationDetailViewModel : BaseViewModel<ConfigurationEntry>
    {
        ConfigurationEntry entry;
        public ConfigurationEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                OnPropertyChanged();
            }
        }
        IAlertMessageService alertService;
        public ConfigurationDetailViewModel(INavService navService,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService;
        }
        public override void Init(ConfigurationEntry entry)
        {
            Entry = entry;
        }

        public Command EditCommand => new Command(async () => await NavigateToEdit());
            

        private async Task NavigateToEdit()
        {
            try
            {
                await NavService.NavigateTo<EditConfigurationViewModel, ConfigurationEntry>(Entry);
            }
            catch (Exception)
            {
                await alertService.ShowFailedNavigationMessage();
                await NavService.GoBack();
            }
        }
    }
}
