using System;

using Android.Util;
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

            result = cameraStreaming.Start(GotchaAFrameFromCamera);

            if (!result) {
                Log.Error(TAG, "Unable to start CameraStreaming.");
            }

            return result;
        }



        public void GotchaAFrameFromCamera() {
            bool result = cameraStreaming.Stop();
            if (!result) {
                Log.Error(TAG, "Unable to stop CameraStreaming.");
            }
        }

    }
}