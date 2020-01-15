using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class SeatsScannerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cameras = new List<ICamera>();
            var cameraMock1 = new Mock<ICamera>("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002");
            var cameraMock2 = new Mock<ICamera>("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003");
            cameras.Add(cameraMock1.Object);
            cameras.Add(cameraMock2.Object);

            var humanDetectorMock = new Mock<IHumanDetector>();

            var seatsScanner = new SeatsScanner(cameras, humanDetectorMock.Object);


        }
    }
}