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

using Newtonsoft.Json;
using ComputeAndroidSDK.Communication;

namespace ComputeAndroidApp.BackgroundService {
    [Service]
    public class ControllerService : Service {
        private ControllerServiceBinder binder;
        // Service variables here.
        private List<CommPackage> WorkItems;


        public override IBinder OnBind(Intent intent) {
           this.binder = new ControllerServiceBinder(this);
           return this.binder;
        }

        public void DoWork() {
            Log.Error("test", "here");
        }

        public void NotifyDeviceActive() {
           //  new UserWS.UserSvc().
        }

        public void DoCommand(String IntentAction) {
            Intent test = new Intent();

            CommPackage pkg = new CommPackage();
            pkg.ComputationRequestId = 1;
            List<CommPackage.ParamListItem> list = new List<CommPackage.ParamListItem>();
            list.Add(new CommPackage.ParamListItem("Param1", "VAlue1"));

            pkg.ParameterList = list;
            pkg.IntentAction = "intent.action";

            test.PutExtra("CommPackage", pkg.SerializeJson());
            test.SetAction(IntentAction);
            this.ApplicationContext.SendBroadcast(test);           
        }

        public override void OnCreate() {
            base.OnCreate();
           
            WorkItems = new List<CommPackage>();

        }

        public override void OnStart(Intent intent, int startId) {
            base.OnStart(intent, startId);
         
        }

        public override void OnDestroy() {
            base.OnDestroy();

           
        } 
    }

 
}