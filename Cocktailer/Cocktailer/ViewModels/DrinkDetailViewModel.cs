using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class DrinkDetailViewModel : BaseViewModel<DrinkEntry>
    {
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
        public DrinkDetailViewModel(INavService navService) : base(navService)
        {
            
        }

        public override void Init(DrinkEntry parameter)
        {
            Entry = parameter;
        }

        //public Command EditCommand => new Command(async () => await NavService.NavigateTo<>)
    }
}
