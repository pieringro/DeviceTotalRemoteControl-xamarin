using DTRC.Data;
using DTRC.Services;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Xamarin.Forms;

namespace DTRC {
    public partial class App : Application {
        public static bool IsUserLoggedIn { get; set; }
        public static SystemConfig config;

        public App() {
            InitializeComponent();
            config = Xamarin.Forms.DependencyService.Get<SystemConfig>();
            if (!IsUserLoggedIn) {
                
                if (config.GetEmailUser() != null && config.GetPassUser() != null) {

                    UserEntity user = new UserEntity {
                        Email = config.GetEmailUser(),
                        Pass = config.GetPassUser()
                    };
                     
                    user.Login((result, errorMsg) => {
                        if (result) {
                            IsUserLoggedIn = true;
                            config.SetEmailUser(user.Email);
                            config.SetPassUser(user.Pass);
                            MainPage = new MainPage();
                        }
                        else {
                            MainPage = new NavigationPage(new Login());
                        }
                    });
                }
                
                MainPage = new NavigationPage(new Login());
            }
            else {
                MainPage = new MainPage();
            }
        }

        protected override void OnStart() {

        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
