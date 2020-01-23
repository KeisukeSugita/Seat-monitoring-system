using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Threading;
using System.Linq;

namespace SeatMonitoringAPI.Models
{
    /// <summary>
    /// 指定したMonikerに対応するカメラのクラス
    /// コンストラクタでMonikerを指定する
    /// </summary>
    public class Camera : ICamera
    {
        private Bitmap Photo { get; set; }  // 取得した画像
        public readonly string Moniker;
        private readonly AutoResetEvent shotEvent;
        public Camera(string moniker)
        {
            Moniker = $@"@device:pnp:\\?\{moniker}#{{65e8773d-8f56-11d0-a3b9-00a0c9223196}}\global";
            shotEvent = new AutoResetEvent(false);
        }


        /// <summary>
        /// monikerに対応するカメラから画像を1枚取得し、Bitmap型で返すメソッド
        /// string moniker:カメラのデバイスインスタンスパス
        /// </summary>
        /// <returns>取得した画像データ</returns>
        public Bitmap Shoot()
        {
            Photo = null;

            if(!WebApiApplication.filterInfoCollection.Cast<FilterInfo>().Any(filterInfo => (filterInfo.MonikerString == Moniker)))
            {
                throw new InvalidOperationException("該当するカメラが存在しません。");
            }


            var videoCaptureDevice = new VideoCaptureDevice(Moniker);

            videoCaptureDevice.NewFrame += new NewFrameEventHandler(PickFrame); // カメラが画像を取得したときに発生するイベント

            videoCaptureDevice.Start();

            try
            {
                if (!shotEvent.WaitOne(1000))
                {
                    throw new InvalidOperationException("画像が取得できませんでした。該当するカメラが存在しないか、接続が切断された可能性があります。");
                }
            }
            finally
            {
                videoCaptureDevice.SignalToStop();
                videoCaptureDevice.WaitForStop();
            }

            return Photo;
        }

        /// <summary>
        /// eventArgs内のFrameプロパティから画像を読み込み、フィールドに格納するクラス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void PickFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // 既に画像を取得していた場合は何も処理を行わない
            if (Photo == null)
            {
                Photo = new Bitmap(eventArgs.Frame);    // 取得した画像をPhotoに格納
                shotEvent.Set();
            }
        }
    }
}