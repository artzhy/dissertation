using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;


namespace AndroidApplication1 {
    [Activity(Label = "AndroidApplication1", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity {
        int count = 1;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate {
                button.Text = string.Format("{0} clicks!", count++);
            };

          //ImageView iv = FindViewById(Resource.Id.

    //        int[100][100]

         Bitmap bm = Bitmap.CreateBitmap(100,100,Bitmap.Config.Argb8888);
      //   bm.SetPixel(1, 1, Color.Black);

         for (int x = 0; x < 100; x++) {
             for (int y = 0; y < 100; y++) {
                 bm.SetPixel(x, y, Color.White);
     
             }
         }

         ImageView iv = FindViewById<ImageView>(Resource.Id.iv1);

         iv.SetImageBitmap(bm);
         iv.RefreshDrawableState();


        }
    }
}

