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
        private bool hasCommPackageGetThreadRunning;
        // Service variables here.
        private List<WorkOrderWS.WorkOrderTrimmed> SlaveWorkItems;

        public override IBinder OnBind(Intent intent) {
           this.binder = new ControllerServiceBinder(this);
           return this.binder;
        }

        public void StartCommFetch() {
            if (!hasCommPackageGetThreadRunning) {
                // start thread
                Thread fetchThread = new Thread(new ThreadStart(FetchComms));
                fetchThread.Start();
            }
        }

        public enum UpdateType {
            Result,
            UpdateRequest,
            Error,
            NewWorkOrder,
            Cancel,
            FetchRequest
        }

        public void FetchComms() {
            if (hasCommPackageGetThreadRunning)
                return;
            hasCommPackageGetThreadRunning = true;
            int deviceId = App.GetDeviceId(this);
            string authToken = App.GetAuthToken(this);
            // get comms and handle them
            int noSecsNoComms = 0;

            while (noSecsNoComms < 180) {
                List<WorkOrderWS.CommunicationPackage> cps = new WorkOrderWS.WorkOrderSvc().GetOutstandingCommunications(authToken, deviceId, true).ToList();

                if (cps.Count() == 0) {
                    noSecsNoComms += 20;
                } else {
                    // Handle all the comms
                    noSecsNoComms = 0;

                    foreach (WorkOrderWS.CommunicationPackage cp in cps) {
                        UpdateType ut = (UpdateType)cp.CommunicationTypek__BackingField;
                        int? workOrderId = cp.WorkOrderIdk__BackingField;
                        int commId = cp.CommunicationIdk__BackingField;

                        Log.Info(BroadcastReceiver.TAG, "Handling comm id: " + commId);

                        new WorkOrderWS.WorkOrderSvc().AcknowledgeCommunication(authToken, commId, true, DateTime.Now, true);

                        App.UpdateLastTransmit();

                        if (ut == UpdateType.NewWorkOrder) {
                            AddSlaveWorkItem(workOrderId.Value);
                            Log.Info(BroadcastReceiver.TAG, "Handling new work order id: " + workOrderId);
                        } else if (ut == UpdateType.Result) {
                            ReceiveWorkOrderResult(workOrderId.Value);
                            Log.Info(BroadcastReceiver.TAG, "Handling result work order id: " + workOrderId);
                        } else if (ut == UpdateType.UpdateRequest) {
                            //TODO: Handle update request

                        } else if (ut == UpdateType.Cancel) {
                            //TODO: Handle cancel request

                        } else if (ut == UpdateType.FetchRequest) {

                        }
                    }
                }
                // Sleep before checking again.
                Thread.Sleep(new TimeSpan(0, 0, 20));
            }

            hasCommPackageGetThreadRunning = false;
        }

        public void NotifyDeviceActive() {
           //  new UserWS.UserSvc().
        }

        public void ReceiveWorkOrderResult(int workOrderId) {
            WorkOrderWS.WorkOrderTrimmed wo = new WorkOrderWS.WorkOrderSvc().GetWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, workOrderId, true);

            ComputeAndroidSDK.Communication.WorkOrderTrimmed testWo = new WorkOrderTrimmed();
            testWo.ApplicationId = wo.ApplicationIdk__BackingField;
            testWo.ApplicationUIResultIntent = wo.ApplicationUIResultIntentk__BackingField;
            testWo.CommPackageJson = wo.CommPackageJsonk__BackingField;
            testWo.ComputeAppIntent = wo.ComputeAppIntentk__BackingField;
            testWo.DeviceId = wo.DeviceIdk__BackingField;
            testWo.DeviceLocalRequestId = wo.DeviceLocalRequestIdk__BackingField;
            testWo.DeviceUIRef = wo.DeviceUIRefk__BackingField;
            testWo.ReceiveTime = wo.ReceiveTimek__BackingField;
            testWo.SlaveWorkerId = wo.SlaveWorkerIdk__BackingField;
            testWo.SlaveWorkerSubmit = wo.SlaveWorkerSubmitk__BackingField;
            testWo.SlaveWorkOrderLastCommunication = wo.SlaveWorkOrderLastCommunicationk__BackingField;
            testWo.WorkOrderId = wo.WorkOrderIdk__BackingField;
            testWo.WorkOrderResultJson = wo.WorkOrderResultJsonk__BackingField;
            testWo.WorkOrderStatus = wo.WorkOrderStatusk__BackingField;
           
            // Send to listener (intent attached to application)
            Intent newIntent = new Intent(wo.ApplicationUIResultIntentk__BackingField);

            newIntent.PutExtra("WorkOrderTrimmed", JsonConvert.SerializeObject(testWo));

            this.ApplicationContext.SendBroadcast(newIntent);

        }


        public void RequestWorkOrderComputation(object o) {

            Intent i = (Intent)o;
            String a = i.GetStringExtra("CommPackage");
            ComputeAndroidSDK.Communication.CommPackage cp = ComputeAndroidSDK.Communication.CommPackage.DeserializeJson(a);


            try {
                new WorkOrderWS.WorkOrderSvc().CreateWorkOrder(App.GetAuthToken(this), App.GetDeviceId(this), true, cp.ApplicationId, true, cp.SerializeParamList(), "ProcessRequest", cp.DeviceLocalRequestId, true);
            } catch (Exception e) {
                Log.Error("RequestWorkOrderComputation", e.Message);
            }
            
            StartCommFetch();
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
            App.UpdateLastTransmit();
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

            System.Threading.Thread oThread = new System.Threading.Thread(new ThreadStart(ContinuallyNotifyActive));
            oThread.Start();

        }

        public void ContinuallyNotifyActive() {
            // This will run until the app is killed
            while (true) {
                if (App.GetLastTransmit() < DateTime.Now.AddMinutes(-4)) {
                    if (App.GetAuthToken(this) != null && App.GetDeviceId(this) != -1) {
                        new UserWS.UserSvc().MarkDeviceActive(App.GetAuthToken(this), App.GetDeviceId(this), true);
                        App.UpdateLastTransmit();
                    }
                }
                Thread.Sleep(new TimeSpan(0, 4, 0)); // Sleep for 4 minutes
            }
        }

        public override void OnStart(Intent intent, int startId) {
            base.OnStart(intent, startId);
         
        }

        public override void OnDestroy() {
            base.OnDestroy();

           
        }



    }

 
}