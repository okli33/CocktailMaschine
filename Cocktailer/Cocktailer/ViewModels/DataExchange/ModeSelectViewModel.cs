using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.DataExchange
{
    public class ModeSelectViewModel : BaseViewModel
    {
        IMemoryService memService;
        IDataExchangeService exchangeService;
        IAlertMessageService alertService;
        public ModeSelectViewModel(INavService navService, IMemoryService memService,
            IDataExchangeService exchangeService, IAlertMessageService alertService) : base(navService)
        {
            this.memService = memService;
            this.exchangeService = exchangeService;
            this.alertService = alertService;
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

        public Command ExportFilesCommand => new Command (async () => await ExportFiles());
        private async Task ExportFiles()
        {
            try
            {
                await exchangeService.Export();
                await alertService.ShowSuccessMessage("Daten erfolgreich exportiert");
            }
            catch
            {
                await alertService.ShowDataErrorMessage();
            }
            
        }
        public Command ImportFilesCommand => new Command(async () => await ImportFiles());

        private async Task ImportFiles()
        {
            try
            {
                await exchangeService.Import();
            }
            catch { await alertService.ShowDataErrorMessage(); }
        }
    }
}
