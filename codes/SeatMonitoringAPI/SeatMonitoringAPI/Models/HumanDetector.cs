using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class HumanDetector
    {
        [DllImport("HumanDetector.dll", CallingConvention = CallingConvention.StdCall)]
        extern static bool detect(int rows, int cols, IntPtr image);
        public bool Detect(Bitmap photo)
        {
            var bmpData = photo.LockBits(new Rectangle(0, 0, photo.Width, photo.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                return detect(photo.Height, photo.Width, bmpData.Scan0);
            }
            finally
            {
                photo.UnlockBits(bmpData);
            }
        }
    }
}