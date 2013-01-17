
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

namespace ComputeAndroidApp.Common {
    [Serializable]
    class CommPackage {
        public int ComputationRequestId;
        public String IntentAction;
        public String BackgroundProcessFunction;
        public List<ParamListItem> ParameterList;
        public Object ComputationResult = null;
        
        public class ParamListItem {
            public String ParameterName;
            public Object ParameterValue;

            public ParamListItem(String _paramName, Object _paramValue) {
                this.ParameterName = _paramName;
                this.ParameterValue = _paramValue;
            }

        }
    }


}