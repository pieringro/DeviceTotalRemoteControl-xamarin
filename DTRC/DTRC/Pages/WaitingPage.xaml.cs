using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DTRC.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingPage : ContentPage {
        public WaitingPage() {
            InitializeComponent();
        }
        
        public string Loading {
            get{
                return Application.Current.Resources["loading"].ToString();
            }
        }
    }
}