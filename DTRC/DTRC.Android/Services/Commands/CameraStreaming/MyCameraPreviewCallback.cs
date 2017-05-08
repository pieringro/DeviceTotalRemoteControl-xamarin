﻿using System;

using Android.Hardware;
using Android.Util;
using DTRC.Services.Commands.TakePicture;

namespace DTRC.Droid.Services.Commands.CameraStreaming {

    internal class MyCameraPreviewCallback : Java.Lang.Object, Camera.IPreviewCallback {
        private bool myLock = false;
        private static string TAG = "MyCameraPreviewCallback";

        public delegate void GotchaAFrame(System.IO.MemoryStream imageStreamToSave);
        public GotchaAFrame GotchaAFrameCallback{ get; set; }

        private string filenameBase;
        

        public MyCameraPreviewCallback(string filenameBase, GotchaAFrame gotchaAFrameCallback = null) {
            this.filenameBase = filenameBase;
            this.GotchaAFrameCallback = gotchaAFrameCallback;
        }

        public void OnPreviewFrame(byte[] data, Camera camera) {
            Log.Info(TAG, "OnPreviewFrame: ricevuto frame dalla camera.");

            try {
                if (!myLock) {
                    myLock = true;
                    Camera.Parameters parameters = camera.GetParameters();
                    Camera.Size size = parameters.PreviewSize;
                    Android.Graphics.YuvImage image =
                        new Android.Graphics.YuvImage(data, parameters.PreviewFormat,
                            size.Width, size.Height, null);
                    System.IO.MemoryStream imageStreamToSave = new System.IO.MemoryStream();
                    image.CompressToJpeg(
                            new Android.Graphics.Rect(0, 0, image.Width, image.Height), 90,
                            imageStreamToSave);
                    imageStreamToSave.Flush();

                    GotchaAFrameCallback?.Invoke(imageStreamToSave);//Invoke se la callback non e' null
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