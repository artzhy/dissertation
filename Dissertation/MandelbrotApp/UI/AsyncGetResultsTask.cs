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
using System.Threading;
using Android.Graphics;

/**
 * The AsyncGetResultsTask is an asyncronous task, and runs in a thread different to the UI.
 * It takes the parameters required to calculate all the work that needs complete, and then chunks the work up in some way, before sending these work packages to the Main App's internal receiver; from here it is then sent on to the Cloud to be computed.
 * 
 * While results are being computed, it shows a progress bar.
 * 
 * When a computation has been complete, the Incoming Receiver of the UI should call the PostUpdate function.  This function call then adds updates the progress bar.  When all computations have been received back from the cloud, it calls  OnPostExecute which should contain the code to update the UI if required.
 **/

namespace com.ComputeApps.MandelbrotApp {
    public class AsyncGetResultsTask : AsyncTask {
        private ProgressDialog _progressDialog;
        private Context _context;
        private Boolean _complete;
        private int _noComplete;
        private int _totalWOs;
        private ImageView _imgView;
        private int _width, _height, _maxIterations;
        private MandelbrotDraw _activity;
        private DateTime StartTime;
        private DateTime EndTime;
        
        public AsyncGetResultsTask(MandelbrotDraw act, Context context, ImageView imgV, int width, int height, int maxIterations) {
            _context = context;
            _imgView = imgV;
            _width = width;
            _height = height;
            _maxIterations = maxIterations;
            _activity = act;
        }

        protected override void OnProgressUpdate(params Java.Lang.Object[] values) {
            base.OnProgressUpdate(values);

            _progressDialog.Progress = _noComplete;
        }

        protected override void OnPreExecute() {
            base.OnPreExecute();

            _progressDialog = new ProgressDialog(_context);
            _progressDialog.SetTitle("Mandelbrot generation in progress");
            _progressDialog.SetMessage("Please wait...");
            _progressDialog.SetCancelable(true);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            _progressDialog.SetButton(-2, "Cancel", CancelClicked);
            _progressDialog.Show();
            
        }

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
            double xmin = -2;
            double xmax = 1.0;
            double ymin = -1.5;
            double ymax = 1.5;
            MandelbrotCalculator mc = new MandelbrotCalculator(_width, _height, _maxIterations,xmax,xmin,ymax,ymin);
            WorkOrderList.NewRequest(this, _width, _height);
            List<CommPackage> cpList = JsonConvert.DeserializeObject<List<CommPackage>>(mc.SubmitWorkOrders());

            _progressDialog.Max = cpList.Count();
            

            foreach (CommPackage cp in cpList) {
                WorkOrderList.SubmitNewWorkOrder(cp);
            }

            StartTime = DateTime.Now;

            while (!_complete) {
                // do nothing
                Thread.Sleep(new TimeSpan(0, 0, 10));
               
            }

         
            return true;
        }

        private void CancelClicked(object sender, DialogClickEventArgs e) {
            WorkOrderList.CancelOutstandingWorkOrders();
            this.Dispose();
        }

        protected override void OnPostExecute(Java.Lang.Object result) {
            base.OnPostExecute(result);

          _progressDialog.Hide();

          EndTime = DateTime.Now;

          DateTime imgStart = DateTime.Now;
            
          Bitmap img = WorkOrderList.TransformWorkOrderResultsToBitmap();

            new AlertDialog.Builder(_context)
               .SetTitle("Mandelbrot generated")
               .SetMessage("Success! Time taken(s) - Cloud: " + EndTime.Subtract(StartTime).TotalSeconds + ". Time taken (s) img: " + DateTime.Now.Subtract(imgStart).TotalSeconds)
               .Show();

            _activity.setBitmap(img);
          
        }

        public void PostUpdate(ComputeAndroidSDK.Communication.WorkOrderTrimmed wt, Boolean TaskIsComplete, int uncomplete, int totalWOs) {
            Java.Lang.Object[] lst = new Java.Lang.Object[1];
            lst[0] = JsonConvert.SerializeObject(wt);
            _noComplete = totalWOs - uncomplete;
            _totalWOs = totalWOs;
            PublishProgress(lst);
            
            _complete = TaskIsComplete;
        }
    } 
}
