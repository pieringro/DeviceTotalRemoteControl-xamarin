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
                EmailUser = usernameEntry.Text,
                PassUser = passwordEntry.Text
            };

            bool loginResult = await user.Login();

            if (loginResult) {
                App.IsUserLoggedIn = true;
                App.config.SetEmailUser(user.EmailUser);
                App.config.SetPassUser(user.PassUser);
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