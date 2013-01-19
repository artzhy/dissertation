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
        private static CommPackage currentWorkItem;
        private static Context applicationContext;

        private WorkList() {

        }

        private static void InitWorkList() {
             workList = new List<ComputeAndroidSDK.Communication.CommPackage>();
             currentWorkItem = null;
        }

        public static List<ComputeAndroidSDK.Communication.CommPackage> getWorkList() {
            if (workList == null) {
               InitWorkList();
            }

            return workList;
        }

        public static CommPackage GetNextWorkItem() {
            if (currentWorkItem == null && workList.Count != 0) 
                return workList.First();
             else
                return null;
        }

        public static void SetAppContext(Context appContext) {
            applicationContext = appContext;
        }

        public static void SubmitResult(CommPackage resultPackage) {
            currentWorkItem = null;
            workList.Remove(resultPackage);

            Intent intent = new Intent();
            intent.PutExtra("CommPackage", resultPackage.SerializeJson());
            intent.SetAction(Constants.RETURN_RESULT_INTENT);
            applicationContext.SendBroadcast(intent);

            // Continue doing work.
            CommPackage nextWorkItem = GetNextWorkItem();
           
            if (nextWorkItem != null)
                DoWork(nextWorkItem);
        }

        public static void DoWork(CommPackage workItem) {
            workItem = WorkerClass.DoWork(workItem);
            SubmitResult(workItem);

        }  

        public static void AddWorkItem(CommPackage toAdd) {
            if (workList == null) {
                InitWorkList();
            }

            workList.Add(toAdd);

            // Continue doing work.
            CommPackage nextWorkItem = GetNextWorkItem();

            if (nextWorkItem != null)
                DoWork(nextWorkItem);
        }

        public static void RemoveWorkItem(CommPackage toRemove) {
            workList.Remove(toRemove);
        }

    }
}