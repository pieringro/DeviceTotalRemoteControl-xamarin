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
    public partial class Login : ContentPage {
        public Login() {
            InitializeComponent();
        }


        async void OnSignUpButtonClicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e) {
            var user = new User {
                EmailUser = usernameEntry.Text,
                PassUser = passwordEntry.Text
            };

            bool isValid = ExecuteFirstLogin(user);
            if (isValid) {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

        private bool ExecuteFirstLogin(User user) {
            //todo
            return true;
        }

    }
}