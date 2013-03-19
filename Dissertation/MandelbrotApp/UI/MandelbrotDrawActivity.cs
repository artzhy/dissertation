using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace com.ComputeApps.MandelbrotApp {
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MandelbrotDraw : Activity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            ImageView iv = FindViewById<ImageView>(Resource.Id.mandelbrotImgView);


            iv.SetMaxWidth(400);
            iv.SetMaxHeight(400);

            button.Click += button_Click;
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState) {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        protected override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);


        }

        void button_Click(object sender, EventArgs e) {

            EditText maxIterations = FindViewById<EditText>(Resource.Id.maxIterations);

            try {
                int maxIterationsNo = int.Parse(maxIterations.Text);

                if (maxIterationsNo > 0 && maxIterationsNo <= 100000) {
                    if (FindViewById<RadioButton>(Resource.Id.cloudComputation).Checked) {
                        new AsyncGetResultsTask(this, this, FindViewById<ImageView>(Resource.Id.mandelbrotImgView), 300, 300, maxIterationsNo).Execute();
                    } else {
                        DateTime startTime = DateTime.Now;
                        ImageView _imgView = FindViewById<ImageView>(Resource.Id.mandelbrotImgView);
                        double xmin = -2;
                        double xmax = 1.0;
                        double ymin = -1.5;
                        double ymax = 1.5;

                        _imgView.SetImageBitmap(new MandelbrotCalculator(300, 300, maxIterationsNo, xmax, xmin, ymax, ymin).GenMandelbrotBitmapTest());

                        new AlertDialog.Builder(this)
                  .SetTitle("Mandelbrot generated")
                  .SetMessage("Success! Time taken(s) - Cloud: " + DateTime.Now.Subtract(startTime).TotalSeconds)
                  .Show();
                    }



                } else {
                    new AlertDialog.Builder(this).SetMessage("Max iterations must greater than 0 and less than or equal to 100,000").SetTitle("Error").Show();
                }
            } catch (Exception) {
                new AlertDialog.Builder(this).SetMessage("Max iterations must be an integer").SetTitle("Error").Show();
            }



        }

        internal void setBitmap(Bitmap bm) {
            ImageView _imgView = FindViewById<ImageView>(Resource.Id.mandelbrotImgView);
            _imgView.SetImageBitmap(bm);
        }



    }


}

