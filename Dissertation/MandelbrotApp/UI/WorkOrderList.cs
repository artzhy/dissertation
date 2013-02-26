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

using ComputeAndroidSDK.Communication;

namespace com.ComputeApps.MandelbrotApp {
    public class WorkOrderList {
        private static List<CommPackage> CommunicationPackages = new List<CommPackage>();
        private static List<WorkOrderTrimmed> WorkOrders = new List<WorkOrderTrimmed>();
        private static Context ApplicationContext;

        public static void SetApplicationContext(Context AppContext) {
            ApplicationContext = AppContext;
        }

        public static void SubmitNewWorkOrder(CommPackage cp) {
            //TODO: This needs to be changed, leave as is just now - low priority!
            cp.DeviceLocalRequestId = new Random().Next(1, 9999999)+(int)DateTime.Now.Ticks;
            CommunicationPackages.Add(cp);

            // Send to controller.

            Intent submitIntent = new Intent(ComputeAndroidSDK.Communication.Constants.REQUEST_WORK_ORDER_INTENT);
            submitIntent.PutExtra("CommPackage", cp.SerializeJson());
            App.Context.SendBroadcast(submitIntent);
           
        }

        public static void SubmitWorkOrderResult(WorkOrderTrimmed wo) {
            try {   
                CommPackage cp = (from x in CommunicationPackages
                                  where wo.DeviceLocalRequestId == x.DeviceLocalRequestId
                                  select x).Single();

                CommunicationPackages.Remove(cp);
                WorkOrders.Add(wo);
                
                //TODO: Send result to the UI result handler..

            } catch {
                
            }
        }

        public static List<WorkOrderTrimmed> GetCompletedWorkOrders() {
            return WorkOrders;
        }

        public static void CancelOutstandingWorkOrders() {
            //TODO: Cancel work outstanding work order algorithms
        }
        

    }
}