using System;

using Android.Util;
using Android.Hardware;
using DTRC.Droid.Services.Commands;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands.CameraStreaming;
using DTRC.Services.Commands.TakePicture;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : ATakePictureCommand {
        private static string TAG = "TakePictureCommand";

        private string pictureDefaultFilename = "pic";
        private CameraStreamingClass cameraStreaming;

        private WriteAndSendPicture wsp;

        public TakePictureCommand() {
            cameraStreaming = new CameraStreamingClass(pictureDefaultFilename);
            wsp = new WriteAndSendPicture();
        }

        public override void SetData() {
            
        }

        

        public override bool Execute() {
            bool result = true;

            result = cameraStreaming.Start(CameraFacing.Back, 
                (imageStreamToSaveBack) => {
                    bool resultStop = cameraStreaming.Stop();
                    if (resultStop) {

                        wsp.WriteFileLocally(imageStreamToSaveBack, pictureDefaultFilename+"BACK",
                        () => {
                            cameraStreaming.Start(CameraFacing.Front, (imageStreamToSaveFront) => {
                                resultStop = cameraStreaming.Stop();
                                if (resultStop) {
                                    wsp.WriteFileLocally(imageStreamToSaveFront, pictureDefaultFilename + "FRONT",
                                    () => {

                                    });
                                }
                            });
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


        private void SaveImage(System.IO.MemoryStream imageStreamToSave, string filenameBase) {
            
            wsp.WriteFileLocally(imageStreamToSave, filenameBase, 
                () => {
                    //file salvato

                });
        }


    }
}