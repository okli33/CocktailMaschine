using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.DataExchange
{
    public class ModeSelectViewModel : BaseViewModel
    {
        public ModeSelectViewModel(INavService navService) : base(navService)
        {

        } 

        public override void Init()
        {

        }

        public Command LogCommand => new Command(async () =>
            await NavService.NavigateTo<LogViewModel>());
        public Command ExportCommand => new Command(async () =>
            await NavService.NavigateTo<ExportViewModel>());
        public Command ImportCommand => new Command(async () =>
           await NavService.NavigateTo<ImportViewModel>());
    }
}
