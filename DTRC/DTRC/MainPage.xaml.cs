using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DTRC.Data;
using System.Diagnostics;
using DTRC.Services.Commands;
using PCLStorage;

namespace DTRC {
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
            btnLogToken.Clicked += btnLogToken_Clicked;
            btnTryTakePictures.Clicked += btnTryTakePicture_Clicked;
            btnReadAllPrivateFiles.Clicked += btnReadAllPrivateFiles_Clicked;
        }

        public void btnLogToken_Clicked(object sender, EventArgs e) {
            FirebaseInstanceId firebaseInstanceId = DependencyService.Get<FirebaseInstanceId>();
            Debug.WriteLine(firebaseInstanceId.Token);
        }

        public async void btnTryTakePicture_Clicked(object sender, EventArgs e) {
            //ATakePictureCommand takePictureCmd = DependencyService.Get<ATakePictureCommand>();
            //await takePictureCmd.TakePicture();
            CommandDispatcher.getInstance().ExecuteCommand("TAKE_PICTURE");
        }

        public async void btnReadAllPrivateFiles_Clicked(object sender, EventArgs e) {
            IFolder localFolder = FileSystem.Current.LocalStorage;
            IList<IFile> files = await localFolder.GetFilesAsync();
            foreach(IFile aFile in files) {
                Debug.WriteLine(aFile.Path);
            }

            IList<IFolder> subfolders = await localFolder.GetFoldersAsync();
            foreach(IFolder aSubfolder in subfolders) {
                IList<IFile> subFolderFiles = await aSubfolder.GetFilesAsync();
                foreach (IFile aFile in subFolderFiles) {
                    Debug.WriteLine(aFile.Path);
                }
            }
        }
    }
}
