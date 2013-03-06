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
using ComputeAndroidSDK.Communication;

namespace com.ComputeApps.MandelbrotApp {
    public class AsyncGetResultsTask : AsyncTask {
        private ProgressDialog _progressDialog;
        private Context _context;
        private Boolean _complete;
        private int _noComplete;
        private int _totalWOs;
        private ImageView _imgView;
        private int _width, _height, _maxIterations;
        
        public AsyncGetResultsTask(Context context, ImageView imgV, int width, int height, int maxIterations) {
            _context = context;
            _imgView = imgV;
            _width = width;
            _height = height;
            _maxIterations = maxIterations;
        }

        protected override void OnProgressUpdate(params Java.Lang.Object[] values) {
            base.OnProgressUpdate(values);

            new AlertDialog.Builder(_context)
               .SetTitle("Still working...")
               .SetMessage("No complete: " + _noComplete + " out of " + _totalWOs)
               .Show();
        }

        protected override void OnPreExecute() {
            base.OnPreExecute();
           
         
            _progressDialog = ProgressDialog.Show(_context, "Mandelbrot generation in progress", "Please wait...");
            
        }

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {

            MandelbrotCalculator mc = new MandelbrotCalculator(_width, _height, _maxIterations);
            WorkOrderList.NewRequest(this);
            List<CommPackage> cpList = JsonConvert.DeserializeObject<List<CommPackage>>(mc.SubmitWorkOrders());

            foreach (CommPackage cp in cpList) {
                WorkOrderList.SubmitNewWorkOrder(cp);
            }

          //  WorkOrderList.SubmitNewWorkOrder(JsonConvert.DeserializeObject<ComputeAndroidSDK.Communication.CommPackage>((string)@params[0]));

            while (!_complete) {
                // do nothing
            }

            return true;
        }

        protected override void OnPostExecute(Java.Lang.Object result) {
            base.OnPostExecute(result);

            _progressDialog.Hide();

            new AlertDialog.Builder(_context)
               .SetTitle("Mandelbrot generated")
               .SetMessage("Success!")
               .Show();

            _imgView.SetImageBitmap(WorkOrderList.TransformWorkOrderResultsToBitmap(_width, _height));

           
        }

        public void PostUpdate(ComputeAndroidSDK.Communication.WorkOrderTrimmed wt, Boolean TaskIsComplete, int uncomplete, int totalWOs) {
            Java.Lang.Object[] lst = new Java.Lang.Object[1];
            lst[0] = JsonConvert.SerializeObject(wt);
            PublishProgress(lst);
            
            _noComplete = totalWOs - uncomplete;
            _totalWOs = totalWOs;
           
            _complete = TaskIsComplete;
        }

    }
}
