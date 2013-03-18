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

namespace AppTemplate.ComputeFunction {
    [Service]
    [IntentFilter(new String[] { "com.AppTemplate.ComputeFunction.ComputeService" })]
    public class ComputeService : Service, IComputeService {
        const int MAX_QUEUE_SIZE = 10;
        private List<ComputeItem> workList;
        private ComputeServiceBinder mBinder;
     
        public override IBinder OnBind(Intent intent) {
            mBinder = new ComputeServiceBinder(this);
            return mBinder;
        }
        
        public Object ComputeResult(int workOrderId, Dictionary<String, Object> parameters) {
       //     if (workList.Count() <= MAX_QUEUE_SIZE) {
                ComputeItem ci = new ComputeItem();

                ci.parameters = parameters;
                ci.workOrderId = workOrderId;

         //       this.workList.Add(ci);
                
                Object res = ci.GetResult();

         //       this.workList.Remove(ci);

                return res;
       //     } else {
          //      throw new Exception("Max items already queued/in progress.");   
           // }
        }

        /* Compute functionality here */

        

    }
    
    public class ComputeItem {
        enum TaskStatus {
            IN_PROGRESS,
            QUEUED,
            COMPLETED
        }

        public int workOrderId;
        public Dictionary<String, Object> parameters;


        public Object GetResult() {
            Object res = 0;
            parameters.TryGetValue("IntVal", out res);
            return (int)res + 1;
        }

    }


}