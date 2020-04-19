using Cocktailer.Models.Entries;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface IMemoryService 
    {
        Task<T> Load<T>(string fileName) where T : IEntry;
        Task<List<T>> GetAvailable<T>() where T : IEntry;
        Task Save<T>(T obj, string fileName) where T : IEntry;
        Task<bool> Delete<T>(string fileName) where T : IEntry;
        Task<IFolder> SubFolder<T>();
    }
}
