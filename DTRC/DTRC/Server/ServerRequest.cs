﻿using DTRC.Utility;
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

        public string email;
        public string pass;
        public string lang;

        public string length;
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

        public RequestBuilder SetEmail(string email) {
            this.InitRequest();
            request.email = email;
            return this;
        }

        public RequestBuilder SetPass(string pass) {
            this.InitRequest();
            request.pass = pass;
            return this;
        }

        public RequestBuilder SetLang(string lang) {
            this.InitRequest();
            request.lang = lang;
            return this;
        }

        public RequestBuilder SetLength(string length) {
            this.InitRequest();
            request.length = length;
            return this;
        }

        public Request Build() {
            return request;
        }
    }
    #endregion





    public class ServerRequest {

        public ServerRequest() {
            InitHttpClient();
        }

        private static readonly string API_KEY = "gfju5789906t5656yjsd_@rrqr";

        private string JsonRequest;
        private HttpClient httpClient;

        private void InitHttpClient() {
            httpClient = new HttpClient();
        }

        private void ConvertRequestToJson(Request request) {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            JsonRequest = JsonConvert.SerializeObject(request, settings);
        }

        public delegate void CallbackOnFinished(string serverResponse);

        public async Task<string> SendFileToServerAsync(string url, string filePath, string name,
            Request request, CallbackOnFinished callbackOnFinish = null) {

            InitHttpClient();
            this.ConvertRequestToJson(request);
            MultipartFormDataContent form = new MultipartFormDataContent();

            IFile fileToSend = await FileSystem.Current.GetFileFromPathAsync(filePath);
            Stream fileToSendStream = await fileToSend.OpenAsync(FileAccess.Read);
            byte[] fileToSendBytesArray = StorageUtility.ConvertStreamInBytesArray(fileToSendStream);

            form.Add(new StringContent(API_KEY), "apikey");
            form.Add(new StringContent(JsonRequest), "data");
            form.Add(new ByteArrayContent(fileToSendBytesArray, 0, fileToSendBytesArray.Length),
                name, filePath);
            Debug.WriteLine("Sending json : "+JsonRequest);
            HttpResponseMessage response = await httpClient.PostAsync(url, form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string serverResponse = response.Content.ReadAsStringAsync().Result;

            Debug.WriteLine("Server response = " + serverResponse);

            callbackOnFinish?.Invoke(serverResponse);

            return serverResponse;
        }


        public async Task<string> SendDataToServerAsync(string url, 
            Request request, CallbackOnFinished callbackOnFinish = null) {

            InitHttpClient();
            this.ConvertRequestToJson(request);
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("data", JsonRequest),
                new KeyValuePair<string, string>("apikey", API_KEY)
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);
            Debug.WriteLine("Sending json : " + JsonRequest);
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
