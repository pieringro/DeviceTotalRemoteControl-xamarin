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
using DTRC.Services;

namespace DTRC.Pages {
    public partial class MainPage : ContentPage {

        private SystemConfig config;

        public MainPage() {
            InitializeComponent();
            BindingContext = this;
            Debug.WriteLine(Application.Current.Resources["welcome_service_ready"]);

            if (config == null) {
                config = DependencyService.Get<SystemConfig>();
            }

            #region debug buttons
            if (IsDebugging) {
                InitDebugButtons();
            }
            #endregion
        }


        async void OnSettingsButtonClicked(object sender, EventArgs e) {
            await ((App)App.Current).OpenSettingsPage(this);
        }


        public string SettingsPage {
            get {
                return Application.Current.Resources["settings_page"].ToString();
            }
        }


        public string WelcomeServiceReady {
            get {
                return Application.Current.Resources["welcome_service_ready"].ToString();
            }
        }

        public string WelcomeUsername {
            get {
                if (config == null) {
                    config = DependencyService.Get<SystemConfig>();
                }
                return string.Format(Application.Current.Resources["welcome_username"].ToString(),
                    config.GetEmailUser());
            }
        }

        #region debug buttons

        private bool _isDebugging = false;
        public bool IsDebugging {
            get {
                return _isDebugging;
            }
        }

        private void InitDebugButtons() {
            btnLogToken.Clicked += btnLogToken_Clicked;
            btnTryTakePictures.Clicked += btnTryTakePicture_Clicked;
            btnStartStopRecording.Clicked += btnStartStopRecording_Clicked;
            btnReadAllPrivateFiles.Clicked += btnReadAllPrivateFiles_Clicked;
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
