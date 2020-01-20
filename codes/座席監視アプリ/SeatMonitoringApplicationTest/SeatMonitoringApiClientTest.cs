using System;
using SeatMonitoringApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class SeatMonitoringApiClientTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var seatMonitoringApiClient = new SeatMonitoringApiClient("localhost");
            var seatsResult = seatMonitoringApiClient.GetSeats();

            Assert.AreEqual("杉田 圭輔", seatsResult[0].Name);
            Assert.AreEqual("Exists", seatsResult[0].Status);
            Assert.AreEqual("杉田 圭輔", seatsResult[1].Name);
            Assert.AreEqual("Failure", seatsResult[1].Status);
        }
    }
}
