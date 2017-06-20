using DTRC.Utility;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Server {

    /// <summary>
    /// {
    ///     "Device_tokenFirebase" : "<token_firebase_del_dispositivo>",
    ///     "Device_id" : "<id_del_dispositivo>"
    /// }
    /// </summary>
    public class Request {
        public string device_tokenFirebase;
        public string device_id;

        public string emailUser;
        public string passUser;
    }

    #region Request Builder
    public class RequestBuilder {

        private Request request;

        private void InitRequest() {
            if(request == null) {
                request = new Request();
            }
        }

        public RequestBuilder SetDevice_tokenFirebase(string device_tokenFirebase) {
            this.InitRequest();
            request.device_tokenFirebase = device_tokenFirebase;
            return this;
        }

        public RequestBuilder SetDevice_id(string device_id) {
            this.InitRequest();
            request.device_id = device_id;
            return this;
        }

        public RequestBuilder SetEmailUser(string emailUser) {
            this.InitRequest();
            request.emailUser = emailUser;
            return this;
        }

        public RequestBuilder SetPassUser(string passUser) {
            this.InitRequest();
            request.passUser = passUser;
            return this;
        }

        public Request Build() {
            return request;
        }
    }
    #endregion





    public class ServerRequest {

        public ServerRequest() {
            httpClient = new HttpClient();
        }

        private Request request;
        private string JsonRequest;
        private HttpClient httpClient;

        private void ConvertRequestToJson(Request request) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            JsonRequest = JsonConvert.SerializeObject(request, settings);
        }

        public delegate void CallbackOnFinished(string serverResponse);

        public async Task<string> SendFileToServerAsync(string filePath, string name,
            Request request, CallbackOnFinished callbackOnFinish = null) {

            this.ConvertRequestToJson(request);
            MultipartFormDataContent form = new MultipartFormDataContent();

            IFile fileToSend = await FileSystem.Current.GetFileFromPathAsync(filePath);
            Stream fileToSendStream = await fileToSend.OpenAsync(FileAccess.Read);
            byte[] fileToSendBytesArray = StorageUtility.ConvertStreamInBytesArray(fileToSendStream);

            form.Add(new StringContent(JsonRequest), "data");
            form.Add(new ByteArrayContent(fileToSendBytesArray, 0, fileToSendBytesArray.Length),
                name, filePath);
            HttpResponseMessage response = await httpClient.PostAsync(ServerConfig.Instance.server_url_send_pic, form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string serverResponse = response.Content.ReadAsStringAsync().Result;

            Debug.WriteLine("Server response = " + serverResponse);

            callbackOnFinish?.Invoke(serverResponse);

            return serverResponse;
        }


        public async Task<string> SendDataToServerAsync(string url, 
            Request request, CallbackOnFinished callbackOnFinish = null) {

            this.ConvertRequestToJson(request);
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("data", JsonRequest)
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string serverResponse = response.Content.ReadAsStringAsync().Result;

            Debug.WriteLine("Server response = " + serverResponse);

            callbackOnFinish?.Invoke(serverResponse);
            return serverResponse;
        }

        public async void SendDataToServer(string url, Request request, CallbackOnFinished callbackOnFinish) {
            await this.SendDataToServerAsync(url, request, callbackOnFinish);
        }

    }
}
