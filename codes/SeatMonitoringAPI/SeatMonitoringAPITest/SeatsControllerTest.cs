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
            var seats = new List<Seat>();
            seats.Add(new Seat(new SeatDefinition("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002", "杉田 圭輔"), SeatState.Exists));
            seats.Add(new Seat(new SeatDefinition("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003", "Keisuke Sugita"), SeatState.NotExists));
            seats.Add(new Seat(new SeatDefinition("usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0004", "スギタ ケイスケ"), SeatState.Failure));

            var seatsScannerMock = new Mock<ISeatsScanner>();
            seatsScannerMock.Setup(x => x.ScanAll()).Returns(seats);

            var seatsController = new SeatsController(seatsScannerMock.Object);
            var seatsResults = seatsController.GetSeats().Content.ReadAsStringAsync().Result;

            Assert.AreEqual(@"[{""name"":""杉田 圭輔"",""status"":""Exists""},{""name"":""Keisuke Sugita"",""status"":""NotExists""},{""name"":""スギタ ケイスケ"",""status"":""Failure""}]", seatsResults);
        }
    }
}
