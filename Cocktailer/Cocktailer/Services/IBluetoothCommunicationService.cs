
using Android.OS;
using Cocktailer.Models.Entries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface IBluetoothCommunicationService
    {
        Stream OutputStream { set; get; }
        Stream InputStream { get; set; }
        Task<byte[]> Read();
        Task<string> Write(string message);
        Task<bool> Init();
        void TryCloseConnection();
    }
}
