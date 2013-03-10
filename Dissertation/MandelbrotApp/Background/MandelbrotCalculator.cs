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
        private const int MinXChunk = 60;
        private const int MinYChunk = 60;
        private int imgWidth, imgHeight, maxIterations, xChunk, yChunk;
        private double xMax, xMin, yMax, yMin;

        public MandelbrotCalculator(int _imgWidth, int _imgHeight, int _maxIterations) {
            imgHeight = _imgHeight;
            imgWidth = _imgWidth;
            maxIterations = _maxIterations;

            // Calculate chunk

            if ((imgWidth / MinXChunk) < 1) {
                xChunk = MinXChunk;
            } else {
                xChunk = MinXChunk;
                while ((imgWidth % xChunk) != 0) {
                    xChunk++;
                }
            }

            if ((imgHeight / MinYChunk) < 1) {
                yChunk = MinYChunk;
            } else {
                yChunk = MinYChunk;
                while ((imgHeight % yChunk) != 0) {
                    yChunk++;
                }
            }
        }

        //public static Bitmap GenMandelbrotBitmapTest(int width, int height, int maxIterations) {
        //    Bitmap bm = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
        //    int x = 0;
        //    while (x < width) {
        //        int y = 0;
        //        while (y < height) {
        //            double xmin = -2;
        //            double xmax = 1.0;
        //            double ymin = -1.5;
        //            double ymax = 1.5;
        //            int colour = GetColourOfPixel(x, y, xmax, xmin, width, ymax, ymin, height, maxIterations);
                    
        //            int r = Color.GetRedComponent(colour);
        //            int g = Color.GetGreenComponent(colour);
        //            int b = Color.GetBlueComponent(colour);
        //            int a = Color.GetAlphaComponent(colour);

        //            bm.SetPixel(x, y, Color.Rgb(r, g, b));
        //            y++;
        //        }
        //        x++;
        //    }

        //    return bm;
        //}

        public static int GetColourOfPixel(int x, int y,double cx, double cy, int maxIterations) {
            double zr = cx, zi = cy;
            double tmp;

            int i;

            for (i = 0; i < maxIterations; i++) {
                tmp = zr * zr - zi * zi + cx;
                zi *= 2 * zr;
                zi += cy;
                zr = tmp;

                if (zr * zr + zi * zi > 4.0)
                    break;
            }

            return Background.ColourScheme.ColourPoint(i, maxIterations);
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

                    double cx = xMin + x * ((xMax - xMin) / imgWidth);
                    double cy = yMin + y * ((yMax - yMin) / imgHeight);

                    parameters.Add(new CommPackage.ParamListItem("xStart", x));
                    parameters.Add(new CommPackage.ParamListItem("yStart", y));
                    parameters.Add(new CommPackage.ParamListItem("xChunkSize", xChunk));
                    parameters.Add(new CommPackage.ParamListItem("yChunkSize", yChunk));
                    parameters.Add(new CommPackage.ParamListItem("cx", cx));
                    parameters.Add(new CommPackage.ParamListItem("cy", cy));
                    parameters.Add(new CommPackage.ParamListItem("maxIterations", maxIterations));
                    cp.ParameterList = parameters;
                    cp.ApplicationId = 5;
                    cpList.Add(cp);
               
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