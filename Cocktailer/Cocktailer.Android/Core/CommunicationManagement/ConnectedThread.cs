using Android.Bluetooth;
using Android.Util;
using Java.Lang;
using System.IO;

namespace Cocktailer.Droid.Core.CommunicationManagement
{
    public class ConnectedThread : Thread
    {
        const int STATE_NONE = 0;
        const int STATE_LISTEN = 1;
        const int STATE_CONNECTING = 2;
        const int STATE_CONNECTED = 3;

        BluetoothSocket socket;
        Stream inStream;
        Stream outStream;
        BluetoothConnectionManager manager;

        public ConnectedThread(BluetoothSocket socket, BluetoothConnectionManager manager, string socketType)
        {
            Log.Debug(BluetoothConnectionManager.TAG, $"create ConnectedThread: {socketType}");
            this.socket = socket;
            this.manager = manager;
            Stream tmpIn = null;
            Stream tmpOut = null;

            // Get the BluetoothSocket input and output streams
            try
            {
                tmpIn = socket.InputStream;
                tmpOut = socket.OutputStream;
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "temp sockets not created", e);
            }

            inStream = tmpIn;
            outStream = tmpOut;
            manager.state = STATE_CONNECTED;
        }

        public override void Run()
        {
            Log.Info(BluetoothConnectionManager.TAG, "BEGIN mConnectedThread");
            byte[] buffer = new byte[1024];
            int bytes;

            // Keep listening to the InputStream while connected
            while (manager.GetState() == STATE_CONNECTED)
            {
                try
                {
                    // Read from the InputStream
                    bytes = inStream.Read(buffer, 0, buffer.Length);

                    // Send the obtained bytes to the UI Activity
                    manager.handler
                           .ObtainMessage(Constants.MESSAGE_READ, bytes, -1, buffer)
                           .SendToTarget();
                }
                catch (Java.IO.IOException e)
                {
                    Log.Error(BluetoothConnectionManager.TAG, "disconnected", e);
                    manager.ConnectionLost();
                    break;
                }
            }
        }

        /// <summary>
        /// Write to the connected OutStream.
        /// </summary>
        /// <param name='buffer'>
        /// The bytes to write
        /// </param>
        public void Write(byte[] buffer)
        {
            try
            {
                outStream.Write(buffer, 0, buffer.Length);

                // Share the sent message back to the UI Activity
                manager.handler
                       .ObtainMessage(Constants.MESSAGE_WRITE, -1, -1, buffer)
                       .SendToTarget();
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "Exception during write", e);
            }
        }

        public void Cancel()
        {
            try
            {
                socket.Close();
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(BluetoothConnectionManager.TAG, "close() of connect socket failed", e);
            }
        }
    }
}
