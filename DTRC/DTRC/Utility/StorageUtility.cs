using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;

namespace DTRC.Utility {
    public static class StorageUtility {
        
        public static string UpdateFilename(string filename, IFolder rootFolder, int? counter) {
            //rimuovo l'estensione nel filename se presente
            if (filename.Contains(".")) {
                filename = filename.Substring(0, filename.IndexOf('.'));
            }

            string filenameExtension = (counter.HasValue ? "_" + counter.Value.ToString() : string.Empty);
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

    }
}
