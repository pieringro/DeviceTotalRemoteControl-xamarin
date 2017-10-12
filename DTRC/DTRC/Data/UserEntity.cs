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

        public string Email { get; set; }

        public string Pass { get; set; }

        public string Lang { get; set; }

        public string LastErrorMessage;

        public async Task<bool> LoginAsync(Callback loginCallback = null) {
            bool loginResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetEmail(Email)
                .SetPass(Pass)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            try {
                string serverResponse = await serverRequest.SendDataToServerAsync(
                    ServerConfig.Instance.server_url_login, request);

                Response response = ServerResponse.ParsingJsonResponse(serverResponse);

                if (!response.Error) {
                    Debug.WriteLine(string.Format("Login avvenuto con successo"));
                    loginResult = true;
                }
                else {
                    Debug.WriteLine(string.Format("Il server ha restituito un errore durante la login. Messaggio : {0}",
                            response.Message));
                    loginResult = false;
                }

                LastErrorMessage = response.Message;
            }
            catch (Exception e) {
                string errorMsg = "Error connecting while \"logging in\"\r\n";
                errorMsg += e.Message;
                Debug.WriteLine(e.StackTrace);

                loginResult = false;
                LastErrorMessage = errorMsg;
            }

            loginCallback?.Invoke(loginResult, LastErrorMessage);
            return loginResult;
        }


        public async void Login(Callback loginCallback) {
            await this.LoginAsync(loginCallback);
        }

        public async Task<bool> SignUpAsync(Callback signUpCallback = null){
            bool signupResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetEmail(Email)
                .SetPass(Pass)
                .SetLang(Lang)
                .Build();
            
            ServerRequest serverRequest = new ServerRequest();

            try { 
                string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.Instance.server_url_new_user, request);

                Response response = ServerResponse.ParsingJsonResponse(serverResponse);

                if (!response.Error) {
                    Debug.WriteLine(string.Format("Registrazione utente avvenuta con successo"));
                    signupResult = true;
                }
                else {
                    Debug.WriteLine(string.Format("Il server ha restituito un errore durante la registrazione del nuovo utente. Messaggio : {0}",
                            response.Message));
                    signupResult = false;
                }

                LastErrorMessage = response.Message;

            }
            catch (Exception e) {
                string errorMsg = "Error connecting while \"adding new user\"\r\n";
                errorMsg += e.Message;
                Debug.WriteLine(e.StackTrace);

                signupResult = false;
                LastErrorMessage = errorMsg;
            }

            signUpCallback?.Invoke(signupResult, LastErrorMessage);
            return signupResult;
        }

    }
}
