using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DTRC {
    public partial class App : Application {
        public static bool IsUserLoggedIn { get; set; }
        public static string EmailUser { get; set; }
        public static string PassUser { get; set; }

        public App() {
            InitializeComponent();
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
