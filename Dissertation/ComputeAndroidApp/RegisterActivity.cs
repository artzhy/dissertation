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
    [Activity(Label = "Register")]
    public class RegisterActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Register);

            Button registerClick = FindViewById<Button>(Resource.Id.btnSubmitRegistration);

            registerClick.Click += registerClick_Click;

        }

        void registerClick_Click(object sender, EventArgs e) {
            String forename = FindViewById<EditText>(Resource.Id.txtForename).Text;
            String surname = FindViewById<EditText>(Resource.Id.txtSurname).Text;
            String username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            String password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            try {
                new UserWS.UserSvc().AddUser(forename, surname, username, password);

                App.ShowDialog("User created", "User successfully created", this);
                                
                StartActivity(typeof(LogonActivity));
            } catch (Exception ex) {
                App.ShowDialog("Error", ex.Message, this);
            }

        }
    }
}