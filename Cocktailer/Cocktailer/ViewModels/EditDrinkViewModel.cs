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
        IAlertMessageService alertService;
        public EditDrinkViewModel(INavService navService, IMemoryService memService,
            IAlertMessageService alertService) : base(navService)
        {
            this.alertService = alertService; 
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
            try
            {
                await MemoryService.Delete<DrinkEntry>(oldFileName);
            }
            catch { await alertService.ShowDataErrorMessage();  return; }
            IsBusy = true;
            try
            {
                await MemoryService.Save
                    (Entry, $"{Entry.Brand}-{Entry.Name}-{Entry.Percentage}");
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }
            finally
            {
                IsBusy = false;
            }
            await NavService.GoBack();
            await NavService.GoBack();
        }

        async Task Delete()
        {
            IsBusy = true;
            bool success = false;
            var fileName = $"{originalEntry.Brand}-{originalEntry.Name}-{originalEntry.Percentage}";
            try
            {
                success = await MemoryService.Delete<DrinkEntry>(fileName);
            }
            catch { IsBusy = false; await alertService.ShowDataErrorMessage(); return; }

            if (!success)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", "Löschen ist fehlgeschlagen", "OK");
                IsBusy = false;
                return;
            }
            IsBusy = false;
            await NavService.GoBack();
            await NavService.GoBack();
        }
    }
}
