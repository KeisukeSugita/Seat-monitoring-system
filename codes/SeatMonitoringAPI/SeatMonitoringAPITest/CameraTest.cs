using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class CameraTest
    {
        // 実際のMonikerを渡したとき、Bitmap型に変換された画像が返されるかのテスト
        [TestMethod]
        public void Shoot_CameraIsFound_ReturnPhoto()
        {
            var camera = new Camera("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002");
            var photo = camera.Shoot();

            Assert.IsFalse(photo == null);
        }

        // 存在しないMonikerを渡したとき、例外が発生するかのテスト
        [TestMethod]
        public void Shoot_CameraIsNotFound_ExceptionThrow()
        {
            var camera = new Camera("FraudulentMoniker");
            try
            {
                camera.Shoot();
            }
            catch(InvalidOperationException e)
            {
                Assert.AreEqual(e.Message, "該当するカメラが存在しません。");
                return;
            }

            Assert.Fail();
        }

    }
}
