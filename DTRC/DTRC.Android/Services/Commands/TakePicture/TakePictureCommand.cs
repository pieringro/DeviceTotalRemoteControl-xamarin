using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DTRC.Services.Commands;
using DTRC.Droid.Services.Commands;
using Android.Hardware;
using Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : Java.Lang.Object, ISurfaceHolderCallback /*Java.Lang.Object, TextureView.ISurfaceTextureListener*/ {


        public TakePictureCommand(Activity activity) {
            //textureView = new TextureView(activity);
            //textureView.SurfaceTextureListener = this;

            //activity.SetContentView(textureView);
            surfaceView = new SurfaceView(Application.Context);
            surfaceHolder = surfaceView.Holder;
            surfaceHolder.AddCallback(this);

            IWindowManager windowManager = 
                Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            WindowManagerLayoutParams parameters = new WindowManagerLayoutParams(
                    WindowManagerLayoutParams.WrapContent,
                    WindowManagerLayoutParams.WrapContent,
                    WindowManagerTypes.SystemOverlay,
                    WindowManagerFlags.WatchOutsideTouch,
                    Android.Graphics.Format.Transparent);
            windowManager.AddView(surfaceView, parameters);
        }


        Camera camera;
        TextureView textureView;
        SurfaceView surfaceView;
        ISurfaceHolder surfaceHolder;
        CameraPreviewCallback cameraPreviewCallback = new CameraPreviewCallback();


        private class CameraPreviewCallback : Java.Lang.Object, Camera.IPreviewCallback {

            public void Dispose() {
                
            }

            public void OnPreviewFrame(byte[] data, Camera camera) {
                
            }
        }


        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Android.Graphics.Format format, int width, int height) {

        }

        public void SurfaceCreated(ISurfaceHolder holder) {
            camera = Camera.Open();
            camera.SetPreviewDisplay(surfaceHolder);

            camera.SetPreviewCallback(cameraPreviewCallback);
            camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {

        }




//        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height) {
//            camera = Camera.Open();
//
//            textureView.LayoutParameters =
//                   new FrameLayout.LayoutParams(width, height);
//
//            try {
//                //camera.SetPreviewTexture(surface);
//                camera.StartPreview();
//
//            }
//            catch (Java.IO.IOException ex) {
//                Console.WriteLine(ex.Message);
//            }
//        }
//
//        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface) {
//            camera.StopPreview();
//            camera.Release();
//
//            return true;
//        }
//
//        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height) {
//            
//        }
//
//        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface) {
//            
//        }
//
//        public void Dispose() {
//            
//        }
    }


}