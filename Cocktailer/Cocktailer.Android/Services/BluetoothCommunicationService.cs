using Android.App;
using Android.Bluetooth;
using Android.OS;
using Cocktailer.Services;

using System;
using System.Diagnostics;
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
        BluetoothSocket socket;

        public Stream OutputStream { set; get; }

        public Stream InputStream { set; get; }

        public async Task<bool> Init()
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
                        BluetoothDevice device = devices
                            .FirstOrDefault(x => x.Name.Equals("DSD TECH HC-05"));
                        if (device == null)
                        {
                            return false;
                        }
                        ParcelUuid[] uuids = device.GetUuids();
                        socket = device.CreateRfcommSocketToServiceRecord(uuids[0].Uuid);
                        try
                        {
                            await socket.ConnectAsync();
                        }
                        catch (Exception)
                        {
                            throw new Exception("Fehler beim Aufbau einer Bluetoothverbindung");
                        }
                        OutputStream = socket.OutputStream;
                        InputStream = socket.InputStream;
                    }
                }
            }
            return true;
        }

        public async Task<string> Write(string s)
        {
            try
            {
                await OutputStream.WriteAsync(Encoding.ASCII.GetBytes(s));
            }
            catch (Java.IO.IOException ex)
            {
                throw new Exception(ex.Message);
            }
            int counter = 10000;
           
            while (!InputStream.IsDataAvailable())
            {
                System.Threading.Thread.Sleep(5);
                counter--;
                if (counter == 0)                
                    throw new TimeoutException();                
            }
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = InputStream.Read(buffer, 0, buffer.Length);
                    {
                        ms.Write(buffer, Convert.ToInt32(ms.Length), read);
                    }
                    return Encoding.ASCII.GetString(ms.ToArray());
                }
            }
            catch (Java.IO.IOException ex)
            {
                throw new Exception(ex.Message);
            }
            return "";
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

        public void TryCloseConnection()
        {
            try
            {
                socket.Close();
                socket.Dispose();
            }
            catch { }
        }

    }
}