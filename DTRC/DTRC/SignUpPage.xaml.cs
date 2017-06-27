using DTRC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DTRC {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage {
        public SignUpPage() {
            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e) {
            UserEntity user = new UserEntity {
                Email = emailEntry.Text,
                Pass = passwordEntry.Text
            };

            bool signupResult = await user.SignUpAsync();

            if(signupResult){
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
                
                Application.Current.MainPage = new MainPage();
                await Navigation.PopToRootAsync();
            }
            else{
                messageLabel.Text = string.Format("Sign Up failed. Message={0}", user.LastErrorMessage);
            }
        }
    }
}