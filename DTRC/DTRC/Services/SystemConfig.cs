using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.SecureStorage;

namespace DTRC.Services {
    public abstract class SystemConfig {

        protected string DeviceId  = null;
        protected string EmailUser = null;
        protected string PassUser  = null;

        public virtual bool ClearDataStored() {
            bool result = CrossSecureStorage.Current.DeleteKey("EmailUser");
            return result;
        }

        public abstract string GetDeviceId();

        /// <summary>
        /// Ottiene l'email dell'utente che e' stata configurata
        /// </summary>
        /// <returns></returns>
        public string GetEmailUser() {
            if(EmailUser == null) {
                //TODO operazioni per ottenere l'email memorizzata
                if (CrossSecureStorage.Current.HasKey("EmailUser")) {
                    EmailUser = CrossSecureStorage.Current.GetValue("EmailUser");
                }
            }
            return EmailUser;
        }

        public void SetEmailUser(string emailUser) {
            CrossSecureStorage.Current.SetValue("EmailUser", emailUser);
            EmailUser = emailUser;
        }

        public abstract string GetPassUser();
        public abstract void SetPassUser(string passUser);
    }



}
