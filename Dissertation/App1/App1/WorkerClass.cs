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

namespace com.ComputeApps.TestApp {
    class WorkerClass {

        public WorkerClass() {

        }

        public int WorkerFunction(int a, int b) {
            return a * b;
        }

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
                    arr[pParameter.Position] = x.First().ParameterValue;
                } else {
                    // Throw exception - param not given
                    throw new Exception("Parameter " + pParameter.Name + " not given.");
                }
            }

            int result = (int)method.Invoke(c, arr);

            workItem.ComputationResult = result;
            return workItem;
        }

    }
}