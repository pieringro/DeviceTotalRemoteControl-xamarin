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
        private ServerSendFile _serverSendFile;

        public WriteAndSendPicture() {
            serverRequest = new ServerRequest();
            _serverSendFile = new ServerSendFile();
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
                    filename = StorageUtility.UpdateFilename(filename, counter);
                }
            }

            using (Stream stream = await localFile.OpenAsync(FileAccess.ReadAndWrite)) {
                imageStream.Position = 0;
                imageStream.CopyTo(stream);
                stream.Flush();
            }

            bool sendingResult = await SendFilePicToServer(localFile.Path, "file");

            //chiamare la callback operazione conclusa
            callbackOnFinished();
        }
        

        private async Task<bool> SendFilePicToServer(string filePath, string name) {
            bool result = false;

            result = 
                await _serverSendFile.SendGenericFileToServer(ServerConfig.Instance.server_url_send_pic, filePath, name);

            return result;

        }

        
    }
}
