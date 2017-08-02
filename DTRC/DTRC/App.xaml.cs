using DTRC.Data;
using DTRC.Services;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using Xamarin.Forms;

namespace DTRC {
    public partial class App : Application {
        public static bool IsUserLoggedIn { get; set; }
        public static SystemConfig config;
        public static FirebaseInstanceId firebaseInstanceId;

        public App() {
            InitializeComponent();
            config = DependencyService.Get<SystemConfig>();
            firebaseInstanceId = DependencyService.Get<FirebaseInstanceId>();

            if (!IsUserLoggedIn) {
                if (config.GetEmailUser() != null && config.GetPassUser() != null) {
                    MainPage = new WaitingPage();
                    
                    DoAutomaticLogin();
                }
            }
            else {
                MainPage = new MainPage();
            }
        }


        private async void DoAutomaticLogin() {
            UserEntity user = new UserEntity {
                Email = config.GetEmailUser(),
                Pass = config.GetPassUser()
            };
            await System.Threading.Tasks.Task.Delay(10000);
            bool loginResult = await user.LoginAsync();

            if (loginResult) {
                IsUserLoggedIn = true;
                config.SetEmailUser(user.Email);
                config.SetPassUser(user.Pass);

                DeviceEntity device = new DeviceEntity {
                    DeviceId = App.config.GetDeviceId(),
                    DeviceToken = App.firebaseInstanceId.Token,
                    User = user
                };

                bool updateTokenResult = await device.UpdateTokenAsync();
                if (updateTokenResult) {
                    MainPage = new MainPage();
                }
                else {
                    //update token fallito, provo con nuovo device
                    bool newDeviceResult = await device.NewDeviceOrUpdateTokenIfExistsAsync();

                    if (newDeviceResult) {
                        MainPage = new NavigationPage(new Login());
                    }
                }
            }
            else {
                MainPage = new NavigationPage(new Login());
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
