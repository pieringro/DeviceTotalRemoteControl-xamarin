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
        
        public UserEntity User { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceId { get; set; }

        public string LastErrorMessage;

        public async Task<bool> UpdateTokenAsync(Callback updateTokenCallback = null) {
            bool updateTokenResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetDevice_id(DeviceId)
                .SetDevice_tokenFirebase(DeviceToken)
                .SetEmailUser(User.EmailUser)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.SERVER_URL_UPDATE_TOKEN, request);

            Response response = ServerResponse.ParsingJsonResponse(serverResponse);

            if (!response.Error) {
                Debug.WriteLine(string.Format("Token aggiornato con successo"));
                updateTokenResult = true;
            }
            else {
                Debug.WriteLine(string.Format("Il server ha restituito un errore durante l'aggiornamento del token. "+
                    "Messaggio : {0}", response.Message));
                updateTokenResult = false;
            }

            LastErrorMessage = response.Message;
            updateTokenCallback(updateTokenResult, LastErrorMessage);
            return updateTokenResult;
        }

        public async void UpdateToken(Callback updateTokenCallback) {
            await this.UpdateTokenAsync(updateTokenCallback);
        }





        public async Task<bool> NewDeviceAsync(Callback newDeviceCallback = null) {
            bool newDeviceResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetDevice_id(DeviceId)
                .SetDevice_tokenFirebase(DeviceToken)
                .SetEmailUser(User.EmailUser)
                .SetPassUser(User.PassUser)
                .Build();

            ServerRequest serverRequest = new ServerRequest();
            string serverResponse = await serverRequest.SendDataToServerAsync(ServerConfig.SERVER_URL_NEW_DEVICE, request);
            Response response = ServerResponse.ParsingJsonResponse(serverResponse);

            if (!response.Error) {
                Debug.WriteLine(string.Format("Nuovo dispositivo aggiunto con successo"));
                newDeviceResult = true;
            }
            else {
                Debug.WriteLine(string.Format("Il server ha restituito un errore durante l'aggiunta del nuovo dispositivo."+
                    "Messaggio : {0}", response.Message));
                newDeviceResult = false;
            }

            if (!newDeviceResult && response.Message != null && response.Message == SystemErrors.DEVICE_EXISTS) {
                return await this.UpdateTokenAsync(newDeviceCallback);
            }
            
            LastErrorMessage = response.Message;
            newDeviceCallback(newDeviceResult, LastErrorMessage);
            return newDeviceResult;
        }

        public async void NewDevice(Callback newDeviceCallback) {
            await this.NewDeviceAsync(newDeviceCallback);
        }



    }
}
