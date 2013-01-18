
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace ComputeAndroidSDK.Communication {
    [Serializable]
    public class CommPackage {
        public int ComputationRequestId { get; set; }
        public String IntentAction { get; set; }
        public String BackgroundProcessFunction { get; set; }
        public List<ParamListItem> ParameterList { get; set; }
        public Object ComputationResult { get; set; }

        public String SerializeJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static CommPackage DeserializeJson(String json) {
            
            
            return JsonConvert.DeserializeObject<CommPackage>(json);
        }

        [Serializable]
        public class ParamListItem {
            public String ParameterName { get; set; }
            public Object ParameterValue { get; set; }

            public ParamListItem(String _paramName, Object _paramValue) {
                this.ParameterName = _paramName;
                this.ParameterValue = _paramValue;
            }

        }
    }




}