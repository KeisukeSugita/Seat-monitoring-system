using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class HumanDetectorCppTest
    {
        [DllImport("HumanDetector.dll", EntryPoint = "detect", CallingConvention = CallingConvention.Cdecl)]
        private extern static bool Detect(int rows, int cols, IntPtr image);

        // 人が写っている写真を渡したとき、戻り値がtrueかのテスト
        [TestMethod]
        public void Detect_HumanInThePhoto_ReturnTrue()
        {
            var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\Exists画像.jpg");
            
            var bmpData = photo.LockBits(new Rectangle(0, 0, photo.Width, photo.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                Assert.IsTrue(Detect(photo.Height, photo.Width, bmpData.Scan0));
            }
            finally
            {
                photo.UnlockBits(bmpData);
            }
        }

        // 人が写っていない写真を渡したとき、戻り値がFalseかのテスト
        [TestMethod]
        public void Detect_HumanNotInThePhoto_ReturnFalse()
        {
            var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\NotExists画像.jpg");

            var bmpData = photo.LockBits(new Rectangle(0, 0, photo.Width, photo.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                Assert.IsFalse(Detect(photo.Height, photo.Width, bmpData.Scan0));
            }
            finally
            {
                photo.UnlockBits(bmpData);
            }
        }
    }
}
