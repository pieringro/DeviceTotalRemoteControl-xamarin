using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Services.Commands {
    public abstract class ARecordAudioCommand : ACommand {
        protected const string TIMER_ID = "Timer";

        public static new string Id {
            get {
                return DTRC.Helpers.Settings.RecordAudioKey;
            }
        }
    }
}
