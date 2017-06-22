﻿using DTRC.Server;
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
                    Debug.WriteLine(string.Format("Il server ha restituito un errore. Messaggio : {0}",
                            response.Message));
                    loginResult = false;
                }

                LastErrorMessage = response.Message;
            }
            catch (Exception e) {
                string errorMsg = "Error connecting to \"" + ServerConfig.Instance.server_url_login + "\"\r\n";
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







        public void SignUp() {

        }

    }
}
