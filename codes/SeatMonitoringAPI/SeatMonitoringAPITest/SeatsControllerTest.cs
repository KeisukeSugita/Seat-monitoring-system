using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringAPI.Controllers;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class SeatsControllerTest
    {
        // SeatsScanner.ScanAll()からの戻り値がキー「Name」,「Status」のJson形式に変換されているかのテスト
        [TestMethod]
        public void GetSeats_ReturnJsonString()
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004"",""Name"":""スギタ ケイスケ""}]")))
            using (var streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Configuration.Initialize(streamReader);
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail();
                }
            }

            var seats = new List<Seat>();
            seats.Add(new Seat(Configuration.Instance.SeatDefinitions[0], SeatStatus.Exists));
            seats.Add(new Seat(Configuration.Instance.SeatDefinitions[1], SeatStatus.NotExists));
            seats.Add(new Seat(Configuration.Instance.SeatDefinitions[2], SeatStatus.Failure));

            var seatsScannerMock = new Mock<ISeatsScanner>();
            seatsScannerMock.Setup(x => x.ScanAll()).Returns(seats);

            var seatsController = new SeatsController(seatsScannerMock.Object);
            var seatsResults = seatsController.GetSeats();

            Assert.AreEqual(@"[{""Name"":""杉田 圭輔"",""Status"":""Exists""},{""Name"":""Keisuke Sugita"",""Status"":""NotExists""},{""Name"":""スギタ ケイスケ"",""Status"":""Failure""}]", seatsResults);
        }
    }
}
