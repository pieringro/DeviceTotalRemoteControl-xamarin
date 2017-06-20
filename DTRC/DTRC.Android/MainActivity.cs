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

namespace DTRC.Droid {
    [Activity(Label = "DTRC", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        const string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            
            if (!Resolver.IsSet) { 
                var container = new SimpleContainer();
                container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
                container.Register<IMediaPicker, MediaPicker>();
                Resolver.SetResolver(container.GetResolver());  // Resolving the services 
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            LoadApplication(new App());

            if (Intent.Extras != null) {
                foreach (var key in Intent.Extras.KeySet()) {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

            this.IsPlayServicesAvailable();
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

