using DTRC.Data;
using PCLAppConfig;
using PCLAppConfig.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DTRC {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage {
        public Login() {
            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e) {
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

                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else {
                messageLabel.Text = string.Format("Login failed. Message={0}", user.LastErrorMessage);
                passwordEntry.Text = string.Empty;
                bool clearDataStoredResult = App.config.ClearDataStored();
                if (!clearDataStoredResult) {
                    Debug.WriteLine("Error, unable to clear data storage");
                }
            }
        }
    }
}