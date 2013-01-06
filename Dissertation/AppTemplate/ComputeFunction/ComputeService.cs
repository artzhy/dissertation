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
    public class ComputeService : Service {
        const int MAX_QUEUE_SIZE = 10;
        private List<ComputeItem> workList;
        private ComputeServiceBinder mBinder;
     
        public override IBinder OnBind(Intent intent) {
            mBinder = new ComputeServiceBinder(this);
            return mBinder;
        }
        
        public void ComputeResult(int workOrderId, Array parameters) {
            if (workList.Count() <= MAX_QUEUE_SIZE) {

            } else {
                throw new Exception("Max items already queued/in progress.");   
            }
        }
     
        public void test() {
            ComputeItem test = new ComputeItem();
            test.workOrderId = 1;
            test.test();
        }
    }
    
    public class ComputeItem {
        enum TaskStatus {
            IN_PROGRESS,
            QUEUED,
            COMPLETED
        }

        [Serializable()]    
        private class ComputeItemResult {
            public int workOrderId;
            public Object result;
        }

        public int workOrderId;

        public void test() {
            ComputeItemResult res = new ComputeItemResult();
            res.workOrderId = 1;
            res.result = "test";

            var test = Newtonsoft.Json.JsonConvert.SerializeObject(res);



    
        }


    }


}