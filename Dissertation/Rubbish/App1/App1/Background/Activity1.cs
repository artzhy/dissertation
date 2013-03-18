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
using Android.Util;

using ComputeAndroidSDK.Communication;

namespace com.ComputeApps.TestApp {
    [Activity(Label = "My Activity", MainLauncher=true)]
    public class Activity1 : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here

            CommPackage cp = new CommPackage();

            cp.BackgroundProcessFunction = "WorkerFunction";
            cp.ParameterList = new List<CommPackage.ParamListItem>();

            cp.ParameterList.Add(new CommPackage.ParamListItem("a", 3));
            cp.ParameterList.Add(new CommPackage.ParamListItem("b", 9));

            string test = cp.SerializeJson();


            WorkList.SetAppContext(this.ApplicationContext);
   //         WorkList.AddWorkItem(cp);

        }
    }
}