using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DTRC.Data;
using System.Diagnostics;

namespace DTRC {
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
            btnLogToken.Clicked += btnLogToken_Clicked;
        }

        public void btnLogToken_Clicked(object sender, EventArgs e) {
            FirebaseInstanceId firebaseInstanceId = DependencyService.Get<FirebaseInstanceId>();
            Debug.WriteLine("InstanceID token: " + firebaseInstanceId.Token);

        }
    }
}
