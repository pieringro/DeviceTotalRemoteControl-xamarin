using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DTRC.Services {
    public abstract class SystemConfig {

        protected string DeviceId;
        protected string EmailUser;
        protected string PassUser;

        public abstract string GetDeviceId();

        /// <summary>
        /// Ottiene l'email dell'utente che e' stata configurata
        /// </summary>
        /// <returns></returns>
        public string GetEmailUser() {
            if(EmailUser == null) {
                //operazioni per ottenere l'email memorizzata
            }
            return EmailUser;
        }


        public string GetPassUser() {
            if(PassUser == null) {
                //operazioni per ottenere la password memorizzata
            }
            return PassUser;
        }


    }
}
