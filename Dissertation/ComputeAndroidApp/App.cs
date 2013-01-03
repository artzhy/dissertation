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

namespace ComputeAndroidApp {
    class App {
        private static UserWS.UserSvc _userWS;


        public static UserWS.UserSvc UserWS {
            get { 
                if(_userWS == null) {
                    _userWS = new UserWS.UserSvc();
                }
                
                return _userWS;
            }
        }
        
        public static void ShowDialog(String title, String message, Context context) {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetCancelable(true);
                builder.SetTitle(title);
                builder.SetInverseBackgroundForced(true);
                builder.SetMessage(message);
                builder.SetPositiveButton("OK", delegate {
                   
                });
                AlertDialog alert = builder.Create();
                alert.Show();
        }
    }
}