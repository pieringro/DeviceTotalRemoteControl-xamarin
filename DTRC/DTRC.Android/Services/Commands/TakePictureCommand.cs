using System;
using System.IO;

using Android.Util;
using Android.Hardware;
using DTRC.Droid.Services.Commands;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands.CameraStreaming;
using DTRC.Services.Commands.TakePicture;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : ATakePictureCommand {
        private static readonly string TAG = "TakePictureCommand";
        private const string pictureDefaultFilename = "pic";

        private CameraStreamingClass cameraStreaming;
        private WriteAndSendPicture wsp;

        private int frontPicDoneCounter = 0;//contatore pic front
        private int backPicDoneCounter = 0;//contatore pic back

        private int frontAllPicToDo = 0;//pic front da fare
        private int backAllPicToDo = 0;//pic back da fare

        public TakePictureCommand() {
            cameraStreaming = new CameraStreamingClass(pictureDefaultFilename);
            wsp = new WriteAndSendPicture();
        }

        public override void SetData(CommandParameter commandParams) {
            if (commandParams.dict.ContainsKey(FRONT_PIC_ID)) {
                frontAllPicToDo = int.Parse(commandParams.dict[FRONT_PIC_ID]);
            }
            if (commandParams.dict.ContainsKey(BACK_PIC_ID)) {
                backAllPicToDo = int.Parse(commandParams.dict[BACK_PIC_ID]);
            }
        }

        
        public override bool Execute() {
            bool result = true;
            frontPicDoneCounter = 0;
            backPicDoneCounter = 0;

            if(backPicDoneCounter < backAllPicToDo) {
                result = TakeBackPic();
            }
            else if(frontPicDoneCounter < frontAllPicToDo){
                result = TakeFrontPic();
            }

            if (!result) {
                Log.Error(TAG, "Unable to start CameraStreaming.");
            }

            return result;
        }
        

        private bool TakeBackPic() {
            bool result = cameraStreaming.Start(CameraFacing.Back, GotAFrameBackPicCallback);
            return result;
        }


        private void GotAFrameBackPicCallback(MemoryStream imageStreamToSaveBack) {
            bool cameraStopped = false;

            WriteAndSendPicture.CallbackOnFinished writtenPicCallback = () => {
                backPicDoneCounter++;
                cameraStreaming.DontTakeFrameCameraPreview = false;
                if (cameraStopped && frontPicDoneCounter < frontAllPicToDo) {
                    TakeFrontPic();
                }
            };

            if (backPicDoneCounter < backAllPicToDo-1 ) {
                wsp.WriteFileLocally(imageStreamToSaveBack, pictureDefaultFilename + "BACK",
                        writtenPicCallback);
            }
            else {
                cameraStopped = cameraStreaming.Stop();
                if (cameraStopped) {
                    wsp.WriteFileLocally(imageStreamToSaveBack, pictureDefaultFilename + "BACK",
                        writtenPicCallback);
                }
                else {
                    Log.Error(TAG, "Unable to stop CameraStreaming.");
                }
            }
        }


        private bool TakeFrontPic() {
            bool result = cameraStreaming.Start(CameraFacing.Front, GotAFrameFrontPicCallback);
            return result;
        }

        
        private void GotAFrameFrontPicCallback(MemoryStream imageStreamToSaveFront) {
            bool cameraStopped = false;
            WriteAndSendPicture.CallbackOnFinished writtenPicCallback = () => {
                frontPicDoneCounter++;
                cameraStreaming.DontTakeFrameCameraPreview = false;
            };

            if (frontPicDoneCounter < frontAllPicToDo) {
                wsp.WriteFileLocally(imageStreamToSaveFront, pictureDefaultFilename + "FRONT",
                        writtenPicCallback);
            }
            else {
                cameraStopped = cameraStreaming.Stop();
                if (!cameraStopped) {
                    Log.Error(TAG, "Unable to stop CameraStreaming.");
                }
            }
        }

    }
}