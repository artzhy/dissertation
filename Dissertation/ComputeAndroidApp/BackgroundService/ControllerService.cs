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
using System.Threading;


namespace ComputeAndroidApp.BackgroundService {
    [Service]
    public class ControllerService : Service {
        private ControllerServiceBinder binder;
        // Service variables here.
        private List<WorkOrderWS.WorkOrderTrimmed> SlaveWorkItems;

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

        public void ReceiveWorkOrderResult(int workOrderId) {
            WorkOrderWS.WorkOrderTrimmed wo = new WorkOrderWS.WorkOrderSvc().GetWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, workOrderId, true);

            // TODO: Send to listener (intent attached to application)
            Intent newIntent = new Intent(wo.ApplicationUIResultIntentk__BackingField);

            newIntent.PutExtra("WorkOrderTrimmed", JsonConvert.SerializeObject(wo));

            this.ApplicationContext.SendBroadcast(newIntent);

        }



        public void RequestWorkOrderComputation(object o) {

            Intent i = (Intent)o;
            String a = i.GetStringExtra("CommPackage");
            ComputeAndroidSDK.Communication.CommPackage cp = ComputeAndroidSDK.Communication.CommPackage.DeserializeJson(a);


            try {
                new WorkOrderWS.WorkOrderSvc().CreateWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, cp.ApplicationId, true, cp.SerializeParamList(), "ProcessRequest");
            } catch (Exception e) {
                Log.Error("RequestWorkOrderComputation", e.Message);
            }
        }

        public void ReceiveWorkOrderUpdateRequest(int workOrderId) {
            WorkOrderWS.WorkOrderTrimmed wo = new WorkOrderWS.WorkOrderSvc().GetWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, workOrderId, true);
           

            // TODO: handle it
        }

        public void AddSlaveWorkItem(int workOrderId) {
            // Get it
            WorkOrderWS.WorkOrderTrimmed wo = new WorkOrderWS.WorkOrderSvc().GetWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, workOrderId, true);

            // Acknowledge it
            new WorkOrderWS.WorkOrderSvc().AcknowledgeWorkOrder(App.GetAuthToken(this), workOrderId, true);
            // Submit comm package to Background service

            Intent doWork = new Intent();
            
            doWork.PutExtra("CommPackage", System.Text.RegularExpressions.Regex.Unescape(wo.CommPackageJsonk__BackingField));
            doWork.SetAction(wo.ComputeAppIntentk__BackingField);
            this.ApplicationContext.SendBroadcast(doWork);   

            // Store it
            wo.WorkOrderStatusk__BackingField = "SUBMITTED_TO_APP";
            SlaveWorkItems.Add(wo);

        }

        //public void DoCommand(String IntentAction) {
        //    Intent test = new Intent();

        //    CommPackage pkg = new CommPackage();
        //    pkg.ComputationRequestId = 1;
        //    List<CommPackage.ParamListItem> list = new List<CommPackage.ParamListItem>();
        //    list.Add(new CommPackage.ParamListItem("Param1", "VAlue1"));

        //    pkg.ParameterList = list;
        //    pkg.IntentAction = "intent.action";

        //    test.PutExtra("CommPackage", pkg.SerializeJson());
        //    test.SetAction(IntentAction);
        //    this.ApplicationContext.SendBroadcast(test);           
        //}

        public override void OnCreate() {
            base.OnCreate();

            SlaveWorkItems = new List<WorkOrderWS.WorkOrderTrimmed>();

        }

        public override void OnStart(Intent intent, int startId) {
            base.OnStart(intent, startId);
         
        }

        public override void OnDestroy() {
            base.OnDestroy();

           
        }



    }

 
}