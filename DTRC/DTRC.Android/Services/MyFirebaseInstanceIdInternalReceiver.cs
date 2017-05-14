using System.Diagnostics;
using Android.App;
using Firebase.Iid;

namespace DTRC.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseInstanceIdInternalReceiver : FirebaseInstanceIdService {
        private const string TAG = "MyFirebaseInstanceIdInternalReceiver";

        public MyFirebaseInstanceIdInternalReceiver() : base(){
            Debug.WriteLine("Constructor", TAG);
        }


        public override void OnTokenRefresh() {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Debug.WriteLine("Refreshed token: " + refreshedToken, TAG);
            SendRegistrationToServer(refreshedToken);
        }


        private void SendRegistrationToServer(string token) {
            // TODO mi collego al mio server e comunico il token di questo dispositivo
            Debug.WriteLine("SendRegistrationToServer("+token+") chiamata", TAG);

            
        }
    }
}
