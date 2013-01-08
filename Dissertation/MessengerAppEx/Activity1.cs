using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MessengerAppEx {
    [Activity(Label = "MessengerAppEx", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, MessengerAppEx.ISvcComm {
        int count = 1;
        Bundle MessengerBundle = new Bundle();
        Messenger _messenger = null;
        IServiceConnection sc;
        private Handler handler = new Handler();

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += button_Click;


            sc = new MessengerAppEx.ServiceConnection(this);

            Intent test = new Intent(this, typeof(ComputeService));

            this.ApplicationContext.StartService(test);
            test.PutExtra("MESSENGER", messenger);

            this.ApplicationContext.BindService(test, sc, Bind.AutoCreate);
            int te = 0;

        }

        void button_Click(object sender, EventArgs e) {
            Message msg = Message.Obtain();
            Bundle bundle = new Bundle();
            bundle.PutString("arg1", "index.html");
            bundle.PutString("arg2", "http://www.vogella.com/index.html");
            msg.Data = bundle;
            messenger.Send(msg);
        }

        protected override void OnResume() {
            base.OnResume();

            // Resume services
            Intent intent = null;
            intent = new Intent(this, typeof(ComputeService));
            // Create a new Messenger for the communication back
            // From the Service to the Activity
            Messenger messenger = new Messenger(handler);
            intent.PutExtra("MESSENGER", messenger);

            this.ApplicationContext.BindService(intent, sc, Bind.AutoCreate);
        }

        protected override void OnPause() {
            base.OnPause();
            UnbindService(sc);
        }

       public Messenger messenger {
            get {
                return _messenger;
            }
            set {
                _messenger = value;
            }
        }
    }

    class CHandler : Handler {
        public void handleMessage(Message message) {
            Bundle data = message.Data;
            if (message.Arg1 == 1 && data != null) {
                //String text = data.GetString();
                //    Toast.MakeText(this, "here", ToastLength.Long).show();
            }
        }
    }

    class ServiceConnection : Java.Lang.Object, IServiceConnection {
        public Messenger messenger;
        public ISvcComm isvcomm;

        public ServiceConnection(ISvcComm act) {
            isvcomm = act;
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            isvcomm.messenger = new Messenger(service);
        }

        public void OnServiceDisconnected(ComponentName name) {
            isvcomm.messenger = null;
        }
    }
}

