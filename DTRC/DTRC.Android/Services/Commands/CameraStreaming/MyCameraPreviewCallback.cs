using System;
using System.IO;

using Android.Hardware;
using Android.Util;
using DTRC.Services.Commands.TakePicture;

namespace DTRC.Droid.Services.Commands.CameraStreaming {

    internal class MyCameraPreviewCallback : Java.Lang.Object, Camera.IPreviewCallback {
        public bool DontTakeFrameCameraPreview = false;//true se non deve prendere il frame
        private static string TAG = "MyCameraPreviewCallback";


        public delegate void GotchaAFrame(MemoryStream imageStreamToSave);
        public GotchaAFrame GotchaAFrameCallback{ get; set; }
        private void InvokeGotchAFrameCallback(MemoryStream imageStreamToSave) {
            GotchaAFrameCallback?.Invoke(imageStreamToSave);//Invoke se la callback non e' null
        }

        private string filenameBase;
        

        public MyCameraPreviewCallback(string filenameBase, GotchaAFrame gotchaAFrameCallback = null) {
            this.filenameBase = filenameBase;
            this.GotchaAFrameCallback = gotchaAFrameCallback;
        }

        public void OnPreviewFrame(byte[] data, Camera camera) {
            Log.Info(TAG, "OnPreviewFrame: ricevuto frame dalla camera.");

            try {
                if (!DontTakeFrameCameraPreview) {
                    DontTakeFrameCameraPreview = true;
                    Camera.Parameters parameters = camera.GetParameters();
                    Camera.Size size = parameters.PreviewSize;
                    Android.Graphics.YuvImage image =
                        new Android.Graphics.YuvImage(data, parameters.PreviewFormat,
                            size.Width, size.Height, null);
                    MemoryStream imageStreamToSave = new MemoryStream();
                    image.CompressToJpeg(
                            new Android.Graphics.Rect(0, 0, image.Width, image.Height), 90,
                            imageStreamToSave);
                    imageStreamToSave.Flush();

                    InvokeGotchAFrameCallback(imageStreamToSave);//Invoke se la callback non e' null
                }
            }
            catch (Exception e) {
                Log.Error("TakePictureCommand",
                    string.Format("An error occure while writing on file.\nException:{0}", e.StackTrace));
            }
            finally {

            }
        }
    }



}