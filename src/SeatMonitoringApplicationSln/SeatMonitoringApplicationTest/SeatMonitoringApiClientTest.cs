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
            var Host = "test";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent(@"[{""name"":""杉田 圭輔"",""status"":""Exists""},{""name"":""Keisuke Sugita"",""status"":""Failure""}]");
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{Host}/api/seats")).Returns(httpResponseMessage);

            var seatMonitoringApiClient = new SeatMonitoringApiClient(Host, myHttpClientMock.Object);
            var seatsResult = seatMonitoringApiClient.GetSeats();

            Assert.AreEqual("杉田 圭輔", seatsResult[0].name);
            Assert.AreEqual(Seat.SeatStatus.Exists, seatsResult[0].status);
            Assert.AreEqual("Keisuke Sugita", seatsResult[1].name);
            Assert.AreEqual(Seat.SeatStatus.Failure, seatsResult[1].status);
        }


        /// <summary>
        /// サーバへの接続に失敗した場合、接続がタイムアウトした場合、SeatsApiExceptionをスローするかのテスト
        /// </summary>
        [TestMethod]
        public void GetSeats_CatchAggregateException_ThrowSeatsApiException()
        {
            var aggregateException = new AggregateException();
            var Host = "test";
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{Host}/api/seats")).Throws(aggregateException);

            var seatMonitoringApiClient = new SeatMonitoringApiClient(Host, myHttpClientMock.Object);

            try
            {
                var seatsResult = seatMonitoringApiClient.GetSeats();
            }
            catch (SeatsApiException e)
            {
                Assert.AreEqual("サーバへの接続に失敗しました。", e.Message);
                Assert.AreEqual(aggregateException, e.InnerException);
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
            var Host = "test";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var myHttpClientMock = new Mock<IMyHttpClient>();
            myHttpClientMock.Setup(x => x.GetAsync($@"http://{Host}/api/seats")).Returns(httpResponseMessage);

            var seatMonitoringApiClient = new SeatMonitoringApiClient(Host, myHttpClientMock.Object);

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
