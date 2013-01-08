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
using Android.Util;

namespace MessengerAppEx {
    [Service]
    class ComputeService : Service {
        private Messenger inMessenger = new Messenger(new IncomingHandler());
        private Messenger outMessenger;

        public ComputeService() {
            
        }

        public override IBinder OnBind(Intent intent) {
            if (intent.GetParcelableExtra("MESSENGER") != null) {
             this.outMessenger = (Messenger) intent.GetParcelableExtra("MESSENGER");
            } 

            return inMessenger.Binder;
        }

        class IncomingHandler : Handler {
            public override void HandleMessage(Message msg) {
 	          Log.Error(this.Class.DeclaringClass.Class.Name, "Handlign message, ComputeService");
            }
        }
      }
}