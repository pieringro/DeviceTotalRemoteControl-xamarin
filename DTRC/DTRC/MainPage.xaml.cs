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
            BindingContext = this;

            #region debug buttons
            btnLogToken.Clicked += btnLogToken_Clicked;
            btnTryTakePictures.Clicked += btnTryTakePicture_Clicked;
            btnStartStopRecording.Clicked += btnStartStopRecording_Clicked;
            btnReadAllPrivateFiles.Clicked += btnReadAllPrivateFiles_Clicked;
            #endregion
        }


        #region debug buttons

        private bool _isDebugging = false;
        public bool IsDebugging {
            get {
                return _isDebugging;
            }
        }

        public void btnLogToken_Clicked(object sender, EventArgs e) {
            FirebaseInstanceId firebaseInstanceId = DependencyService.Get<FirebaseInstanceId>();
            Debug.WriteLine(firebaseInstanceId.Token);
        }


        public void btnTryTakePicture_Clicked(object sender, EventArgs e) {
            //CommandDispatcher.getInstance().ExecuteCommand("TAKE_PICTURE");

            Dictionary<string, string> commandParams = new Dictionary<string, string> {
                { "CommandId", "TAKE_PICTURE" },
                //{ "FrontPic" , "3" },
                { "BackPic" , "1" }
            };

            CommandDispatcher.getInstance().ExecuteCommandWithParams("TAKE_PICTURE", commandParams);
        }


        public void btnStartStopRecording_Clicked(object sender, EventArgs e) {

            Dictionary<string, string> commandParams = new Dictionary<string, string> {
                { "CommandId", "RECORD_AUDIO" },
                { "Timer" , "00:00:05" }
            };

            CommandDispatcher.getInstance().ExecuteCommandWithParams("RECORD_AUDIO", commandParams);
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

        #endregion

    }
}
