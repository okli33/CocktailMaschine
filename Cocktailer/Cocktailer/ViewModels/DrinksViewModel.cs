using Cocktailer.Models.Entries;
using Cocktailer.Models.MemoryManagement;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class DrinksViewModel : BaseViewModel
    {
        private ObservableCollection<DrinkEntry> drinkEntries;
        public ObservableCollection<DrinkEntry> DrinkEntries
        {
            get => drinkEntries;
            set
            {
                drinkEntries = value;
                OnPropertyChanged();
            }
        }

        public DrinksViewModel(INavService navService) : base(navService)
        {
            DrinkEntries = new ObservableCollection<DrinkEntry>();
        }

        public override void Init()
        {
            LoadDrinks();
        }

        private void LoadDrinks()
        {
            DrinkEntries.Clear();

            DrinkEntries.Add(
                new DrinkEntry
                {
                    Brand = "Bacardi",
                    Name = "Razz",
                    Percentage = 32.5
                });
            DrinkEntries.Add(
                new DrinkEntry
                {
                    Brand = "Sierra",
                    Name = "Tequilla",
                    Percentage = 40
                });
        }

        public Command<DrinkEntry> ViewCommand => new Command<DrinkEntry>(async entry =>
            await NavService.NavigateTo<DrinkDetailViewModel, DrinkEntry>(entry));

        public Command NewCommand => new Command(async () =>
            await NavService.NavigateTo<NewDrinkViewModel>());
    }
}
