using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;
using System.IO;
using System.Diagnostics;

namespace DTRC.Utility {
    public static class StorageUtility {

        private static string FILENAME_NUMBER_SEPARATOR = "_";
        public static string UpdateFilename(string filename, int? counter) {
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

        public static async Task<bool> IsFileExistAsync(this string folderFileName, string rootFolderName = null) {
            IFolder folder = null;
            if (rootFolderName == null) {
                folder = FileSystem.Current.LocalStorage;
            }
            else {
                folder = await FileSystem.Current.LocalStorage.CreateFolderAsync(rootFolderName,
                   CreationCollisionOption.OpenIfExists);
            }
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


        public static async Task<bool> CreateEmptyFileLocalStorage(string filename) {
            bool result = true;

            try {
                IFolder localFolder = FileSystem.Current.LocalStorage;
                IFile localFile = await localFolder.CreateFileAsync(filename,
                    CreationCollisionOption.FailIfExists);
            } catch (Exception e) {
                result = false;
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }

        public static async Task<bool> CreateEmptyFileInFolder(string folderName, string filename) {
            bool result = true;

            try {
                IFolder localFolder = FileSystem.Current.LocalStorage;
                localFolder = await localFolder.CreateFolderAsync(folderName, 
                    CreationCollisionOption.OpenIfExists);
                IFile localFile = await localFolder.CreateFileAsync(filename,
                    CreationCollisionOption.FailIfExists);
            }
            catch (Exception e) {
                result = false;
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }


        public static async Task<bool> DeleteFileInFolder(string folderName, string filename) {
            bool result = true;

            try {
                IFolder localFolder = FileSystem.Current.LocalStorage;
                localFolder = await localFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                IFile localFileToDelete = await localFolder.GetFileAsync(filename);
                if (localFileToDelete != null) {
                    await localFileToDelete.DeleteAsync();
                }
            }
            catch (Exception e) {
                result = false;
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }

        public static async Task<bool> DeleteFileLocalStorage(string filePath) {
            bool result = true;

            try {
                IFolder localFolder = FileSystem.Current.LocalStorage;
                IFile localFileToDelete = await localFolder.GetFileAsync(filePath);
                if (localFileToDelete != null) {
                    await localFileToDelete.DeleteAsync();
                }
            }
            catch (Exception e) {
                result = false;
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }

        public static string GetAppLocalPath() {
            return FileSystem.Current.LocalStorage.Path;
        }


    }
}
