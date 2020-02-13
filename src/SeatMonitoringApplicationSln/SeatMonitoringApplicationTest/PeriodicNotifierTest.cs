using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SeatMonitoringApplication;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class PeriodicNotifierTest
    {
        private List<(List<Seat>, bool, DateTime)> GetResult(ISeatMonitoringApiClient seatMonitoringApiClient, int count, int interval)
        {
            var results = new List<(List<Seat>, bool, DateTime)>();
            PeriodicNotifier.Destination destination = (List<Seat> seats, bool isSucceeded) => {
                results.Add((seats, isSucceeded, DateTime.Now));
            };

            var periodicNotifier = new PeriodicNotifier(destination, seatMonitoringApiClient, interval);
            periodicNotifier.Start();
            while (results.Count < count)
            {
                Thread.Sleep(1000);
            }
            periodicNotifier.Stop();

            return results;
        }

        /// <summary>
        /// SeatMonitoringApiからのレスポンスをList<Seat>で受け取ることができた場合、
        /// seatsを受け取ったリスト、isSucceededをtrueとして正しく通知できているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Start_GetSeatsIsSuccess_IsSucceededIsTrue()
        {

            var expectedSeats = new List<Seat>();
            expectedSeats.Add(new Seat("杉田 圭輔", Seat.SeatStatus.Exists));
            expectedSeats.Add(new Seat("Keisuke Sugita", Seat.SeatStatus.Failure));

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();
            seatMonitoringApiClientMock.Setup(x => x.GetSeats()).Returns(expectedSeats);

            var results = GetResult(seatMonitoringApiClientMock.Object, 3, 5 * 1000);

            foreach (var (resultSeats, resultIsSucceeded, resultTime) in results)
            {
                Assert.AreEqual(expectedSeats, resultSeats);
                Assert.IsTrue(resultIsSucceeded);
            }
            var span = results[1].Item3 - results[0].Item3;
            Assert.IsTrue(5 * 1000 * 0.9 < span.TotalMilliseconds && span.TotalMilliseconds < 5 * 1000 * 1.1);
            span = results[2].Item3 - results[1].Item3;
            Assert.IsTrue(5 * 1000 * 0.9 < span.TotalMilliseconds && span.TotalMilliseconds < 5 * 1000 * 1.1);
        }


        /// <summary>
        /// SeatMonitoringApiClientで例外が発生した場合、
        /// seatsはnull、isSucceededをfalseとして正しく通知できているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Start_GetSeatsIsNotSuccess_IsSucceededIsFalse()
        {
            var expectedSeats = new List<Seat>();

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();
            seatMonitoringApiClientMock.Setup(x => x.GetSeats()).Throws(new SeatsApiException());

            var results = GetResult(seatMonitoringApiClientMock.Object, 3, 5 * 1000);

            foreach (var (resultSeats, resultIsSucceeded, resultTime) in results)
            {
                Assert.IsNull(resultSeats);
                Assert.IsFalse(resultIsSucceeded);
            }
            var span = results[1].Item3 - results[0].Item3;
            Assert.IsTrue(5 * 1000 * 0.9 < span.TotalMilliseconds && span.TotalMilliseconds < 5 * 1000 * 1.1);
            span = results[2].Item3 - results[1].Item3;
            Assert.IsTrue(5 * 1000 * 0.9 < span.TotalMilliseconds && span.TotalMilliseconds < 5 * 1000 * 1.1);
        }


        /// <summary>
        /// Stopメソッドが呼び出されたとき、PeriodicNotifierのプロパティIsStopRequestedがtrueとなっているかを確認するテスト
        /// </summary>
        [TestMethod]
        public void Stop_IsStopRequestedIsFalse()
        {
            var seats = new List<Seat>();

            var seatMonitoringApiClientMock = new Mock<ISeatMonitoringApiClient>();

            var results = GetResult(seatMonitoringApiClientMock.Object, 3, 5 * 1000);

            var expectedCount = results.Count;

            Thread.Sleep(10 * 1000);

            Assert.AreEqual(expectedCount, results.Count);
        }
    }
}
