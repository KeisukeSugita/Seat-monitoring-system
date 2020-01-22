using System;
using SeatMonitoringApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace SeatMonitoringApplicationTest
{
    [TestClass]
    public class SeatMonitoringApiClientTest
    {
        /// <summary>
        /// SeatMonitoringApiから正常なレスポンスを受けた場合、JSON形式の文字列をList<Seat>型に変換して返せているかのテスト
        /// </summary>
        [TestMethod]
        public void GetSeats_IsSucceeded_ReturnSeatList()
        {
            var ipAddress = "test";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent(@"[{""Name"":""杉田 圭輔"",""Status"":""Exists""},{""Name"":""Keisuke Sugita"",""Status"":""Failure""}]");
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{ipAddress}:44383/api/seats")).Returns(httpResponseMessage);

            var seatMonitoringApiClient = new SeatMonitoringApiClient(ipAddress, myHttpClientMock.Object);
            var seatsResult = seatMonitoringApiClient.GetSeats();

            Assert.AreEqual("杉田 圭輔", seatsResult[0].Name);
            Assert.AreEqual(Seat.SeatStatus.Exists, seatsResult[0].Status);
            Assert.AreEqual("Keisuke Sugita", seatsResult[1].Name);
            Assert.AreEqual(Seat.SeatStatus.Failure, seatsResult[1].Status);
        }

        /// <summary>
        /// 接続がタイムアウトした場合、SeatsApiExceptionをスローするかのテスト
        /// </summary>
        [TestMethod]
        public void GetSeats_CatchTaskCanceledException_ThrowSeatsApiException()
        {
            var ipAddress = "test";
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{ipAddress}:44383/api/seats")).Throws(new TaskCanceledException());

            var seatMonitoringApiClient = new SeatMonitoringApiClient(ipAddress, myHttpClientMock.Object);

            try
            {
                var seatsResult = seatMonitoringApiClient.GetSeats();
            }
            catch (SeatsApiException e)
            {
                Assert.AreEqual("接続がタイムアウトしました。", e.Message);
                return;
            }

            Assert.Fail();
        }

        /// <summary>
        /// サーバへの接続に失敗した場合、SeatsApiExceptionをスローするかのテスト
        /// </summary>
        [TestMethod]
        public void GetSeats_CatchAggregateException_ThrowSeatsApiException()
        {
            var ipAddress = "test";
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{ipAddress}:44383/api/seats")).Throws(new AggregateException());

            var seatMonitoringApiClient = new SeatMonitoringApiClient(ipAddress, myHttpClientMock.Object);

            try
            {
                var seatsResult = seatMonitoringApiClient.GetSeats();
            }
            catch (SeatsApiException e)
            {
                Assert.AreEqual("サーバへの接続に失敗しました。", e.Message);
                return;
            }

            Assert.Fail();
        }

        /// <summary>
        /// レスポンスのステータスコードが200ではなかった場合、SeatsApiExceptionをスローするかのテスト
        /// </summary>
        [TestMethod]
        public void GetSeats_IsFailed_ThrowSeatsApiException()
        {
            var ipAddress = "test";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{ipAddress}:44383/api/seats")).Returns(httpResponseMessage);

            var seatMonitoringApiClient = new SeatMonitoringApiClient(ipAddress, myHttpClientMock.Object);

            try
            {
                var seatsResult = seatMonitoringApiClient.GetSeats();
            }
            catch (SeatsApiException e)
            {
                Assert.AreEqual($@"ステータスコード""{(int)httpResponseMessage.StatusCode}""で失敗しました。", e.Message);
                return;
            }

            Assert.Fail();
        }
    }
}
