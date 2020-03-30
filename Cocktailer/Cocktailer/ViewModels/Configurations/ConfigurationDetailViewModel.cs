using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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

        public ConfigurationDetailViewModel(INavService navService) : base(navService)
        {
        }
        public override void Init(ConfigurationEntry entry)
        {
            Entry = entry;
        }

        public Command ViewCommand => new Command(async () => await
            NavService.NavigateTo<MainViewModel>());

        public Command EditCommand => new Command(async () => await
            NavService.NavigateTo<EditConfigurationViewModel, ConfigurationEntry>(Entry));
    }
}
