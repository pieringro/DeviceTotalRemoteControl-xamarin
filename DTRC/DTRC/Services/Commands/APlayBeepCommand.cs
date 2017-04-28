using System;
using System.Collections.Generic;
using System.Text;

namespace DTRC.Services.Commands
{
    public abstract class APlayBeepCommand : ACommand {

        public static new string Id {
            get {
                return "PLAY_BEEP";
            }
        }
    }
}
