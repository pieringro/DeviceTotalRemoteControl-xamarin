using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DTRC.Data;
using System.Diagnostics;
using DTRC.Services.Commands;

namespace DTRC {
    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
            btnLogToken.Clicked += btnLogToken_Clicked;
            btnTryTakePictures.Clicked += btnTryTakePicture_Clicked;
        }

        public void btnLogToken_Clicked(object sender, EventArgs e) {
            FirebaseInstanceId firebaseInstanceId = DependencyService.Get<FirebaseInstanceId>();
            Debug.WriteLine("InstanceID token: " + firebaseInstanceId.Token);
        }

        public async void btnTryTakePicture_Clicked(object sender, EventArgs e) {
            ATakePictureCommand takePictureCmd = DependencyService.Get<ATakePictureCommand>();
            await takePictureCmd.TakePicture();
        }
    }
}
