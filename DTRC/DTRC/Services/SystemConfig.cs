using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.SecureStorage;

namespace DTRC.Services {
    public abstract class SystemConfig {
        /// <summary>
        /// Password utilizzata per la criptazione dei dati sensibili dell'utente
        /// Dovrebbe essere una chiave di crittazione per la libreria Plugin.SecureStorage
        /// </summary>
        public static readonly string STORAGE_PASSWORD = "3fj389bn278v";

        protected string DeviceId  = null;
        protected string EmailUser = null;
        protected string PassUser  = null;
        protected string LangUser  = null;

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
                try {
                    //operazioni per ottenere l'email memorizzata
                    if (CrossSecureStorage.Current.HasKey("EmailUser")) {
                        EmailUser = CrossSecureStorage.Current.GetValue("EmailUser");
                    }
                } catch(Exception e) {

                }
            }
            return EmailUser;
        }

        public void SetEmailUser(string emailUser) {
            CrossSecureStorage.Current.SetValue("EmailUser", emailUser);
            EmailUser = emailUser;
        }


        public string GetPassUser() {
            if (PassUser == null) {
                //operazioni per ottenere la password memorizzata
                if (CrossSecureStorage.Current.HasKey("PassUser")) {
                    PassUser = CrossSecureStorage.Current.GetValue("PassUser");
                }
            }
            return PassUser;
        }

        public void SetPassUser(string passUser) {
            CrossSecureStorage.Current.SetValue("PassUser", passUser);
            PassUser = passUser;
        }


        public string GetLangUser() {
            if(LangUser == null) {
                LangUser = "English";
            }
            return LangUser;
        }

        public void SetLangUser(string langUser) {
            LangUser = langUser;
        }

    }



}
