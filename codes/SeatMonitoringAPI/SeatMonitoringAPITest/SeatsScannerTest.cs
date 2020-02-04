using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class SeatsScannerTest
    {
        // ConfigurationのSeatDefinitionsプロパティの各要素と、対応するHumanDetector.Detect()の戻り値を正確にList<Seat>変換できているかテスト
        [TestMethod]
        public void ScanAll_ReturnSeatList()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""Name"":""スギタ ケイスケ""}]")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                Configuration.Initialize(streamReader);
            }

            var cameras = new List<ICamera>();
            var existsPhoto = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\Exists画像.jpg");
            var notExistsPhoto = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\NotExists画像.jpg");

            var cameraMock1 = new Mock<ICamera>();
            cameraMock1.Setup(x => x.Shoot()).Returns(existsPhoto);

            var cameraMock2 = new Mock<ICamera>();
            cameraMock2.Setup(x => x.Shoot()).Returns(notExistsPhoto);

            var cameraMock3 = new Mock<ICamera>();
            cameraMock3.Setup(x => x.Shoot()).Throws<InvalidOperationException>();

            cameras.Add(cameraMock1.Object);
            cameras.Add(cameraMock2.Object);
            cameras.Add(cameraMock3.Object);

            var humanDetectorMock = new Mock<IHumanDetector>();
            humanDetectorMock.Setup(x => x.Detect(existsPhoto)).Returns(true);
            humanDetectorMock.Setup(x => x.Detect(notExistsPhoto)).Returns(false);

            var seatsScanner = new SeatsScanner(cameras, humanDetectorMock.Object);
            var seats = seatsScanner.ScanAll();

            Assert.AreEqual("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002", seats[0].seatDefinition.deviceInstansePath);
            Assert.AreEqual("杉田 圭輔", seats[0].seatDefinition.name);
            Assert.AreEqual("Exists", seats[0].status.ToString());
            Assert.AreEqual("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003", seats[1].seatDefinition.deviceInstansePath);
            Assert.AreEqual("Keisuke Sugita", seats[1].seatDefinition.name);
            Assert.AreEqual("NotExists", seats[1].status.ToString());
            Assert.AreEqual("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004", seats[2].seatDefinition.deviceInstansePath);
            Assert.AreEqual("スギタ ケイスケ", seats[2].seatDefinition.name);
            Assert.AreEqual("Failure", seats[2].status.ToString());

            var privateObject = new PrivateType(typeof(Configuration));
            privateObject.SetStaticField("instance", null);
        }
    }
}