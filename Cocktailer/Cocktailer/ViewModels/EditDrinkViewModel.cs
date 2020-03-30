using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class EditDrinkViewModel : BaseViewModel<DrinkEntry>
    {
        private DrinkEntry originalEntry;
        private DrinkEntry entry;
        public DrinkEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                OnPropertyChanged();
            }
        }
        private IMemoryService MemoryService;
        public EditDrinkViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            MemoryService = memService;
        }

        public override void Init(DrinkEntry parameter)
        {
            Entry = parameter;
            originalEntry = parameter;
            Entry = new DrinkEntry
            {
                Brand = parameter.Brand,
                Name = parameter.Name,
                Percentage = parameter.Percentage
            };
        }

        Command saveDrink;
        public Command SaveDrink => saveDrink ?? (saveDrink = new Command(async () => await
            Save()));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(
            async () => await Delete()));

        async Task Save()
        {
            var oldFileName = $"{originalEntry.Brand}-{originalEntry.Name}-{originalEntry.Percentage}";
            await MemoryService.Delete<DrinkEntry>(oldFileName);
            IsBusy = true;
            try
            {
                await MemoryService.Save
                    (Entry, $"{Entry.Brand}-{Entry.Name}-{Entry.Percentage}");
            }
            finally
            {
                IsBusy = false;
            }
            NavService.ClearBackStack();
            await NavService.NavigateTo<MainViewModel>();
            await NavService.NavigateTo<DrinksViewModel>();
        }

        async Task Delete()
        {
            IsBusy = true;
            var fileName = $"{originalEntry.Brand}-{originalEntry.Name}-{originalEntry.Percentage}";
            var success = await MemoryService.Delete<DrinkEntry>(fileName);
            if (!success)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", "Löschen ist fehlgeschlagen", "OK");
                IsBusy = false;
                return;
            }
            IsBusy = false;
            NavService.ClearBackStack();
            await NavService.NavigateTo<MainViewModel>();
            await NavService.NavigateTo<DrinksViewModel>();
        }
    }
}
