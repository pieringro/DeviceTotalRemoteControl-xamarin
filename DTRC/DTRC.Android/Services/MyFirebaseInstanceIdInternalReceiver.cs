using System.Diagnostics;
using Android.App;
using Firebase.Iid;
using DTRC.Server;
using DTRC.Utility;

namespace DTRC.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseInstanceIdInternalReceiver : FirebaseInstanceIdService {
        private const string TAG = "MyFirebaseInstanceIdInternalReceiver";

        private SystemConfig config;

        public MyFirebaseInstanceIdInternalReceiver() : base(){
            Debug.WriteLine("Constructor", TAG);
            config = Xamarin.Forms.DependencyService.Get<SystemConfig>();
        }


        public override void OnTokenRefresh() {
            string refreshedToken = FirebaseInstanceId.Instance.Token;
            Debug.WriteLine("Refreshed token: " + refreshedToken, TAG);
            UpdateTokenToServer(refreshedToken);
        }


        private void UpdateTokenToServer(string token) {
            // TODO mi collego al mio server e comunico il token di questo dispositivo
            Debug.WriteLine("SendRegistrationToServer("+token+") chiamata", TAG);
            
            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetDevice_id(config.GetDeviceId())
                .SetDevice_tokenFirebase(token)
                .SetEmailUser(config.GetEmailUser())
                .SetPassUser(config.GetPassUser())
                .Build();

            ServerRequest serverRequest = new ServerRequest();
            serverRequest.SendDataToServer(ServerConfig.SERVER_URL_UPDATE_TOKEN, request,
                (serverResponse) => {
                    Response response = ServerResponse.ParsingJsonResponse(serverResponse);

                    if (!response.Error) {
                        Debug.WriteLine(string.Format("Aggiornamento token avvenuto con successo"));
                    }
                    else {
                        Debug.WriteLine(string.Format("Il server ha restituito un errore. Messaggio : {0}",
                                response.Message));
                    }
                });
            
        }

        
    }
}
