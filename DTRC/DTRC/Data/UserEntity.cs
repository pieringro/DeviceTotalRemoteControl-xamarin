using DTRC.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Data {
    public class UserEntity {
        public delegate void Callback(bool result, string errorMessage);

        public string EmailUser { get; set; }

        public string PassUser { get; set; }

        public string LastErrorMessage;

        public async Task<bool> Login(Callback loginCallback = null){
            bool loginResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetEmailUser(EmailUser)
                .SetPassUser(PassUser)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.Instance.server_url_login, request);

            Response response = ServerResponse.ParsingJsonResponse(serverResponse);

            if (!response.Error) {
                Debug.WriteLine(string.Format("Login avvenuto con successo"));
                loginResult = true;
            }
            else {
                Debug.WriteLine(string.Format("Il server ha restituito un errore. Messaggio : {0}",
                        response.Message));
                loginResult = false;
            }

            LastErrorMessage = response.Message;
            loginCallback(loginResult, LastErrorMessage);
            return loginResult;
        }

        public async Task<bool> SignUp(Callback signUpCallback = null){
            bool signupResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetEmailUser(EmailUser)
                .SetPassUser(PassUser)
                .Build();
            
            ServerRequest serverRequest = new ServerRequest();

            string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.Instance.server_url_new_user, request);

            Response response = ServerResponse.ParsingJsonResponse(serverResponse);

            if (!response.Error) {
                Debug.WriteLine(string.Format("Registrazione utente avvenuta con successo"));
                signupResult = true;
            }
            else {
                Debug.WriteLine(string.Format("Il server ha restituito un errore. Messaggio : {0}",
                        response.Message));
                signupResult = false;
            }

            LastErrorMessage = response.Message;
            loginCallback(signupResult, LastErrorMessage);
            return signupResult;
        }

    }
}
