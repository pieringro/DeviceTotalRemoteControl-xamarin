using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DTRC.Helpers;
using System.ComponentModel;

namespace DTRC.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage, INotifyPropertyChanged {
        public SettingsPage() {
            InitializeComponent();
            BindingContext = this;
        }


        public string EnableDisableCommands {
            get {
                return Application.Current.Resources["enable_disable_commands"].ToString();
            }
        }


        public string AllCommands {
            get {
                return Application.Current.Resources["all_commands"].ToString();
            }
        }


        private bool _allCommandsEnabled;
        public bool AllCommandsEnabled {
            get {
                return _allCommandsEnabled;
            }
            set {
                _allCommandsEnabled         = value;
                PlayBeepCommandEnabled      = value;
                RecordAudioCommandEnabled   = value;
                TakePictureCommandEnabled   = value;
            }
        }

        public string PlayBeepCommand {
            get {
                return Application.Current.Resources["play_beep_command"].ToString();
            }
        }

        public bool PlayBeepCommandEnabled {
            get {
                return Settings.PlayBeepEnabled;
            }
            set {
                Settings.PlayBeepEnabled = value;
                RaisePropertyChanged("PlayBeepCommandEnabled");
            }
        }


        public string RecordAudioCommand {
            get {
                return Application.Current.Resources["record_audio_command"].ToString();
            }
        }

        public bool RecordAudioCommandEnabled {
            get {
                return Settings.RecordAudioEnabled;
            }
            set {
                Settings.RecordAudioEnabled = value;
                RaisePropertyChanged("RecordAudioCommandEnabled");
            }
        }

        public string TakePictureCommand {
            get {
                return Application.Current.Resources["take_picture_command"].ToString();
            }
        }

        public bool TakePictureCommandEnabled {
            get {
                return Settings.TakePictureEnabled;
            }
            set {
                Settings.TakePictureEnabled = value;
                RaisePropertyChanged("TakePictureCommandEnabled");
            }
        }




        public new event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}