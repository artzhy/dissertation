using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace com.MessengerAppEx {
    [Activity(Label = "MessengerAppEx", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, com.MessengerAppEx.ISvcComm {

        Messenger ReplyMessenger = new Messenger(new com.MessengerAppEx.Activity1.IncomingHandler());
        IServiceConnection sc;
        Messenger _messenger;

        Messenger ISvcComm.messenger {
            get {
                return _messenger;
            }
            set {
                _messenger = value;
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
          
            sc = new com.MessengerAppEx.ServiceConnection(this);
       
        }

        void button_Click(object sender, EventArgs e) {
            Message msg = Message.Obtain();
            Bundle bundle = new Bundle();
            bundle.PutString("arg1", "index.html");
            msg.Data = bundle;
            msg.ReplyTo = ReplyMessenger;
            _messenger.Send(msg);
            
        }

        protected override void OnResume() {
            base.OnResume();

            // Resume services
            Intent intent = null;
            intent = new Intent("com.test.service");
         
            this.ApplicationContext.BindService(intent, sc, Bind.AutoCreate);
        }

        protected override void OnPause() {
            base.OnPause();
            UnbindService(sc);
        }
        
        class IncomingHandler : Handler {
            public override void HandleMessage(Message msg) {

                Message msg1 = Message.Obtain();
                Bundle bundle = new Bundle();
                bundle.PutString("arg1", "index.html");
                msg1.Data = bundle;
                Messenger replyto = new Messenger(msg.ReplyTo.Binder);
                replyto.Send(msg1);

                // Log.Error(this.Class.DeclaringClass.Class.Name, "Handlign message, ComputeService");
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

