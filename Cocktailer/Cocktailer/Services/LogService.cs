using Cocktailer.Models.Entries;
using Xamarin.Essentials;
using System;
using PCLStorage;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Java.Sql;
using System.Linq;

namespace Cocktailer.Services
{
    public class LogService : ILogService
    {
        IFolder Logfolder;
        async Task<IFolder> appFolder() => await PCLStorage.FileSystem.Current.LocalStorage
            .CreateFolderAsync("CocktailMachine", CreationCollisionOption.OpenIfExists);
        async Task<IFolder> GetLogFolder()
        {
            if (Logfolder == null)
            {
                Logfolder = await (await appFolder())
                    .CreateFolderAsync(typeof(LogEntry).Name, CreationCollisionOption.OpenIfExists);
            }
            return Logfolder;
        }
        async Task<IFile> GetLogFile(string fileName)
        {
            IFolder logFolder = await GetLogFolder();
            return await logFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        }
        public async Task AddToLogFile(LogEntry entry)
        {
            string fileName;
            if (DateTime.Now.Hour > 10)
                fileName = DateTime.Now.ToString("D") + ".txt";
            else
            {
                fileName = DateTime.Now.AddDays(-1).ToString("D") + ".txt";
            }
            var file = await GetLogFile(fileName);
            var text = await file.ReadAllTextAsync();
            text += entry.Name;
            await file.WriteAllTextAsync(text);
        }

        public async Task ShareLogFile(IFile logFile)
        {
            await Share.RequestAsync(new ShareFileRequest()
            {
                File = new ShareFile(logFile.Path),
                Title = logFile.Name
            });
        }

        public async Task<List<IFile>> GetLogFiles()
        {
            return (await (await GetLogFolder()).GetFilesAsync()).ToList();
        }
    }
}
