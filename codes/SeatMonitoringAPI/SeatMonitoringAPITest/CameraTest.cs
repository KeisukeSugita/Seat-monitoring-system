using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class CameraTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var camera = new Camera("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002");
            var photo = camera.Shoot();

            Assert.IsFalse(photo == null);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var camera = new Camera("FraudulentMoniker");
            try
            {
                camera.Shoot();
            }
            catch(InvalidOperationException e)
            {
                Assert.AreEqual(e.Message, "該当するカメラが存在しません。");
            }
        }
    }
}
