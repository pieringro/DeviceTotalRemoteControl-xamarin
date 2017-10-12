using DTRC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DTRC.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage, INotifyPropertyChanged {
        public SignUpPage() {
            InitializeComponent();
            IsWaiting = false;
            BindingContext = this;

            if (this.langPicker.Items.Count == 0) {
                foreach (string lang in LangEntity.allLang) {
                    this.langPicker.Items.Add(lang);
                }
                langPicker.SelectedIndex = 0;
            }
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e) {
            IsWaiting = true;

            UserEntity user = new UserEntity {
                Email = emailEntry.Text,
                Pass = passwordEntry.Text,
                Lang = (string)langPicker.SelectedItem
            };

            bool signupResult = await user.SignUpAsync();

            if(signupResult){
                App.IsUserLoggedIn = true;
                App.config.SetEmailUser(user.Email);
                App.config.SetPassUser(user.Pass);
                App.config.SetLangUser((string)langPicker.SelectedItem);

                DeviceEntity device = new DeviceEntity {
                    DeviceId = App.config.GetDeviceId(),
                    DeviceToken = App.firebaseInstanceId.Token,
                    User = user
                };

                bool newDeviceResult = await device.NewDeviceOrUpdateTokenIfExistsAsync();

                if (!newDeviceResult) {
                    await DisplayAlert("Error",
                        string.Format("", device.LastErrorMessage), "OK");
                }
                else {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
                }
            }
            else{
                messageLabel.Text = string.Format("Sign Up failed. Message={0}", user.LastErrorMessage);
            }

            IsWaiting = false;
        }


        private bool _isWaiting;
        public bool IsWaiting {
            get {
                return _isWaiting;
            }
            set {
                _isWaiting = value;
                RaisePropertyChanged("IsWaiting");
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


    }
}