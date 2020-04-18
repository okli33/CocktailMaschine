﻿using Cocktailer.Services;
using Cocktailer.ViewModels.Configurations;
using Cocktailer.ViewModels.Recipes;
using Cocktailer.Views;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        IBluetoothCommunicationService btService;
        public MainViewModel(INavService navService, 
            IBluetoothCommunicationService btService) : base(navService) 
        {
            this.btService = btService;
        }

        public Command SuffCommand => new Command(async () => await NavService
            .NavigateTo<SelectConfigurationViewModel>());
        public Command RecipesCommand => new Command(async () => await NavService
            .NavigateTo<RecipesViewModel>());
        public Command ConfigurationsCommand => new Command(async () => await NavService
            .NavigateTo<ConfigurationsViewModel>());
        public Command DrinksCommand => new Command(async () =>await NavService
            .NavigateTo<DrinksViewModel>());
        public Command BluetoothCommand => new Command(async () => await
            NavService.NavigateTo<BluetoothViewModel>());
    }
}
