using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using DTRC.Droid.Data;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseInstanceId))]
namespace DTRC.Droid.Data {

    public class FirebaseInstanceId : DTRC.Data.FirebaseInstanceId {
        
        public FirebaseInstanceId() { }


        public override string Token {
            get {
                return Firebase.Iid.FirebaseInstanceId.Instance.Token;
            }
        }

    }

}