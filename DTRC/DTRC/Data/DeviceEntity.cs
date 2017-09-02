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
                .SetEmail(User.Email)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            try {
                string serverResponse = await serverRequest.SendDataToServerAsync(
                    ServerConfig.Instance.server_url_update_token, request);

                Response response = ServerResponse.ParsingJsonResponse(serverResponse);

                if (!response.Error) {
                    Debug.WriteLine(string.Format("Token aggiornato con successo"));
                    updateTokenResult = true;
                }
                else {
                    Debug.WriteLine(string.Format("Il server ha restituito un errore durante l'aggiornamento del token. " +
                        "Messaggio : {0}", response.Message));
                    updateTokenResult = false;
                }

                LastErrorMessage = response.Message;

            }
            catch(Exception e) {
                string errorMsg = "Error connecting to \"updating token\"\r\n";
                errorMsg += e.Message;
                Debug.WriteLine(e.StackTrace);

                updateTokenResult = false;
                LastErrorMessage = errorMsg;
            }

            updateTokenCallback?.Invoke(updateTokenResult, LastErrorMessage);
            return updateTokenResult;
        }

        public async void UpdateToken(Callback updateTokenCallback) {
            await this.UpdateTokenAsync(updateTokenCallback);
        }





        public async Task<bool> NewDeviceOrUpdateTokenIfExistsAsync(Callback newDeviceCallback = null) {
            bool newDeviceResult;

            RequestBuilder requestBuilder = new RequestBuilder();
            Request request = requestBuilder
                .SetDevice_id(DeviceId)
                .SetDevice_tokenFirebase(DeviceToken)
                .SetEmail(User.Email)
                .SetPass(User.Pass)
                .Build();

            ServerRequest serverRequest = new ServerRequest();

            try {
                string serverResponse = await serverRequest.SendDataToServerAsync(
                    ServerConfig.Instance.server_url_new_device, request);
                Response response = ServerResponse.ParsingJsonResponse(serverResponse);

                if (!response.Error) {
                    Debug.WriteLine(string.Format("Nuovo dispositivo aggiunto con successo"));
                    newDeviceResult = true;
                }
                else {
                    Debug.WriteLine(string.Format("Il server ha restituito un errore durante l'aggiunta del nuovo dispositivo." +
                        "Messaggio : {0}", response.Message));
                    newDeviceResult = false;
                }

                if (!newDeviceResult && response.Message != null && response.Message == SystemErrors.DEVICE_EXISTS) {
                    return await this.UpdateTokenAsync(newDeviceCallback);
                }

                LastErrorMessage = response.Message;

            }
            catch (Exception e) {
                string errorMsg = "Error connecting to \"Adding new device\"\r\n";
                errorMsg += e.Message;
                Debug.WriteLine(e.StackTrace);

                newDeviceResult = false;
                LastErrorMessage = errorMsg;
            }


            newDeviceCallback?.Invoke(newDeviceResult, LastErrorMessage);
            return newDeviceResult;
        }

        public async void NewDeviceOrUpdateTokenIfExists(Callback newDeviceCallback) {
            await this.NewDeviceOrUpdateTokenIfExistsAsync(newDeviceCallback);
        }



    }
}
