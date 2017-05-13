using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Hardware;
using Android.Content;
using Android.Util;
using DTRC.Droid.Services.Commands;
using DTRC.Services.Commands;

namespace DTRC.Droid.Services.Commands.CameraStreaming {
    public class CameraStreamingClass {
        private static string TAG = "CameraStreamingClass";

        public CameraStreamingClass(string pictureDefaultFilename) {
            this.PictureDefaultFilename = pictureDefaultFilename;
        }

        public bool IsStopped { get; private set; }

        public bool DontTakeFrameCameraPreview {
            get {
                return cameraPreviewCallback.DontTakeFrameCameraPreview;
            }
            set {
                cameraPreviewCallback.DontTakeFrameCameraPreview = value;
            }
        }

        private string PictureDefaultFilename;

        private IWindowManager windowManager;
        private SurfaceView surfaceView;
        private ISurfaceHolder surfaceHolder;
        private MySurfaceHolderCallback surfaceHolderCallback;
        private MyCameraPreviewCallback cameraPreviewCallback;

        private void GotchaAFrameFromCamera(System.IO.MemoryStream imageStreamToSave) {
            GotchaAFrameCallback(imageStreamToSave);

        }

        public delegate void GotchaAFrame(System.IO.MemoryStream imageStreamToSave);
        private GotchaAFrame GotchaAFrameCallback;

        public bool Start(CameraFacing cameraId, GotchaAFrame gotchaAFrameFromCamera) {
            bool result = true;
            this.IsStopped = false;
            this.GotchaAFrameCallback = gotchaAFrameFromCamera;
            try {
                using (Handler handler = new Handler(Looper.MainLooper)) {
                    handler.Post(() => {
                        cameraPreviewCallback = new MyCameraPreviewCallback(PictureDefaultFilename,
                            GotchaAFrameFromCamera);

                        surfaceView = new SurfaceView(Application.Context);

                        windowManager =
                            Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                        WindowManagerLayoutParams parameters = new WindowManagerLayoutParams(
                                1, 1,
                                WindowManagerTypes.SystemOverlay,
                                WindowManagerFlags.WatchOutsideTouch,
                                Android.Graphics.Format.Translucent);

                        surfaceHolder = surfaceView.Holder;

                        surfaceView.SetZOrderOnTop(true);
                        surfaceHolder.SetFormat(Android.Graphics.Format.Translucent);

                        surfaceHolderCallback = new MySurfaceHolderCallback(surfaceHolder, cameraPreviewCallback);
                        surfaceHolderCallback.cameraId = cameraId;
                        surfaceHolder.AddCallback(surfaceHolderCallback);

                        windowManager.AddView(surfaceView, parameters);
                    });
                }
            }
            catch (Exception e) {
                Log.Error(TAG, "Error: " + e.StackTrace);
                result = false;
            }

            return result;
        }


        public bool Stop() {
            bool result = true;
            try {
                if (!IsStopped) {
                    surfaceHolder.Dispose();
                    windowManager.RemoveView(surfaceView);
                    surfaceView.Dispose();
                    windowManager.Dispose();
                    IsStopped = true;
                    cameraPreviewCallback.GotchaAFrameCallback = null;
                }
            } catch(Exception e) {
                Log.Error(TAG, "Error: "+e.StackTrace);
                result = false;
            }
            return result;
        }


    }

}