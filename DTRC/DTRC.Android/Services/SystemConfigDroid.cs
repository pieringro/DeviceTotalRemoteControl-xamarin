using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Android.Content;
using Android.Telephony;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DTRC.Services;

[assembly: Xamarin.Forms.Dependency(typeof(SystemConfig))]
namespace DTRC.Droid.Services {
    public class SystemConfigDroid : SystemConfig {

        public override string GetDeviceId() {
            //TelephonyManager tm = (TelephonyManager)Application.Context.GetSystemService(Android.Content.Context.TelephonyService);
            //DeviceId = tm.DeviceId;

            DeviceId = Android.OS.Build.Serial;
            return DeviceId;
        }
        
    }
}