using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Diagnostics;
using Android.OS;
using PCLAppConfig;
using Plugin.SecureStorage;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands;
using DTRC.Droid.Services;

namespace DTRC.Services {
    [BroadcastReceiver(Enabled = true, Exported = true, Permission = "android.permission.RECEIVE_BOOT_COMPLETED")]
    [IntentFilter(new string[] { "android.intent.action.BOOT_COMPLETED" })]
    public class StartReceiver : BroadcastReceiver{
        public override void OnReceive(Context context, Intent intent) {
            System.Diagnostics.Debug.WriteLine("StartReceiver.OnReceive called. Prepare to start firebase messaging service...");

            DTRC.Droid.Utility.IOCContainer.InitXLabResolver();
            DTRC.Droid.Utility.IOCContainer.InitFabricCrashlytics(Application.Context);

            //Bundle bundle = new Bundle();
            //global::Xamarin.Forms.Forms.Init(context, bundle);
            //try {
            //    ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            //}
            //catch (TypeInitializationException e) {
            //    System.Diagnostics.Debug.WriteLine("ConfigurationManager exception: " + e.StackTrace);
            //}
            //SecureStorageImplementation.StoragePassword = SystemConfig.STORAGE_PASSWORD;

            //avvio servizio firebase recupero token
            Intent firebaseTokenServiceIntent = new Intent(context, typeof(MyFirebaseInstanceIdInternalReceiver));
            context.StartService(firebaseTokenServiceIntent);

            //avvio servizio firebase messaging
            Intent firebaseMessagingServiceIntent = new Intent(context, typeof(MyFirebaseMessagingService));
            context.StartService(firebaseMessagingServiceIntent);
        }
    }
}