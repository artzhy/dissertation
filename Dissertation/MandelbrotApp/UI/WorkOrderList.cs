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

        //For perf analysis only
        public static double deserialiseSecs;
        public static double setPixelSecs;

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
            deserialiseSecs = 0;
            setPixelSecs = 0;
        }

        private class ComparerTest : IEqualityComparer<CommunicationResources.PixelColour> {

            public bool Equals(CommunicationResources.PixelColour x, CommunicationResources.PixelColour y) {
                return x.x == y.x && x.y == y.y;
            }

            public int GetHashCode(CommunicationResources.PixelColour obj) {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() ^ obj.colour.GetHashCode();
            }
        }

        public static Bitmap TransformWorkOrderResultsToBitmap() {

            //List<CommunicationResources.PixelColour> pc = new List<CommunicationResources.PixelColour>();
            //foreach (WorkOrderTrimmed wo in WorkOrders) {
             
            //        pc.AddRange(JsonConvert.DeserializeObject<ResultPackage>(wo.WorkOrderResultJson).PixelColours);
            //}

            //List<CommunicationResources.PixelColour> dist = pc.Distinct(new ComparerTest()).ToList();

            //// Get duplicates



                foreach (WorkOrderTrimmed wo in WorkOrders) {
                    Log.Info("WorkOrderList", "Filling pixels: " + wo.WorkOrderId);
                    DateTime StartRender = DateTime.Now;
                    int[][] arr = JsonConvert.DeserializeObject<ResultPackage>(wo.WorkOrderResultJson).PixelColours;
                    CommPackage cp = JsonConvert.DeserializeObject<CommPackage>(wo.CommPackageJson);
                    object test = cp.ParameterList.Where(a => a.ParameterName == "xStart").First().ParameterValue;
                    Int64 xMin = (Int64)cp.ParameterList.Where(a => a.ParameterName == "xStart").First().ParameterValue;
                    Int64 yMin = (Int64)cp.ParameterList.Where(a => a.ParameterName == "yStart").First().ParameterValue;

                    for (int x = 0; x < arr.Length; x++) {
                        int[] yArr = arr[x];
                        for (int y = 0; y < yArr.Length; y++) {
                            int adjX = x + (int)xMin;
                            int adjY = y + (int)yMin;

                            System.Drawing.Color c = System.Drawing.Color.FromArgb(yArr[y]);
                            DateTime startSetPixel = DateTime.Now;
                            image.SetPixel(adjX, adjY, new Color(c.R, c.G, c.B));
                            setPixelSecs += DateTime.Now.Subtract(startSetPixel).TotalSeconds;

                        }
                    }
                    
                    deserialiseSecs += DateTime.Now.Subtract(StartRender).TotalSeconds;
                    
                    Log.Info("WorkOrderList", "Completed filling pixels" + wo.WorkOrderId);
                }
            return image;
        }
      

    }
}