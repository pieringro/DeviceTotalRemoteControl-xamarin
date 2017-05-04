using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace DTRC.Services.Commands {
    public abstract class ATakePictureCommand : ACommand {

        public static new string Id {
            get {
                return "TAKE_PICTURE";
            }
        }

        //private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //private IMediaPicker _mediaPicker;
        //private ImageSource ImageSource;
        //
        //public void Setup() {
        //    if (_mediaPicker == null) {
        //        IDevice device = Resolver.Resolve<IDevice>();
        //        _mediaPicker = DependencyService.Get<IMediaPicker>();
        //        if (_mediaPicker == null) {
        //            _mediaPicker = device.MediaPicker;
        //        }
        //    }
        //}
        //
        //
        //
        //public async Task TakePicture() {
        //    Setup();
        //
        //    ImageSource = null;
        //    
        //    await this._mediaPicker.TakePhotoAsync(
        //        new CameraMediaStorageOptions {DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400})
        //        .ContinueWith(t =>
        //    {
        //        if (t.IsFaulted) {
        //            var s = t.Exception.InnerException.ToString();
        //        }
        //        else if (t.IsCanceled) {
        //            var canceled = true;
        //        }
        //        else {
        //            var mediaFile = t.Result;
        //
        //            ImageSource = ImageSource.FromStream(() => mediaFile.Source);
        //
        //            return mediaFile;
        //        }
        //
        //        return null;
        //    }, _scheduler);
        //}

    }

}
