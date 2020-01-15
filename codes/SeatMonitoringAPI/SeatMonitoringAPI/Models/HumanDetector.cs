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
    public interface IHumanDetector
    {
        bool Detect(Bitmap photo);
    }


    public class HumanDetector : IHumanDetector
    {
        [DllImport("HumanDetector.dll", EntryPoint = "detect", CallingConvention = CallingConvention.Cdecl)]
        private extern static bool Detect(int rows, int cols, IntPtr image);
        public bool Detect(Bitmap photo)
        {
            var bmpData = photo.LockBits(new Rectangle(0, 0, photo.Width, photo.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                return Detect(photo.Height, photo.Width, bmpData.Scan0);
            }
            catch
            {
                throw new InvalidOperationException("画像の判定に失敗しました。");
            }
            finally
            {
                photo.UnlockBits(bmpData);
            }
        }
    }
}