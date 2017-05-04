using DTRC.Utility;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTRC.Services.Commands.TakePicture {

    public class WriteAndSendPicture {

        private const string localFolderName = "PicturesPicked";


        public WriteAndSendPicture(byte[] picBytes) {
            this.picBytes = picBytes;
        }

        private byte[] picBytes;
        

        public async void WriteFileLocally() {
            IFolder localFolder = FileSystem.Current.LocalStorage;
            localFolder = await localFolder.CreateFolderAsync(localFolderName, 
                CreationCollisionOption.OpenIfExists);

            string filename = "pic";
            filename = await this.UpdateFilename(filename, localFolder);

            IFile localFile = await localFolder.CreateFileAsync(filename, 
                CreationCollisionOption.ReplaceExisting);

            using (Stream stream = await localFile.OpenAsync(FileAccess.ReadAndWrite)) {
                stream.Write(picBytes, 0, picBytes.Length);
                stream.Flush();
            }
            
        }

        private async Task<string> UpdateFilename(string filename, IFolder rootFolder) {
            int counter = 0;
            string tmpFilename = filename;
            bool fileExists = await StorageUtility.IsFileExistAsync(filename, rootFolder);

            while (fileExists) {
                tmpFilename = filename + "_"+counter;
                fileExists = await StorageUtility.IsFileExistAsync(tmpFilename, rootFolder);
                counter++;
            }
            return tmpFilename;
        }







    }
}
