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
    
    public class MySurfaceHolderCallback : Java.Lang.Object, ISurfaceHolderCallback {

        public MySurfaceHolderCallback(Camera camera, ISurfaceHolder surfaceHolder,
            MyCameraPreviewCallback cameraPreviewCallback) {

            this.camera = camera;
            this.surfaceHolder = surfaceHolder;
            this.cameraPreviewCallback = cameraPreviewCallback;
        }

        Camera camera;
        ISurfaceHolder surfaceHolder;
        MyCameraPreviewCallback cameraPreviewCallback;

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

        }

    }
    

}