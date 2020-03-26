using Cocktailer.Models.Entries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface IBluetoothCommunicationService
    {
        byte[] Read();
        Task Write(string message);
        Task Connect(string name);
        Task<IEnumerable<BluetoothEntry>>GetDevicesAsync();
        bool Connected { get; }
    }
}
