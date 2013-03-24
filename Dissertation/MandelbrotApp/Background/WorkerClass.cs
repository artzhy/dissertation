using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ComputeAndroidSDK.Communication;

/**
 * This class carries out the computation given parameters.  The method that the parameters are passed to should be set against the app in the database in the BackgroundProcessFunction field.
 * 
 * 
 **/

namespace com.ComputeApps.MandelbrotApp {
    class WorkerClass {

        public WorkerClass() {

        }

        public static int[][] ProcessRequest(int xStart, int yStart, double xMax, double xMin, double yMax, double yMin, int height, int width, int xChunkSize, int yChunkSize, int maxIterations) {
            int[][] pixelColours = new int[xChunkSize][];
            
            for (int x = xStart; x < (xStart + xChunkSize); x++) {
                pixelColours[x - xStart] = new int[yChunkSize];
                for (int y = yStart; y < (yStart + yChunkSize); y++) {
                    pixelColours[x-xStart][y-yStart] = MandelbrotCalculator.GetColourOfPixel(x, y, xMax, xMin, yMax, yMin, height, width, maxIterations);           
                }
            }

            return pixelColours;
        }


        /**
         * This method should be the same across all implementations
         **/
        public static CommPackage DoWork(CommPackage workItem) {
            Type type = typeof(WorkerClass);
            MethodInfo method = type.GetMethod(workItem.BackgroundProcessFunction);
            WorkerClass c = new WorkerClass();

            // Build up parameter array
            object[] arr = new object[method.GetParameters().Count()];

            foreach (ParameterInfo pParameter in method.GetParameters()) {
                IEnumerable<CommPackage.ParamListItem> x = (from y in workItem.ParameterList
                                                            where y.ParameterName == pParameter.Name
                                                            select y);



                if (x.Count() == 1) {
                    Type tTest = x.First().ParameterValue.GetType();
                    if (Type.GetTypeCode(tTest) == TypeCode.Int64) {
                        arr[pParameter.Position] = Convert.ToInt32(x.First().ParameterValue);
                    } else {
                        arr[pParameter.Position] = x.First().ParameterValue;
                    }
                } else {
                    // Throw exception - param not given
                    throw new Exception("Parameter " + pParameter.Name + " not given.");
                }
            }

           // DateTime
            workItem.ComputationStartTime = DateTime.Now;
            int[][] result = (int[][])method.Invoke(c, arr);
            workItem.ComputationEndTime = DateTime.Now;


            DateTime serialisationStart = DateTime.Now;

            workItem.ComputationResult = new ResultPackage(result).SerializeJson();

            workItem.SerialisationTime = (Decimal)DateTime.Now.Subtract(serialisationStart).TotalSeconds;
            
            return workItem;
        }

    }
}