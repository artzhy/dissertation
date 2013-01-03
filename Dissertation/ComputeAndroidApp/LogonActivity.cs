using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;

namespace ComputeAndroidApp {
    [Activity(Label = "ComputeAndroidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class LogonActivity : Activity {
        
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button buttonRegister = FindViewById<Button>(Resource.Id.btnRegister);
            Context context = this;
            buttonRegister.Click += delegate {
                StartActivity(typeof(RegisterActivity));
                
            };


            Button buttonLogin = FindViewById<Button>(Resource.Id.btnLogin);
            buttonLogin.Click += buttonLogin_Click;
          
       /*     SharedPreferences userDetails = context.getSharedPreferences("userdetails", MODE_PRIVATE);
            Editor edit = userDetails.edit();
            edit.clear();
            edit.putString("username", txtUname.getText().toString().trim());
            edit.putString("password", txtPass.getText().toString().trim());
            edit.commit();*/

        }

        void buttonLogin_Click(object sender, EventArgs e) {
            String username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            String password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            Boolean success, success1;

            new AuthWS.AuthSvc().AuthenticateUser(username, password, out success, out  success1);

            if (success) {

                ISharedPreferences prefs = this.GetSharedPreferences(this.ApplicationContext.PackageName, FileCreationMode.Private);

                prefs.Edit().PutString("Username", username).Commit();
                prefs.Edit().PutString("Password", password).Commit();


            } else {
                App.ShowDialog("Invalid login", "Invalid login details.", this);
            }

        }
    }
}

