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
using DTRC.Droid.Services;
using Plugin.SecureStorage;

[assembly: Xamarin.Forms.Dependency(typeof(SystemConfigDroid))]
namespace DTRC.Droid.Services {
    public class SystemConfigDroid : SystemConfig {

        public SystemConfigDroid() {

        }

        public override bool ClearDataStored() {
            bool result = base.ClearDataStored();
            SecureStorageImplementation.StoragePassword = null;
            return result;
        }

        public override string GetDeviceId() {
            //TelephonyManager tm = (TelephonyManager)Application.Context.GetSystemService(Android.Content.Context.TelephonyService);
            //DeviceId = tm.DeviceId;

            DeviceId = Android.OS.Build.Serial;
            return DeviceId;
        }

        public override string GetPassUser() {
            if (PassUser == null) {
                //TODO operazioni per ottenere l'email memorizzata
                if (SecureStorageImplementation.StoragePassword != null) {
                    PassUser = SecureStorageImplementation.StoragePassword;
                }
            }
            return PassUser;
        }

        public override void SetPassUser(string passUser) {
            SecureStorageImplementation.StoragePassword = passUser;
            PassUser = passUser;
        }

    }
}