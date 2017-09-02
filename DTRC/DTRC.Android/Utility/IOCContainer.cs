using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using CrashlyticsKit;
using FabricSdk;

namespace DTRC.Droid.Utility {
    public class IOCContainer {

        public static void InitXLabResolver() {
            if (!Resolver.IsSet) {
                SimpleContainer container = new SimpleContainer();
                container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
                container.Register<IMediaPicker, MediaPicker>();
                container.Register<DTRC.Services.SystemConfig, DTRC.Droid.Services.SystemConfigDroid>();
                container.Register<DTRC.Services.Commands.APlayBeepCommand, DTRC.Droid.Services.Commands.PlayBeepCommand>();
                container.Register<DTRC.Services.Commands.ATakePictureCommand, DTRC.Droid.Services.Commands.TakePictureCommand>();
                container.Register<DTRC.Services.Commands.ARecordAudioCommand, DTRC.Droid.Services.Commands.RecordAudioCommand>();
                Resolver.SetResolver(container.GetResolver());
            }
        }


        private static bool FabricCrashlyticsReady = false;
        public static void InitFabricCrashlytics(Context context) {
            if (!FabricCrashlyticsReady) {
                Crashlytics.Instance.Initialize();
                Fabric.Instance.Debug = true;
                Fabric.Instance.Initialize(context);
                FabricCrashlyticsReady = true;
            }
        }

    }
}