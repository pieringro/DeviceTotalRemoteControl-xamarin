using DTRC.Utility;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using DTRC.Server;

namespace DTRC.Services.Commands.TakePicture {
    
    public class WriteAndSendPicture {

        public delegate void CallbackOnFinished();

        private const string localFolderName = "PicturesPicked";

        private ServerRequest serverRequest;

        public WriteAndSendPicture() {
            serverRequest = new ServerRequest();
        }


        public async void WriteFileLocally(MemoryStream imageStream, string filename, 
            CallbackOnFinished callbackOnFinished) {

            IFolder localFolder = FileSystem.Current.LocalStorage;
            IFile localFile = null;
            localFolder = await localFolder.CreateFolderAsync(localFolderName, 
                CreationCollisionOption.OpenIfExists);
            
            bool fileCreated = false;
            int counter = 0;
            while (!fileCreated) {
                try {
                    filename += ".jpg";
                    localFile = await localFolder.CreateFileAsync(filename,
                        CreationCollisionOption.FailIfExists);
                    fileCreated = true;
                }
                catch (Exception e) {
                    //file exists
                    counter++;
                    filename = StorageUtility.UpdateFilename(filename, localFolder, counter);
                }
            }

            using (Stream stream = await localFile.OpenAsync(FileAccess.ReadAndWrite)) {
                imageStream.Position = 0;
                imageStream.CopyTo(stream);
                stream.Flush();
            }

            await SendFilePicToServer(localFile.Path, "file");

            //chiamare la callback operazione conclusa
            callbackOnFinished();
        }
        

        private async Task SendFilePicToServer(string filePath, string name) {
            try {
                RequestBuilder requestBuilder = new RequestBuilder();
                Request request = requestBuilder
                    .SetDevice_id(App.config.GetDeviceId())
                    .SetDevice_tokenFirebase(App.firebaseInstanceId.Token)
                    .Build();

                string responseString = await serverRequest.SendFileToServerAsync(filePath, name, request);
                Response response = ServerResponse.ParsingJsonResponse(responseString);
                if (!response.Error) {
                    Debug.WriteLine(string.Format("File {0} inviato con successo"));
                }
                else {
                    Debug.WriteLine(
                        string.Format("Il server ha restituito un errore durante l'invio del file {0}. Messaggio : {1}",
                            name, response.Message));
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(string.Format("Non e' stato possibile l'invio del file {0}.", name));
            }
        }

        
    }
}
