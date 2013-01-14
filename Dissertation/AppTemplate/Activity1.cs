using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AppTemplate {
    [Activity(Label = "AppTemplate", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, IComputeActivity {
        int count = 1;
        public ComputeFunction.ComputeServiceBinder _binder;
        public Boolean _binderSet ;

        public ComputeFunction.ComputeServiceBinder binder {
            get {
                return _binder;
            }
            set {
                _binder = value;
            }
        }

        public bool binderSet {
            get {
                return _binderSet;
            }
            set {
                _binderSet = value;
            }
        }


        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += button_Click;

            this.ApplicationContext.StartService(new Intent(this, typeof(ComputeFunction.ComputeService)));

            Intent serviceIntent = new Intent(this, typeof(ComputeFunction.ComputeService));
            IServiceConnection conn = new ComputeFunction.ServiceConnection(this);

            ApplicationContext.BindService(serviceIntent, conn, Bind.AutoCreate);
          
        }


        void button_Click(object sender, EventArgs e) {

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += button_Click;

            Dictionary<String, Object> parameters = new Dictionary<string,object>();

            parameters.Add("IntVal", 1);      

          Task<Object> result = Task.Factory.StartNew(
                 () => {
                     return binder.GetService().ComputeResult(3, parameters);
                 });

            result.ContinueWith((task) => RunOnUiThread( () => {
                if (task.IsFaulted)
                    Toast.MakeText(this,
                           task.Exception.ToString(), ToastLength.Long).Show();

              
                Toast.MakeText(this,
                            "Button is clicked" + task.Result.ToString(), ToastLength.Long).Show();

                }));

           
  

            

        //RunOnUiThread(() => (button.Text = (int)result.Result.ToString)
           int tst = 0;
        }



       
    }
}

