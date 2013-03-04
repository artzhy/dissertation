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

namespace com.ComputeApps.MandelbrotApp {
    public class MandelbrotCalculator {
        private const int MinXChunk = 20;
        private const int MinYChunk = 20;
        private int imgWidth, imgHeight, maxIterations, xChunk, yChunk;

        public MandelbrotCalculator(int _imgWidth, int _imgHeight, int _maxIterations) {
            imgHeight = _imgHeight;
            imgWidth = _imgWidth;
            maxIterations = _maxIterations;

            // Calculate chunk

            if ((imgWidth / MinXChunk) < 1) {
                xChunk = MinXChunk;
            } else {
                xChunk = imgWidth/4;
                while ((imgWidth % xChunk) != 0) {
                    xChunk--;
                }
            }

            if ((imgHeight / MinYChunk) < 1) {
                yChunk = MinYChunk;
            } else {
                yChunk = imgHeight / 4;
                while ((imgHeight % yChunk) != 0) {
                    yChunk++;
                }
            }
        }

        public static int GetColourOfPixel(int x, int y, int maxIterations) {
            Boolean inside = true;

            //Run iterations over this point
            int iteration = 0;

            int y0 = y;
            int x0 = x;

            for (iteration = 0; iteration < maxIterations; iteration++) {
                // z^2 + c
                int newx = x ^ 2 - (y * y) + x;
                int newy = (2 * x * y) + y;

                x = newx;
                y = newy;

                // Distance is > 2, escapes to infinity
                if ((x ^ 2 + y ^ 2) > 4) {
                    inside = false;
                    break;
                }
            }

            int colourCodeHex;
            if (inside)
                colourCodeHex = Background.ColourScheme.ColourInsidePoint();
            else
                colourCodeHex = Background.ColourScheme.ColourOutsidePoint(iteration, maxIterations);

            return colourCodeHex;
        }

        public String SubmitWorkOrders() {
            int x = 0;
            int y = 0;

            List<CommPackage> cpList = new List<CommPackage>();

            Bitmap bm = Bitmap.CreateBitmap(imgWidth, imgHeight, Bitmap.Config.Argb8888);

            while (x < imgWidth) {
                y = 0;

                while (y < imgHeight) {

                    ComputeAndroidSDK.Communication.CommPackage cp = new ComputeAndroidSDK.Communication.CommPackage();
                    List<CommPackage.ParamListItem> parameters = new List<CommPackage.ParamListItem>();


                    parameters.Add(new CommPackage.ParamListItem("xStart", x));
                    parameters.Add(new CommPackage.ParamListItem("yStart", y));
                    parameters.Add(new CommPackage.ParamListItem("xChunkSize", xChunk));
                    parameters.Add(new CommPackage.ParamListItem("yChunkSize", yChunk));
                    parameters.Add(new CommPackage.ParamListItem("maxIterations", maxIterations));
                    cp.ParameterList = parameters;
                    cp.ApplicationId = 5;
                    cpList.Add(cp);
                    //return cp.SerializeJson();

               //     WorkOrderList.SubmitNewWorkOrder(cp);

                    //Intent woRequestIntent = new Intent(ComputeAndroidSDK.Communication.Constants.REQUEST_WORK_ORDER_INTENT);
                    //woRequestIntent.PutExtra("CommPackage", cp.SerializeJson());

                    //App.Context.SendBroadcast(woRequestIntent);

                 //   return bm;
              
                    //List<CommunicationResources.PixelColour> colours = ProcessRequest(x, y, xChunk, yChunk, maxIterations);

                    //foreach (CommunicationResources.PixelColour col in colours) {
                    //    int r = Color.GetRedComponent(col.colour);
                    //    int g = Color.GetGreenComponent(col.colour);
                    //    int b = Color.GetBlueComponent(col.colour);
                    //    int a = Color.GetAlphaComponent(col.colour);

                    //    bm.SetPixel(col.x, col.y, Color.Rgb(r, g, b));
                    //}

                    y += yChunk;

                }
                x += xChunk;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(cpList);



            //while (x < imgWidth) {
            //    int y = 0;
            //    while (y < imgHeight) {
            //        //ComputeAndroidSDK.Communication.CommPackage cp = new ComputeAndroidSDK.Communication.CommPackage();
            //        //List<CommPackage.ParamListItem> parameters = new List<CommPackage.ParamListItem>();

            //        //parameters.Add(new CommPackage.ParamListItem("xStart", x));
            //        //parameters.Add(new CommPackage.ParamListItem("yStart", y));
            //        //parameters.Add(new CommPackage.ParamListItem("xChunkSize", xChunk));
            //        //parameters.Add(new CommPackage.ParamListItem("yChunkSize", yChunk));
            //        //parameters.Add(new CommPackage.ParamListItem("maxIterations", maxIterations));
            //        //cp.ParameterList = parameters;



            //        //for(int xb = 0; xb < xChunk; xb++) {
            //        //    for (int yb = 0; yb < yChunk; yb++) {
            //        //        int colour = GetColourOfPixel(xb + x, yb + y, maxIterations);
            //        //        int r = Color.GetRedComponent(colour);
            //        //        int g = Color.GetGreenComponent(colour);
            //        //        int b = Color.GetBlueComponent(colour);
            //        //        int a = Color.GetAlphaComponent(colour);


            //        //        bm.SetPixel(x + xb, y + yb, Color.Rgb(r,g,b));
            //        //    }
            //        //}





            //        y += yChunk;
            //    }
            //    x += xChunk;
            //}

         //   return bm;


            //int x = 0;

            //while (x < imgWidth) {
            //    int y = 0;
            //    while (y < imgHeight) {
            //        // Work Order Here

            //        y += yChunk;
            //    }
            //    x += xChunk;
            //}


        }



    }
}