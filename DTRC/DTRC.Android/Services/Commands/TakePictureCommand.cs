using System;

using Android.Util;
using Android.Hardware;
using DTRC.Droid.Services.Commands;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands.CameraStreaming;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : ATakePictureCommand {
        private static string TAG = "TakePictureCommand";

        private string pictureDefaultFilename = "pic";
        private CameraStreamingClass cameraStreaming;

        public TakePictureCommand() {
            cameraStreaming = new CameraStreamingClass(pictureDefaultFilename);
        }

        public override void SetData() {
            
        }

        

        public override bool Execute() {
            bool result = true;

            result = cameraStreaming.Start(CameraFacing.Back, () => {
                bool resultStop = cameraStreaming.Stop();
                if (!resultStop) {
                    cameraStreaming.Start(CameraFacing.Front, () => {
                        cameraStreaming.Stop();
                    });
                }
                else {
                    Log.Error(TAG, "Unable to stop CameraStreaming.");
                }
            });

            if (!result) {
                Log.Error(TAG, "Unable to start CameraStreaming.");
            }

            return result;
        }



    }
}