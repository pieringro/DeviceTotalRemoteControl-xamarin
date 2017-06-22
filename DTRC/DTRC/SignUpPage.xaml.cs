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

        private void OnSignUpButtonClicked(object sender, EventArgs e) {
            UserEntity user = new UserEntity {
                EmailUser = emailEntry.Text,
                PassUser = passwordEntry.Text
            };

            bool signupResult = user.SignUp();

            if(signupResult){
                App.IsUserLoggedIn = true;
                App.config.SetEmailUser(user.EmailUser);
                App.config.SetPassUser(user.PassUser);
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else{

            }
        }
    }
}