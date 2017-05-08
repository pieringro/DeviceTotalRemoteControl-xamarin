using DTRC.Utility;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DTRC.Services.Commands.TakePicture {
    
    public class WriteAndSendPicture {

        public delegate void CallbackOnFinished();

        private const string localFolderName = "PicturesPicked";


        public WriteAndSendPicture() {

        }


        public async void WriteFileLocally(MemoryStream imageStream, string filenameWithoutExt, 
            CallbackOnFinished callbackOnFinished) {

            IFolder localFolder = FileSystem.Current.LocalStorage;
            IFile localFile = null;
            localFolder = await localFolder.CreateFolderAsync(localFolderName, 
                CreationCollisionOption.OpenIfExists);
            
            bool fileCreated = false;
            int counter = 0;
            while (!fileCreated) {
                try {
                    filenameWithoutExt += ".jpg";
                    localFile = await localFolder.CreateFileAsync(filenameWithoutExt,
                        CreationCollisionOption.FailIfExists);
                    fileCreated = true;
                }
                catch (Exception e) {
                    //file exists
                    counter++;
                    filenameWithoutExt = StorageUtility.UpdateFilename(filenameWithoutExt, localFolder, counter);
                }
            }

            using (Stream stream = await localFile.OpenAsync(FileAccess.ReadAndWrite)) {
                imageStream.Position = 0;
                imageStream.CopyTo(stream);
                stream.Flush();
            }

            //chiamare la callback operazione conclusa
            callbackOnFinished();
        }
    }
}
