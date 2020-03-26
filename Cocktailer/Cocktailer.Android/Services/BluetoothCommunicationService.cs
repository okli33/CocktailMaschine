using Cocktailer.Models.Entries;
using Cocktailer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cocktailer.Droid.Services
{
    public class BluetoothCommunicationService : IBluetoothCommunicationService
    {
        public bool Connected => throw new NotImplementedException();

        public Task Connect(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BluetoothEntry>> GetDevicesAsync()
        {
            throw new NotImplementedException();
        }

        public byte[] Read()
        {
            throw new NotImplementedException();
        }

        public Task Write(string message)
        {
            throw new NotImplementedException();
        }
    }
}