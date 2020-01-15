using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public interface ICamera
    {
        Bitmap Shoot();
    }


    public class Camera : ICamera
    {
        private Bitmap Photo { get; set; }  // 取得した画像
        public string Moniker { get; private set; }
        private bool IsPicked = false;  // 画像を取得したかどうかを表すフラグ

        public Camera(string moniker)
        {
            Moniker = $@"@device:pnp:\\?\{moniker}#{{65e8773d-8f56-11d0-a3b9-00a0c9223196}}\global";
        }

        /*
         * monikerに対応するカメラから画像を1枚取得し、Bitmap型で返すメソッド
         * string moniker:カメラのデバイスインスタンスパス
         */
        public Bitmap Shoot()
        {
            foreach (FilterInfo filterInfo in WebApiApplication.filterInfoCollection)   // 接続されているカメラのdeviceMonikerに渡されたmonikerがあるか確認
            {
                if (filterInfo.MonikerString == Moniker)
                {
                    var videoCaptureDevice = new VideoCaptureDevice(Moniker);

                    videoCaptureDevice.NewFrame += new NewFrameEventHandler(PickFrame); // カメラが画像を取得したときに発生するイベント

                    videoCaptureDevice.Start();
                    int processingTime = 0;
                    while (!IsPicked)
                    {
                        Thread.Sleep(10);
                        processingTime++;
                        if (processingTime >= 100)
                        {
                            throw new InvalidOperationException("画像が取得できませんでした。該当するカメラが存在しないか、接続が切断された可能性があります。");
                        }
                    }
                    videoCaptureDevice.Stop();

                    IsPicked = false;   // 次にShootが呼ばれたときに画像を取得できるよう、falseにする

                    return Photo;
                }
            }
            throw new InvalidOperationException("該当するカメラが存在しません。");
        }

        private void PickFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // 既に画像を取得していた場合は何も処理を行わない
            if (!IsPicked)
            {
                Photo = new Bitmap(eventArgs.Frame);    // 取得した画像をPhotoに格納
                IsPicked = true;    // 画像を取得したとフラグを建てる
            }
        }
    }
}