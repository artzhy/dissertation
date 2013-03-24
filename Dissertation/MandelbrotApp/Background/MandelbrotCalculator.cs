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
        private const int MinXChunk = 70;
        private const int MinYChunk = 70;
        private int imgWidth, imgHeight, maxIterations, xChunk, yChunk;
        private double xMax, xMin, yMax, yMin;

        public MandelbrotCalculator(int _imgWidth, int _imgHeight, int _maxIterations, double _xMax, double _xMin, double _yMax, double _yMin) {
            imgHeight = _imgHeight;
            imgWidth = _imgWidth;
            maxIterations = _maxIterations;
            xMax = _xMax;
            xMin = _xMin;
            yMax = _yMax;
            yMin = _yMin;

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

        public List<CommunicationResources.PixelColour> GenPixelColourList() {
         //   Bitmap bm = Bitmap.CreateBitmap(imgWidth, imgHeight, Bitmap.Config.Argb8888);
            double xmin = -2;
            double xmax = 1.0;
            double ymin = -1.5;
            double ymax = 1.5;

            int x = 0;

            List<CommunicationResources.PixelColour> pixelColours = new List<CommunicationResources.PixelColour>();

            while (x < imgWidth) {
                int y = 0;
                while (y < imgHeight) {
                    int colour = GetColourOfPixel(x, y, xmax, xmin, ymax, ymin, imgWidth, imgHeight, maxIterations);
                    pixelColours.Add(new CommunicationResources.PixelColour(x, y, colour));

                    y++;
                }
                x++;
            }

            return pixelColours;
        }

        public Bitmap GetBitmap(List<CommunicationResources.PixelColour> pixelColours) {
            Bitmap bm = Bitmap.CreateBitmap(imgWidth, imgHeight, Bitmap.Config.Argb8888);
            foreach (CommunicationResources.PixelColour colour in pixelColours) {
                int r = Color.GetRedComponent(colour.colour);
                int g = Color.GetGreenComponent(colour.colour);
                int b = Color.GetBlueComponent(colour.colour);
               
                 bm.SetPixel(colour.x, colour.y, Color.Rgb(r, g, b));

            }

            return bm;
        }

        public static int GetColourOfPixel(int x, int y, double xMax, double xMin, double yMax, double yMin, int height, int width, int maxIterations) {

            double cx = xMin + x * ((xMax - xMin) / width);
            double cy = yMin + y * ((yMax - yMin) / height);

            double zr = cx, zi = cy;
            double tmp;

            int i;

            for (i = 0; i < maxIterations; i++) {
                tmp = zr * zr - zi * zi + cx;
                zi = zi * 2 * zr;
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

                    parameters.Add(new CommPackage.ParamListItem("xStart", x));
                    parameters.Add(new CommPackage.ParamListItem("yStart", y));
                    parameters.Add(new CommPackage.ParamListItem("xChunkSize", xChunk));
                    parameters.Add(new CommPackage.ParamListItem("yChunkSize", yChunk));
                    parameters.Add(new CommPackage.ParamListItem("xMax", xMax));
                    parameters.Add(new CommPackage.ParamListItem("xMin", xMin));
                    parameters.Add(new CommPackage.ParamListItem("yMin", yMin));
                    parameters.Add(new CommPackage.ParamListItem("yMax", yMax));
                    parameters.Add(new CommPackage.ParamListItem("width", imgWidth));
                    parameters.Add(new CommPackage.ParamListItem("height", imgHeight));
                    parameters.Add(new CommPackage.ParamListItem("maxIterations", maxIterations));
                    cp.ParameterList = parameters;
                    cp.ApplicationId = 5;
                    cpList.Add(cp);
               
                    y += yChunk;

                }
                x += xChunk;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(cpList);

        }



    }
}