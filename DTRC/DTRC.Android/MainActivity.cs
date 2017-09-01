using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using PCLAppConfig;
using DTRC.Droid.Services.Commands;
using Plugin.SecureStorage;
using DTRC.Services;
using DTRC.Droid.Services;
using DTRC.Services.Commands;

namespace DTRC.Droid {
    [Activity(Label = "DTRC", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        const string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            DTRC.Droid.Utility.IOCContainer.InitXLabResolver();
            
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            try {
                ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            } catch (TypeInitializationException e) {
                System.Diagnostics.Debug.WriteLine("ConfigurationManager exception: "+e.StackTrace);
            }

            SecureStorageImplementation.StoragePassword = SystemConfig.STORAGE_PASSWORD;
            LoadApplication(new App());

            
            FabricSdk.Initializer.Initialize(FabricSdk.Fabric.Instance, this);
            CrashlyticsKit.Initializer.Initialize(CrashlyticsKit.Crashlytics.Instance);


            if (Intent.Extras != null) {
                foreach (var key in Intent.Extras.KeySet()) {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

            this.IsPlayServicesAvailable();

        }

        protected override void OnStart() {
            base.OnStart();
            //proviamo qui ad inizializzare a chiamare Crashlytics
            CrashlyticsKit.Crashlytics.Instance.Crash();
        }


        public bool IsPlayServicesAvailable() {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success) {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)) {
                    Log.Info("MainActivity.IsPlayServicesAvailable",
                        GoogleApiAvailability.Instance.GetErrorString(resultCode));
                }
                else {
                    Log.Info("MainActivity.IsPlayServicesAvailable", "This device is not supported");
                    Finish();
                }
                return false;
            }
            else {
                Log.Info("MainActivity.IsPlayServicesAvailable", "Google Play Services is available.");
                return true;
            }
        }

    }
}

