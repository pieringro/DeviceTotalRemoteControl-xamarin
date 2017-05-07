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
using Android.Hardware;

namespace DTRC.Droid.Services.Commands.CameraStreaming {
    
    internal class MySurfaceHolderCallback : Java.Lang.Object, ISurfaceHolderCallback {

        public MySurfaceHolderCallback(ISurfaceHolder surfaceHolder,
            MyCameraPreviewCallback cameraPreviewCallback) {

            this.surfaceHolder = surfaceHolder;
            this.cameraPreviewCallback = cameraPreviewCallback;
        }

        ISurfaceHolder surfaceHolder;
        MyCameraPreviewCallback cameraPreviewCallback;

        public Camera camera { get; private set; }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height) {

        }

        public void SurfaceCreated(ISurfaceHolder holder) {
            if (camera != null) {
                camera.Release();
                camera = null;
            }
            camera = Camera.Open(0);

            camera.SetPreviewDisplay(surfaceHolder);

            camera.SetPreviewCallback(cameraPreviewCallback);
            camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {
            camera.StopPreview();
            camera.SetPreviewCallback(null);
            camera.Release();
            camera = null;
        }

    }
    

}