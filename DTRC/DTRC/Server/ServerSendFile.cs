using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DTRC.Server {
    public class ServerSendFile {
        

        public async Task<bool> SendFileAudioToServer(string serverUrl, string filePath, string name, 
            ServerSendFileCallback callback = null) {
            bool result = true;
            string message = null;
            try {
                ServerRequest serverRequest = new ServerRequest();

                RequestBuilder requestBuilder = new RequestBuilder();
                Request request = requestBuilder
                    .SetDevice_id(App.config.GetDeviceId())
                    .SetDevice_tokenFirebase(App.firebaseInstanceId.Token)
                    .Build();

                string responseString = await serverRequest.SendFileToServerAsync(serverUrl,
                    filePath, name, request);
                Response response = ServerResponse.ParsingJsonResponse(responseString);
                if (!response.Error) {
                    Debug.WriteLine(string.Format("File {0} inviato con successo", filePath));
                    result = true;
                }
                else {
                    result = false;
                    message = string.Format("Il server ha restituito un errore durante l'invio del file {0}. Messaggio : {1}",
                            name, response.Message);
                    Debug.WriteLine(message);
                }
            }
            catch (Exception e) {
                result = false;
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine(string.Format("Non e' stato possibile l'invio del file {0}.", name));
            }

            callback?.Invoke(result, message);

            return result;
        }



    }
}
