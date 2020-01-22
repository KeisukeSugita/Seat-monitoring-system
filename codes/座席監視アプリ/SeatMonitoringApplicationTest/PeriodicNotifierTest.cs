using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringApplication;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class PeriodicNotifierTest
    {
        /// <summary>
        /// Start_GetSeatsIsSuccess_IsSucceededIsTrueのテスト用通知先メソッド
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="isSucceeded"></param>
        public void TestUpdate_IsSucceeded(List<Seat> seats, bool isSucceeded)
        {
            Assert.IsTrue(isSucceeded);
            Assert.AreEqual("杉田 圭輔", seats[0].Name);
            Assert.AreEqual(Seat.SeatStatus.Exists, seats[0].Status);
            Assert.AreEqual("Keisuke Sugita", seats[1].Name);
            Assert.AreEqual(Seat.SeatStatus.Failure, seats[1].Status);
        }

        /// <summary>
        /// Start_GetSeatsIsNotSuccess_IsSucceededIsFalseのテスト用通知先メソッド
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="isSucceeded"></param>
        public void TestUpdate_IsNotSucceeded(List<Seat> seats, bool isSucceeded)
        {
            Assert.IsFalse(isSucceeded);
            Assert.IsNull(seats);
        }

        /// <summary>
        /// SeatMonitoringApiからのレスポンスをList<Seat>で受け取ることができた場合、
        /// seatsを受け取ったリスト、isSucceededをtrueとして正しく通知できているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Start_GetSeatsIsSuccess_IsSucceededIsTrue()
        {
            var seats = new List<Seat>();
            seats.Add(new Seat("杉田 圭輔", Seat.SeatStatus.Exists));
            seats.Add(new Seat("Keisuke Sugita", Seat.SeatStatus.Failure));

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();
            seatMonitoringApiClientMock.Setup(x => x.GetSeats()).Returns(seats);

            PeriodicNotifier.Destination destination = TestUpdate_IsSucceeded;

            var periodicNotifier = new PeriodicNotifier(destination, seatMonitoringApiClientMock.Object);
            periodicNotifier.Start();
        }


        /// <summary>
        /// SeatMonitoringApiClientで例外が発生した場合、
        /// seatsはnull、isSucceededをfalseとして正しく通知できているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Start_GetSeatsIsNotSuccess_IsSucceededIsFalse()
        {
            var seats = new List<Seat>();

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();
            seatMonitoringApiClientMock.Setup(x => x.GetSeats()).Throws(new SeatsApiException());

            PeriodicNotifier.Destination destination = TestUpdate_IsNotSucceeded;

            var periodicNotifier = new PeriodicNotifier(destination, seatMonitoringApiClientMock.Object);
            periodicNotifier.Start();
        }


        /// <summary>
        /// Stopメソッドが呼び出されたとき、PeriodicNotifierのプロパティIsStopRequestedがtrueとなっているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Stop_IsStopRequestedIsFalse()
        {
            var seats = new List<Seat>();

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();

            PeriodicNotifier.Destination destination = TestUpdate_IsSucceeded;

            var periodicNotifier = new PeriodicNotifier(destination, seatMonitoringApiClientMock.Object);
            periodicNotifier.Stop();

            Assert.AreEqual(true, periodicNotifier.IsStopRequested);
        }
    }
}
