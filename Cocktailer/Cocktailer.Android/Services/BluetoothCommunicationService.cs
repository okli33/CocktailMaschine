using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Cocktailer.Models.Entries;
using Cocktailer.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(IBluetoothCommunicationService))]
namespace Cocktailer.Droid.Services
{

    public class BluetoothCommunicationService : Activity, IBluetoothCommunicationService
    {
        BluetoothAdapter btAdapter;

        public Stream OutputStream { set; get; }

        public Stream InputStream { set; get; }

        public async void Init()
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            if (btAdapter != null)
            {
                if (btAdapter.IsEnabled)
                {
                    var bondedDevices = btAdapter.BondedDevices;
                    if (bondedDevices.Count > 0)
                    {
                        var devices = bondedDevices.ToList();
                        BluetoothDevice device = devices[0];
                        ParcelUuid[] uuids = device.GetUuids();
                        BluetoothSocket socket = device.CreateRfcommSocketToServiceRecord(uuids[0].Uuid);
                        await socket.ConnectAsync();
                        OutputStream = socket.OutputStream;
                        InputStream = socket.InputStream;
                    }


                }
            }
        }

        public async Task Write(string s)
        {
            try
            {
                await OutputStream.WriteAsync(Encoding.ASCII.GetBytes(s));
            }
            catch (Java.IO.IOException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> Read()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await InputStream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
            catch (Java.IO.IOException ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}