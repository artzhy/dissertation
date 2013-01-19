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

namespace com.App1 {
    class WorkList {
        private static List<ComputeAndroidSDK.Communication.CommPackage> workList;

        private WorkList() {

        }

        public static void InitWorkList() {
             workList = new List<ComputeAndroidSDK.Communication.CommPackage>();
        }

        public static List<ComputeAndroidSDK.Communication.CommPackage> getWorkList() {
            if (workList == null) {
               InitWorkList();
            }

            return workList;
        }

        public static void AddWorkItem(CommPackage toAdd) {
            if (workList == null) {
                InitWorkList();
            }

            workList.Add(toAdd);
        }

        public static void RemoveWorkItem(CommPackage toRemove) {
            workList.Remove(toRemove);
        }

    }
}