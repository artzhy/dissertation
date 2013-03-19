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
using Android.Util;

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
        private static int[] arr;
        private static int imgWidth, imgHeight;

        public static void SetApplicationContext(Context AppContext) {
            ApplicationContext = AppContext;
        }

        public static void NewRequest(AsyncGetResultsTask agr, int _imgWidth, int _imgHeight) {
            ClearState();
          AsyncResultTask = agr;
          image = Bitmap.CreateBitmap(_imgWidth, _imgHeight, Bitmap.Config.Argb8888);
          imgWidth = _imgWidth;
          imgHeight = _imgHeight;
          arr = new int[imgWidth * imgHeight];
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

                

                List<int> completedWOs = WorkOrders.Select(x => x.DeviceLocalRequestId).ToList();
                List<int> allWOs = CommunicationPackages.Select(x => x.DeviceLocalRequestId).ToList();

                List<int> umcompletedItems = completedWOs.Except(allWOs).Concat(allWOs.Except(completedWOs)).ToList();

                AsyncResultTask.PostUpdate(wo, umcompletedItems.Count() == 0, umcompletedItems.Count(), allWOs.Count());

            } catch (Exception e) {
                Log.Error("WorkOrderList", e.Message);
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
            IntPtr dest = image.LockPixels();
                foreach (WorkOrderTrimmed wo in WorkOrders) {
                    Log.Info("WorkOrderList", "Filling pixels: " + wo.WorkOrderId);
                    foreach (CommunicationResources.PixelColour col in JsonConvert.DeserializeObject<ResultPackage>(wo.WorkOrderResultJson).PixelColours) {

                        System.Drawing.Color c = System.Drawing.Color.FromArgb(col.colour);


                       

                        image.SetPixel(col.x, col.y, new Color(c.R, c.G, c.B));

                    
                        
                        

                       
                    }

                  

                    Log.Info("WorkOrderList", "Completed filling pixels" + wo.WorkOrderId);
                }
                image.UnlockPixels();
            return image;
        }
      

    }
}