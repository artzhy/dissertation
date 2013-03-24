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

/**
 * This class is defined wholly by the user.  It is what the WorkerClass uses as its means of packaging up the result of a computation, and it is what the UI elements expects returned as result. 
It must be serialisable.  
**/

namespace com.ComputeApps.MandelbrotApp {
    [Serializable]
    class ResultPackage {
        // Add your own properties here!
        public int[][] PixelColours {
            get;
            set;
        }

        public ResultPackage(int[][] _PixelColours) {
            this.PixelColours = _PixelColours;
        }

        public String SerializeJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static ResultPackage DeserializeJson(String json) {
            return JsonConvert.DeserializeObject<ResultPackage>(json);
        }

    }
}