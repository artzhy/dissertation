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

namespace ComputeAndroidApp.BackgroundService {
    [Service]
    public class ControllerService : Service {
        private ControllerServiceBinder binder;
        // Service variables here.
        


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

            Common.CommPackage pkg = new Common.CommPackage();
            pkg.ComputationRequestId = 1;
            List<ComputeAndroidApp.Common.CommPackage.ParamListItem> list = new List<ComputeAndroidApp.Common.CommPackage.ParamListItem>();
           list.Add(new Common.CommPackage.ParamListItem("Param1", "VAlue1"));

            pkg.ParameterList = list;
            pkg.IntentAction = "intent.action";


            
           // Newtonsoft.Json.JsonSerializer.Create().Serialize(

            string json = JsonConvert.SerializeObject(pkg, Formatting.Indented, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat  = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            });
            
      //      test.PutExtra("params", JsonConvert.SerializeObject(lst));

            test.SetAction(IntentAction);
            this.ApplicationContext.SendBroadcast(test);           
        }

        public override void OnCreate() {
            base.OnCreate();
            App.HandleException(new Exception("Here!"), this, false);
            // ...
        }

        public override void OnStart(Intent intent, int startId) {
            base.OnStart(intent, startId);
            App.HandleException(new Exception("Here!"), this, false);

            // ...
        }

        public override void OnDestroy() {
            base.OnDestroy();

            // ...
        } 
    }

 
}