using Cocktailer.Models.Entries;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Cocktailer.Services
{
    public class DataExchangeService : IDataExchangeService
    {
        async Task<IFolder> appFolder() => await PCLStorage.FileSystem.Current.LocalStorage
            .CreateFolderAsync("CocktailMachine", CreationCollisionOption.OpenIfExists);
        async Task<IFolder> dataFolder() => await PCLStorage.FileSystem.Current.LocalStorage
            .CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);
        public async Task<IFolder> SubFolder<T>() => await (await appFolder()).CreateFolderAsync(typeof(T).Name,
                CreationCollisionOption.OpenIfExists);


        public async Task Export()
        {
            var file = await CreateZipArchive();
            await Share.RequestAsync(new ShareFileRequest()
            {
                File = new ShareFile(file.Path),
                Title = file.Name
            });
        }
        private async Task<IFile> CreateZipArchive()
        {
            var nameTag = DateTime.Now.ToString("s");
            var folder = await appFolder();
            var dataPath = await dataFolder();
            string archivePath = Path.Combine(dataPath.Path, $"Data-{nameTag}.zip");

            Monitor.Enter(folder);
            try
            {
                ZipFile.CreateFromDirectory(folder.Path, archivePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Monitor.Exit(folder);
            }
            return await dataPath.GetFileAsync($"Data-{nameTag}.zip");
        }

        public async Task Import()
        {
            var file = await GetFile();
            if (file == null)
                return;
            if (!file.FileName.EndsWith(".zip"))
                throw new FileLoadException();

            await ImportDataFromArchive(file);
        }

        private async Task ImportDataFromArchive(FileData data)
        {
            try
            {                
                using (var archive = new ZipArchive(data.GetStream()))
                {
                    foreach (var entry in archive.Entries.Where(x => x.FullName.StartsWith("DrinkEntry")))
                    {
                        using (var sr = new StreamReader(entry.Open()))
                        {
                            var fileText = sr.ReadToEnd();
                            var subFolder = await SubFolder<DrinkEntry>();
                            try
                            {
                                var file = await subFolder
                                    .CreateFileAsync(typeof(DrinkEntry).Name
                                    + "-" + entry.Name, CreationCollisionOption.FailIfExists);
                                await file.WriteAllTextAsync(fileText);
                            }
                            catch { }
                        }

                    }
                    foreach (var entry in archive.Entries.Where(x => x.FullName.StartsWith("RecipeEntry")))
                    {
                        using (var sr = new StreamReader(entry.Open()))
                        {
                            var fileText = sr.ReadToEnd();
                            var subFolder = await SubFolder<RecipeEntry>();
                            try
                            {
                                var file = await subFolder
                                    .CreateFileAsync(typeof(RecipeEntry).Name
                                    + "-" + entry.Name, CreationCollisionOption.FailIfExists);
                                await file.WriteAllTextAsync(fileText);
                            }
                            catch { }
                        }

                    }
                    foreach (var entry in archive.Entries.Where(x => x.FullName.StartsWith("ConfigurationEntry")))
                    {
                        using (var sr = new StreamReader(entry.Open()))
                        {
                            var fileText = sr.ReadToEnd();
                            var subFolder = await SubFolder<ConfigurationEntry>();
                            try
                            {
                                var file = await subFolder
                                    .CreateFileAsync(typeof(ConfigurationEntry).Name
                                    + "-" + entry.Name, CreationCollisionOption.FailIfExists);
                                await file.WriteAllTextAsync(fileText);
                            }
                            catch {  }
                            
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<FileData> GetFile()
        {
            FileData fileData = await CrossFilePicker.Current.PickFile();
            if (fileData == null)
                return null; // user canceled file picking
            return fileData;
        }
    }
}
