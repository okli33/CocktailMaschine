using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cocktailer.Droid.Services;

namespace Cocktailer.Droid
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        BluetoothDeviceReceiver receiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            receiver = new BluetoothDeviceReceiver(this);
            RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionFound));
        }
    }

    public class BluetoothDeviceReceiver : BroadcastReceiver
    {
        Activity activity;
        public BluetoothDeviceReceiver(Activity act)
        {
            activity = act;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == BluetoothDevice.ActionFound)
            {
                BluetoothDevice device = (BluetoothDevice)intent
                    .GetParcelableExtra(BluetoothDevice.ExtraDevice);

            }
        }
    }
}