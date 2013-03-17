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
using Android.Graphics;
using Newtonsoft.Json;

/**
 * The Work Order List contains a list of communication packages (work order computation requests) and also completed work orders.  From here new work order computations can be requested - it also handles when a result is received by updating the AsyncGetResultsTask.
 * */

namespace com.ComputeApps.MandelbrotApp {
    public class WorkOrderList {
        private static List<CommPackage> CommunicationPackages = new List<CommPackage>();
        private static List<WorkOrderTrimmed> WorkOrders = new List<WorkOrderTrimmed>();
        private static Context ApplicationContext;
        private static AsyncGetResultsTask AsyncResultTask;
        private static Bitmap image;

        public static void SetApplicationContext(Context AppContext) {
            ApplicationContext = AppContext;
        }

        public static void NewRequest(AsyncGetResultsTask agr, int imgWidth, int imgHeight) {
            AsyncResultTask = agr;
          image = Bitmap.CreateBitmap(imgWidth, imgHeight, Bitmap.Config.Argb8888);
        }

        public static void SubmitNewWorkOrder(CommPackage cp) {
            //TODO: This needs to be changed, leave as is just now - low priority!
            cp.DeviceLocalRequestId = new Random().Next(1, 9999999);
            CommunicationPackages.Add(cp);

            // Send to controller.

            Intent submitIntent = new Intent(ComputeAndroidSDK.Communication.Constants.REQUEST_WORK_ORDER_INTENT);
            submitIntent.PutExtra("CommPackage", cp.SerializeJson());
            App.Context.SendBroadcast(submitIntent);
           
        }

        public static void SubmitWorkOrderResult(WorkOrderTrimmed wo) {
            try {   
                WorkOrders.Add(wo);


                List<CommunicationResources.PixelColour> pixels = JsonConvert.DeserializeObject<ResultPackage>(wo.WorkOrderResultJson).PixelColours;

                foreach (CommunicationResources.PixelColour col in pixels) {
                    int r = Color.GetRedComponent(col.colour);
                    int g = Color.GetGreenComponent(col.colour);
                    int b = Color.GetBlueComponent(col.colour);
                    int a = Color.GetAlphaComponent(col.colour);

                    try {
                        image.SetPixel(col.x, col.y, Color.Rgb(r, g, b));
                    } catch {

                    }
                }

                List<int> completedWOs = WorkOrders.Select(x => x.DeviceLocalRequestId).ToList();
                List<int> allWOs = CommunicationPackages.Select(x => x.DeviceLocalRequestId).ToList();

                List<int> umcompletedItems = completedWOs.Except(allWOs).Concat(allWOs.Except(completedWOs)).ToList();

                AsyncResultTask.PostUpdate(wo, umcompletedItems.Count() == 0, umcompletedItems.Count(), allWOs.Count());

            } catch {
                
            }
        }

        public static List<WorkOrderTrimmed> GetCompletedWorkOrders() {
            return WorkOrders;
        }

        public static void CancelOutstandingWorkOrders() {
            //TODO: Cancel work outstanding work order algorithms
            Intent submitIntent = new Intent(ComputeAndroidSDK.Communication.Constants.CANCEL_WORK_ORDER_INTENT);
            submitIntent.PutExtra("localIdList", JsonConvert.SerializeObject(CommunicationPackages.Select(x => x.DeviceLocalRequestId)));
            App.Context.SendBroadcast(submitIntent);
            ClearState();
        }


        public static void ClearState() {
            CommunicationPackages = new List<CommPackage>();
            WorkOrders = new List<WorkOrderTrimmed>();
            image = null;
            ApplicationContext = null;
            AsyncResultTask= null;
        }

        public static Bitmap TransformWorkOrderResultsToBitmap() {
           return image;
        }
        

    }
}