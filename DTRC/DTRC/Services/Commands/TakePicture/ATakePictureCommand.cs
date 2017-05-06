﻿using System;
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

    }

}
