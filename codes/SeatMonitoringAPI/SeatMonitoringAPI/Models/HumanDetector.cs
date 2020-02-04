using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 画像に人が写っているかを判定するクラス
    /// </summary>
    public class HumanDetector : IHumanDetector
    {
        private string cascadeFile = ConfigurationManager.AppSettings["CascadeFilePath"];

        /// <summary>
        /// 実際に画像の判定を行っているメソッド
        /// rows：画像データの行数
        /// cols：画像データの列数
        /// image：画像データの先頭ポインタ
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="image"></param>
        /// <returns>判定結果</returns>
        [DllImport("HumanDetector.dll", EntryPoint = "detect", CallingConvention = CallingConvention.StdCall)]
        private extern static bool Detect(int rows, int cols, IntPtr image, string cascadeFile);

        /// <summary>
        /// Bitmap型からWidth、Height、画像データ部分の先頭ポインタを取り出してDllのDetectに渡し、その結果を返すメソッド
        /// photo：Bitmap型の画像データ
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>判定結果</returns>
        public bool Detect(Bitmap photo)
        {
            // メモリにデータをロックし、ロックした部分をBitmapData型で扱う
            var bmpData = photo.LockBits(new Rectangle(0, 0, photo.Width, photo.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                return Detect(photo.Height, photo.Width, bmpData.Scan0, cascadeFile);
            }
            catch(Exception e)
            {
                throw new InvalidOperationException("画像の判定に失敗しました。", e);
            }
            finally
            {
                // ロックしたメモリのアンロック
                photo.UnlockBits(bmpData);
            }
        }
    }
}