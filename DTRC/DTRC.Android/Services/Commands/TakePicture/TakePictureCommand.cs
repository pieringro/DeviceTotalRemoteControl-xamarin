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
using DTRC.Services.Commands.TakePicture;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(TakePictureCommand))]
namespace DTRC.Droid.Services.Commands {
    public class TakePictureCommand : ATakePictureCommand {


        public TakePictureCommand() {

            
        }

        public override void SetData() {
            
        }

        public override bool Execute() {
            bool result = true;
            try {
                //devo eseguire tutto nel thread UI 
                using (Handler handler = new Handler(Looper.MainLooper)) {
                    handler.Post(() => {

                        cameraPreviewCallback = new MyCameraPreviewCallback();

                        surfaceView = new SurfaceView(Application.Context);

                        IWindowManager windowManager =
                            Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                        WindowManagerLayoutParams parameters = new WindowManagerLayoutParams(
                                1,1,
                                WindowManagerTypes.SystemOverlay,
                                WindowManagerFlags.WatchOutsideTouch,
                                Android.Graphics.Format.Translucent);

                        surfaceHolder = surfaceView.Holder;

                        surfaceView.SetZOrderOnTop(true);
                        surfaceHolder.SetFormat(Android.Graphics.Format.Translucent);

                        surfaceHolderCallback = new MySurfaceHolderCallback(camera, surfaceHolder, cameraPreviewCallback);
                        surfaceHolder.AddCallback(surfaceHolderCallback);

                        windowManager.AddView(surfaceView, parameters);

                    });
                }

            } catch (Exception e) {
                result = false;
            }
            
            return result;
        }


        Camera camera;
        TextureView textureView;
        SurfaceView surfaceView;
        ISurfaceHolder surfaceHolder;
        MySurfaceHolderCallback surfaceHolderCallback;
        MyCameraPreviewCallback cameraPreviewCallback;


        private class MyCameraPreviewCallback : Java.Lang.Object, Camera.IPreviewCallback {
            private static readonly Object obj = new object();
            public void Dispose() {
                
            }

            public void OnPreviewFrame(byte[] data, Camera camera) {
                Android.Util.Log.Info("TakePictureCommand", "Ce l'ho fatta, preview camera ottenuta!");
                //camera.Release();

                bool lockWasTaken = false;
                var temp = obj;

                try {
                    Monitor.Enter(temp, ref lockWasTaken);
                    WriteAndSendPicture wsp = new WriteAndSendPicture(data);
                    wsp.WriteFileLocally();
                }
                finally {
                    if (lockWasTaken) {
                        Monitor.Exit(temp);
                    }
                }
            }
        }

        private class MySurfaceHolderCallback : Java.Lang.Object, ISurfaceHolderCallback {

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
                if(camera != null) {
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


}