using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SeatMonitoringAPI.Models
{
    /*
     * 画像に人が写っているかを判定するクラス
     */
    public class HumanDetector : IHumanDetector
    {
        /*
         * 実際に画像の判定を行っているメソッド
         * rows：画像データの行数
         * cols：画像データの列数
         * image：画像データの先頭ポインタ
         */
        [DllImport("HumanDetector.dll", EntryPoint = "detect", CallingConvention = CallingConvention.Cdecl)]
        private extern static bool Detect(int rows, int cols, IntPtr image);

        /*
         * Bitmap型からWidth、Height、画像データ部分の先頭ポインタを取り出してDllのDetectに渡し、その結果を返すメソッド
         * photo：Bitmap型の画像データ
         */
        public bool Detect(Bitmap photo)
        {
            // メモリにデータをロックし、ロックした部分をBitmapData型で扱う
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
                // ロックしたメモリのアンロック
                photo.UnlockBits(bmpData);
            }
        }
    }
}