using Cocktailer.Models.Entries;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public class MemoryService : IMemoryService
    {
        async Task<IFolder> appFolder() => await FileSystem.Current.LocalStorage
            .CreateFolderAsync("CocktailMachine", CreationCollisionOption.OpenIfExists);
        public async Task<IFolder> SubFolder<T>() => await (await appFolder()).CreateFolderAsync(typeof(T).Name,
                CreationCollisionOption.OpenIfExists);
        public async Task<List<T>> GetAvailable<T>() where T : IEntry
        {
            IFolder folder = null;
            List<T> availableObjects = new List<T>();
            try
            {                
                folder = await SubFolder<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                availableObjects.Add(await Load<T>(file.Name));
            }
            return availableObjects;
        }

        public async Task<T> Load<T>(string fileName) where T : IEntry
        {
            fileName = fileName.EndsWith(".json") ? fileName : fileName + ".json";
            IFolder folder = await SubFolder<T>();
            var fileText = await (await folder.GetFileAsync(fileName)
                ).ReadAllTextAsync();
            return JsonConvert.DeserializeObject<T>(fileText);
        }

        public async Task Save<T>(T obj, string name) where T : IEntry
        {
            var text = JsonConvert.SerializeObject(obj);
            var folder = await SubFolder<T>();
            name = name.Replace(typeof(T).Name + "-", "");
            var file = await folder.CreateFileAsync(name + ".json",
                CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(text);
        }

        public async Task<bool> Delete<T>(string name) where T : IEntry
        {
            string fileName = name.EndsWith(".json") || name.EndsWith(".txt") ? 
                name :  name + ".json";
            var folder = await SubFolder<T>();
            try
            {
                await (await folder.GetFileAsync(fileName)).DeleteAsync();
                return true;
            }
            catch
            {
                
            }
            fileName = typeof(T).Name + "-" + fileName;
            try
            {
                await (await folder.GetFileAsync(fileName)).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
