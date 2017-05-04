using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;

namespace DTRC.Utility {
    public static class StorageUtility {

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
