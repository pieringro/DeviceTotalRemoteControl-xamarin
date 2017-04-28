using System;
using System.Collections.Generic;
using System.Text;

namespace DTRC.Services.Commands
{
    public abstract class ACommand
    {
        public static string Id {
            get {
                return string.Empty;
            }
        }

        public abstract void SetData();
        public abstract bool Execute();

    }
}
