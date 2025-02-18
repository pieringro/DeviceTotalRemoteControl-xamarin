﻿using System.Diagnostics;
using Android.App;
using Firebase.Iid;
using DTRC.Data;

namespace DTRC.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseInstanceIdInternalReceiver : FirebaseInstanceIdService {
        private const string TAG = "MyFirebaseInstanceIdInternalReceiver";

        private SystemConfig config;

        public MyFirebaseInstanceIdInternalReceiver() : base(){
            Debug.WriteLine("Constructor", TAG);
            //config = Xamarin.Forms.DependencyService.Get<SystemConfig>();
            config = XLabs.Ioc.Resolver.Resolve<SystemConfig>();
        }

        public override void OnCreate() {
            base.OnCreate();
            DTRC.Droid.Utility.IOCContainer.InitFabricCrashlytics(this);
            DTRC.Droid.Utility.IOCContainer.InitXLabResolver();
            Debug.WriteLine("DTRC servizio creato. OnCreate", TAG);
        }

        public override void OnTokenRefresh() {
            string refreshedToken = Firebase.Iid.FirebaseInstanceId.Instance.Token;
            Debug.WriteLine("Refreshed token: " + refreshedToken, TAG);
            UpdateTokenToServer(refreshedToken);
        }


        private void UpdateTokenToServer(string token) {
            // mi collego al mio server e comunico il token di questo dispositivo

            if (config.GetEmailUser() != null) {
                Debug.WriteLine(string.Format("SendRegistrationToServer({0}) chiamata", token), TAG);
                DeviceEntity device = new DeviceEntity {
                    DeviceId = config.GetDeviceId(),
                    DeviceToken = token,
                    User = new UserEntity { Email = config.GetEmailUser() }
                };
                device.UpdateToken((updateTokenResult, errorMessage) => {
                    if (updateTokenResult) {
                        Debug.WriteLine(string.Format("Aggiornamento token avvenuto con successo"));
                    }
                    else {
                        Debug.WriteLine(string.Format("Il server ha restituito un errore. Messaggio : {0}",
                            errorMessage));
                    }
                });
            }
        }

    }
}
