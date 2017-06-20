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
                MainPage = new NavigationPage(new Login());
            }
            else {
                MainPage = new MainPage();
            }
        }

        protected override void OnStart() {



            //set emailUser e passUser se esistono nella memoria di massa
            //effettua subito la login



        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
