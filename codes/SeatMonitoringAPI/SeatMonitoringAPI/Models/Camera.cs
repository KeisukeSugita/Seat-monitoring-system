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
    public class Camera
    {
        static private Bitmap Photo { get; set; }
        static private bool isPicked = false;
        public Bitmap Shoot(string moniker)
        {
            string deviceMoniker = $@"@device:pnp:\\?\{moniker}#{{65e8773d-8f56-11d0-a3b9-00a0c9223196}}\global";
            foreach (FilterInfo fic in WebApiApplication.filterInfoCollection)
            {
                if (fic.MonikerString == deviceMoniker)
                {
                    var videoCaptureDevice = new VideoCaptureDevice(deviceMoniker);

                    videoCaptureDevice.NewFrame += new NewFrameEventHandler(pickFrame);

                    videoCaptureDevice.Start();
                    while (!isPicked)
                    {
                        Thread.Sleep(10);
                    }
                    videoCaptureDevice.Stop();

                    return Photo;
                }
            }
            throw new InvalidOperationException("該当するカメラが存在しません。");
        }

        static private void pickFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isPicked)
            {
                return;
            }
            Photo = eventArgs.Frame;
        }
    }
}