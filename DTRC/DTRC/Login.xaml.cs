using DTRC.Data;
using PCLAppConfig;
using PCLAppConfig.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DTRC {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage , INotifyPropertyChanged {
        public Login() {
            InitializeComponent();
            IsWaiting = false;
            BindingContext = this;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e) {
            IsWaiting = true;

            UserEntity user = new UserEntity {
                Email = usernameEntry.Text,
                Pass = passwordEntry.Text
            };
            
            bool loginResult = await user.LoginAsync();

            if (loginResult) {
                App.IsUserLoggedIn = true;
                App.config.SetEmailUser(user.Email);
                App.config.SetPassUser(user.Pass);

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
            else {
                messageLabel.Text = string.Format("Login failed. Message={0}", user.LastErrorMessage);
                passwordEntry.Text = string.Empty;
                bool clearDataStoredResult = App.config.ClearDataStored();
                if (!clearDataStoredResult) {
                    Debug.WriteLine("Error, unable to clear data storage");
                }
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

        
        public string SignUp {
            get {
                return Application.Current.Resources["sign_up"].ToString();
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