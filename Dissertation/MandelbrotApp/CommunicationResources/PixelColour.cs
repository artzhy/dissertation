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

namespace com.ComputeApps.MandelbrotApp.CommunicationResources {
    [Serializable]
    public class PixelColour {
        public int x, y, colour;

        public PixelColour(int _x, int _y, int _colour) {
            x = _x;
            y = _y;
          colour = _colour;
        
        }

    }
}