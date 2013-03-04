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

namespace com.ComputeApps.MandelbrotApp {
    public class WorkOrderList {
        private static List<CommPackage> CommunicationPackages = new List<CommPackage>();
        private static List<WorkOrderTrimmed> WorkOrders = new List<WorkOrderTrimmed>();
        private static Context ApplicationContext;
        private static AsyncGetResultsTask AsyncResultTask;

        public static void SetApplicationContext(Context AppContext) {
            ApplicationContext = AppContext;
        }

        public static void NewRequest(AsyncGetResultsTask agr) {
            AsyncResultTask = agr;
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
                //CommPackage cp = (from x in CommunicationPackages
                //                  where wo.DeviceLocalRequestId == x.DeviceLocalRequestId
                //                  select x).Single();

                //CommunicationPackages.Remove(cp);
                WorkOrders.Add(wo);


                List<int> completedWOs = WorkOrders.Select(x => x.DeviceLocalRequestId).ToList();
                List<int> allWOs = CommunicationPackages.Select(x => x.DeviceLocalRequestId).ToList();

                List<int> umcompletedItems = completedWOs.Except(allWOs).Concat(allWOs.Except(completedWOs)).ToList();


                //TODO: Send result to the UI result handler..
                AsyncResultTask.PostUpdate(wo, umcompletedItems.Count() == 0, umcompletedItems.Count(), allWOs.Count());


            } catch {
                
            }
        }

        public static List<WorkOrderTrimmed> GetCompletedWorkOrders() {
            return WorkOrders;
        }

        public static void CancelOutstandingWorkOrders() {
            //TODO: Cancel work outstanding work order algorithms
        }

        public static Bitmap TransformWorkOrderResultsToBitmap(int imgHeight, int imgWidth) {
            Bitmap bm = Bitmap.CreateBitmap(imgWidth, imgHeight, Bitmap.Config.Argb8888);
            foreach (WorkOrderTrimmed wot in WorkOrders) {
                List<CommunicationResources.PixelColour> pixels = JsonConvert.DeserializeObject<ResultPackage>(wot.WorkOrderResultJson).PixelColours;


                foreach (CommunicationResources.PixelColour col in pixels) {
                    int r = Color.GetRedComponent(col.colour);
                    int g = Color.GetGreenComponent(col.colour);
                    int b = Color.GetBlueComponent(col.colour);
                    int a = Color.GetAlphaComponent(col.colour);

                    bm.SetPixel(col.x, col.y, Color.Rgb(r, g, b));
                }
            }
            return bm;
        }
        

    }
}