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

using Newtonsoft.Json;

namespace com.ComputeApps.MandelbrotApp {
    [Serializable]
    class ResultPackage {
        // Add your own properties here!
        public Object MultResult {
            get;
            set;
        }

        public ResultPackage(Object _multResult) {
            this.MultResult = _multResult;
        }

        public String SerializeJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static ResultPackage DeserializeJson(String json) {
            return JsonConvert.DeserializeObject<ResultPackage>(json);
        }

    }
}