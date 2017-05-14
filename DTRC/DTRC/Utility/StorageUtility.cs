using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;
using System.IO;

namespace DTRC.Utility {
    public static class StorageUtility {

        private static string FILENAME_NUMBER_SEPARATOR = "_";
        public static string UpdateFilename(string filename, IFolder rootFolder, int? counter) {
            //rimuovo l'estensione nel filename se presente
            if (filename.Contains(".")) {
                filename = filename.Substring(0, filename.IndexOf('.'));
            }
            //rimuovo il contatore precedente se presente
            if (filename.Contains(FILENAME_NUMBER_SEPARATOR)) {
                filename = filename.Substring(0, filename.IndexOf(FILENAME_NUMBER_SEPARATOR));
            }

            string filenameExtension = (counter.HasValue ? FILENAME_NUMBER_SEPARATOR + counter.Value.ToString() : string.Empty);
            string tmpFilename = filename + filenameExtension;
            counter++;
            return tmpFilename;
        }


        public static async Task<bool> IsFileExistAsync(this string folderFileName, IFolder rootFolder = null) {
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(folderFileName);
            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FileExists) {
                return true;
            }
            return false;
        }

        public static byte[] ConvertStreamInBytesArray(Stream sourceStream) {
            using (var memoryStream = new MemoryStream()) {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }



    }
}
