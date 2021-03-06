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

namespace ComputeAndroidSDK.Communication {
    public class WorkOrder {
        public int WorkOrderId {
            get;
            set;
        }
        public int DeviceId {
            get;
            set;
        }
        public int ApplicationId {
            get;
            set;
        }
        public string CommPackageJson {
            get;
            set;
        }
        public System.DateTime ReceiveTime {
            get;
            set;
        }
        public Nullable<int> SlaveWorkerId {
            get;
            set;
        }
        public Nullable<System.DateTime> SlaveWorkerSubmit {
            get;
            set;
        }
        public Nullable<System.DateTime> SlaveWorkOrderLastCommunication {
            get;
            set;
        }
        public string WorkOrderStatus {
            get;
            set;
        }
        public string WorkOrderResultJson {
            get;
            set;
        }
        public Nullable<System.DateTime> StartComputationTime {
            get;
            set;
        }
        public Nullable<System.DateTime> EndComputationTime {
            get;
            set;
        }

        public String DeviceUIRef {
            get;
            set;
        }

    }
}
