﻿using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Configurations
{
    public class EditConfigurationViewModel : BaseViewModel<ConfigurationEntry>
    {
        public static ObservableCollection<string> AvailableDrinks { get; set; }
        private bool saveOld;
        public bool SaveOld
        {
            get => saveOld;
            set
            {
                saveOld = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string originalEntry;
        private ConfigurationEntry entry;
        public ConfigurationEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                OnPropertyChanged();
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
        private IMemoryService memService;
        public EditConfigurationViewModel(INavService navService, IMemoryService memoryService) : base(navService)
        {
            memService = memoryService;
        }

        public override async void Init(ConfigurationEntry entry)
        {
            AvailableDrinks = new ObservableCollection<string>((await memService
                .GetAvailable<DrinkEntry>())
                .Select(x => x.Brand + "/" + x.Name + "," + x.Percentage + "%").ToList());
            Entry = entry;
            SpotList = new ObservableCollection<Spot>(Entry.Spots);
            Name = entry.Name;
            originalEntry = entry.Name;            
        }
        public Command SaveCommand => new Command(async () => await SafeConfig());

        private async Task SafeConfig()
        {
            if (originalEntry != Name && !saveOld)
                await DeleteConfig();
            if (saveOld && originalEntry == Entry.Name)
                await Application.Current.MainPage.DisplayAlert("Fehler beim speichern",
                    "Der Name für eine Konfiguration ist schon vergeben", "OK");
            else
            {
                Entry = new ConfigurationEntry()
                {
                    Spots = SpotList.ToList(),
                    Name = Name
                };
                await memService.Save(Entry, Entry.Name);
                NavService.ClearBackStack();
                await NavService.NavigateTo<MainViewModel>();
                await NavService.NavigateTo<ConfigurationsViewModel>();
            }
        }

        public Command DeleteCommand => new Command(async () => await DeleteConfig());
        private async Task DeleteConfig()
        {
            IsBusy = true;
            var fileName = originalEntry;
            var success = await memService.Delete<ConfigurationEntry>(fileName);
            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler", "Löschen ist fehlgeschlagen", "OK");
                IsBusy = false;
                return;
            }
            IsBusy = false;
            NavService.ClearBackStack();
            await NavService.NavigateTo<MainViewModel>();
            await NavService.NavigateTo<ConfigurationsViewModel>();
        }
    }
}
