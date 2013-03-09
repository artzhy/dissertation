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
        int count = 1;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            ImageView iv = FindViewById<ImageView>(Resource.Id.mandelbrotImgView);


            iv.SetMaxWidth(300);
            iv.SetMaxHeight(400);

            button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e) {
            //TODO: Validation

            //TODO: Split request up and into WOs

     
            new AsyncGetResultsTask(this, this,FindViewById<ImageView>(Resource.Id.mandelbrotImgView), 180, 300, 100).Execute();

    
           // iv.SetImageBitmap();


 

        }

        internal void setBitmap(Bitmap bm) {
            ImageView _imgView = FindViewById<ImageView>(Resource.Id.mandelbrotImgView);
            _imgView.SetImageBitmap(bm);
        //    _imgView.RefreshDrawableState();
        }

    }
}

