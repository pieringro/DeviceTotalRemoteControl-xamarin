using DTRC.Server;
using DTRC.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Data {
    public class DeviceEntity {
        public delegate void Callback(bool result, string errorMessage);

        public string EmailUser { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceId { get; set; }

        public string LastErrorMessage;

        public async Task<bool> UpdateTokenAsync(Callback updateTokenCallback = null) {
            bool loginResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetDevice_id(DeviceId)
                .SetDevice_tokenFirebase(DeviceToken)
                .SetEmailUser(EmailUser)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.SERVER_URL_UPDATE_TOKEN, request);

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
            updateTokenCallback(loginResult, LastErrorMessage);
            return loginResult;
        }

        public async void UpdateToken(Callback updateTokenCallback) {
            await this.UpdateTokenAsync(updateTokenCallback);
        }



    }
}
