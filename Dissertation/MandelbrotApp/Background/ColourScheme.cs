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

/**
 * General algorithm taken from uk.ac.ed.inf.mandelbrotmaps.colouring
 * TODO: acknowledge properly
 **/


namespace com.ComputeApps.MandelbrotApp.Background {
    public class ColourScheme {
        private const int COLOUR_SPACING = 30;

        public static int ColourPoint(int iteration, int maxIteration) {
            if (iteration == maxIteration)
                return ColourInsidePoint();

            if (iteration <= 0)
                unchecked {
                    return (int)0xFF000000;
                }

            int colourCodeR, colourCodeG, colourCodeB;
            double colourCode;

            // Percentage (0.0 -- 1.0)
            colourCode = (double)iteration / (double)maxIteration;

            // Red
            colourCodeR = Math.Min((int)(255 * 6 * colourCode), 255);

            // Green
            colourCodeG = (int)(255 * colourCode);

            // Blue
            colourCodeB = (int)(127.5 - 127.5 * Math.Cos(7 * Math.PI * colourCode));

            //Compute colour from the three components
            int colourCodeHex = (0xFF << 24) + (colourCodeR << 16) + (colourCodeG << 8) + (colourCodeB);

            return colourCodeHex;
        }

        public static int ColourInsidePoint() {
            unchecked {
                return (int)0xFFFFFFFF;
            }

        }

    }
}