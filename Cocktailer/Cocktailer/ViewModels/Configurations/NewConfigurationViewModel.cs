﻿using Cocktailer.Models.ConfigurationManagement;
using Cocktailer.Models.DataManagement;
using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.Configurations
{
    public class NewConfigurationViewModel : BaseValidationViewModel
    {
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

        public NewConfigurationViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            memoryService = memService;
        }

        public override async void Init()
        {
            var Drinks = await memoryService.GetAvailable<DrinkEntry>();
            //(4,4) for current config, maybe make configurable later on
            SpotList = new ObservableCollection<Spot>(SpotMaker.CreateSpotList(4, 4, Drinks));
            
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
            await memoryService.Save<ConfigurationEntry>(config, Name);
            await NavService.GoBack();
        }

        bool CanSave() => !string.IsNullOrEmpty(Name) 
            //&& SpotList.Select(x => x.Drink).Where(x => !string.IsNullOrEmpty(x.Name)).Any()
            && !HasErrors;
        
    }
}