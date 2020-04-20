using Cocktailer.Models.Entries;
using Cocktailer.Services;
using Cocktailer.Views.DataExchange;
using Microsoft.Runtime.CompilerServices;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cocktailer.ViewModels.DataExchange
{
    public class LogViewModel : BaseValidationViewModel
    {
        private ObservableCollection<FileEntry> logFiles;
        public ObservableCollection<FileEntry> LogFiles
        {
            get => logFiles;
            set
            {
                logFiles = value;
                OnPropertyChanged();
            }
        }

        private List<FileEntry> selectedItems;
        public List<FileEntry> SelectedItems
        {
            get => selectedItems;
            set
            {
                selectedItems = value;
                Validate(() => selectedItems != null && selectedItems.Count > 0, "");
                OnPropertyChanged();
                DeleteSelectedCommand.ChangeCanExecute();
            }
        }
        ILogService logService;
        IMemoryService memService;
        IAlertMessageService alertService;
        public LogViewModel(INavService navService, ILogService logService,
            IMemoryService memService, IAlertMessageService alertService) : base(navService)
        {
            this.logService = logService;
            this.memService = memService;
            this.alertService = alertService;
        }

        public override async void Init()
        {
            SelectedItems = new List<FileEntry>();
            IsBusy = true;
            try
            {
                LogFiles = new ObservableCollection<FileEntry>((await logService.GetLogFiles())
                    .Select(x => new FileEntry() { Name = x.Name, File = x, Selected = false }));
            }
            catch { }
            IsBusy = false;
        }

        private Command deleteSelectedCommand;
        public Command DeleteSelectedCommand => deleteSelectedCommand ??
            (deleteSelectedCommand = new Command(async () => await DeleteSelected(), CanDelete));
        private bool CanDelete() => SelectedItems != null && SelectedItems.Count > 0 && !HasErrors;
        private async Task DeleteSelected()
        {
            foreach (var file in SelectedItems)
            {
                if (!await memService.Delete<LogEntry>(file.Name))
                {
                    await alertService.ShowErrorMessage($"Fehler beim Löschen von {file.Name}");
                }
            }
            Init();
        }

        public Command DeleteSingleCommand => new Command(async (value) => await 
            DeleteSingle((FileEntry)value));
        private async Task DeleteSingle(FileEntry entry)
        {
            if (!await memService.Delete<LogEntry>(entry.Name))
            {
                await alertService.ShowErrorMessage($"Fehler beim Löschen von {entry.Name}");
            }
            Init();
        }

        public Command RefreshCommand => new Command(() => Init());
        public Command ChangeSelectedCommand => new Command((value) =>
            ChangeSelected((FileEntry)value));
        private void ChangeSelected(FileEntry entry)
        {
            entry.Selected = !entry.Selected;
            var logs = LogFiles;
            logs.Remove(logs.First(x => x.Name == entry.Name));
            logs.Add(entry);
            LogFiles = logs;
            var sel = SelectedItems;
            if (entry.Selected)
                sel.Add(entry);
            else
                sel.Remove(sel.First(x => x.Name == entry.Name));
            SelectedItems = sel;

        }

        public Command ShareCommand => new Command(async (value) => await Share((FileEntry)value));

        private async Task Share(FileEntry file)
        {
            await logService.ShareLogFile(file.File);
        }
    }
}
