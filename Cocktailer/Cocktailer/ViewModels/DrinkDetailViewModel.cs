using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels
{
    public class DrinkDetailViewModel : BaseViewModel<DrinkEntry>
    {
        private DrinkEntry _entry;
        public DrinkEntry Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                OnPropertyChanged();
            }
        }
        IMemoryService MemoryService;
        public DrinkDetailViewModel(INavService navService, IMemoryService memService) : base(navService)
        {
            MemoryService = memService;
        }

        public override void Init(DrinkEntry parameter)
        {
            Entry = parameter;
        }
        public Command<DrinkEntry> EditCommand =>  new Command<DrinkEntry>(async entry =>
            await NavService.NavigateTo<EditDrinkViewModel, DrinkEntry>(entry));
    }
}
