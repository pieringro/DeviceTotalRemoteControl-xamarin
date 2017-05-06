using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Hardware;
using Android.Content;
using DTRC.Droid.Services.Commands;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands.CameraStreaming;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : ATakePictureCommand {

        private static string TAG = "TakePictureCommand";

        private string pictureDefaultFilename = "pic";
        private CameraStreaming.CameraStreaming cameraStreaming;

        public TakePictureCommand() {
            cameraStreaming = new CameraStreaming.CameraStreaming(pictureDefaultFilename);
        }

        public override void SetData() {
            
        }

        

        public override bool Execute() {
            bool result = true;

            

            result = cameraStreaming.Start(GotchaAFrameFromCamera);
            
            return result;
        }



        public void GotchaAFrameFromCamera() {
            cameraStreaming.Stop();
        }

    }
}