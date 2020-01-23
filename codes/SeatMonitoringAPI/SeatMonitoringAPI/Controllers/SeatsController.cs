using SeatMonitoringAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;

namespace SeatMonitoringAPI.Controllers
{
    /// <summary>
    /// 初期化されたConfigurationのSeatDefinitionsの要素に対し、各座席に人がいるかを判定するクラス
    /// </summary>
    public class SeatsController : ApiController
    {
        private readonly ISeatsScanner seatsScanner;

        /// <summary>
        /// SeatsScannerプロパティを初期化するコンストラクタ
        /// ConfigurationのSeatDefinitionsの数だけCameraクラスのインスタンスを作成してListにし、
        /// SeatsScannerクラスのインスタンスを作成してプロパティにセットする
        /// </summary>
        public SeatsController()
        {
            var cameras = new List<ICamera>();
            foreach (var seatDefinition in Models.Configuration.Instance.seatDefinitions)
            {
                cameras.Add(new Camera(seatDefinition.moniker));
            }
            seatsScanner = new SeatsScanner(cameras, new HumanDetector());
        }

        /// <summary>
        /// テスト用コンストラクタ
        /// </summary>
        /// <param name="seatsScanner"></param>
        public SeatsController(ISeatsScanner seatsScanner)
        {
            this.seatsScanner = seatsScanner;
        }

        /// <summary>
        /// ConfigurationのSeatDefinitionsに登録されている全座席の状態(人がいるか)を取得し、
        /// 監視座席名と座席状態の配列をJson形式にシリアライズしてstringを返すメソッド
        /// </summary>
        /// <returns>SeatMonitoringApiのレスポンスメッセージ</returns>
        public HttpResponseMessage GetSeats()
        {
            var result = seatsScanner.ScanAll()
                .Select(seat => new SeatsResult(seat.seatDefinition.name, seat.status.ToString()))
                .ToList();


            return new HttpResponseMessage()
            {
                // JSON形式の文字列を固定で返す
                Content = new StringContent(
                    JsonConvert.SerializeObject(result),
                    Encoding.UTF8,
                    "application/json"
                )
            };
        }
    }
}
