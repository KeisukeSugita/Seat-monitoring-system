using SeatMonitoringAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace SeatMonitoringAPI.Controllers
{
    /// <summary>
    /// 初期化されたConfigurationのSeatDefinitionsの要素に対し、各座席に人がいるかを判定するクラス
    /// </summary>
    public class SeatsController : ApiController
    {
        private ISeatsScanner SeatsScanner { get; set; }

        /// <summary>
        /// SeatsScannerプロパティを初期化するコンストラクタ
        /// ConfigurationのSeatDefinitionsの数だけCameraクラスのインスタンスを作成してListにし、
        /// SeatsScannerクラスのインスタンスを作成してプロパティにセットする
        /// </summary>
        public SeatsController()
        {
            var cameras = new List<ICamera>();
            foreach (var seatDefinition in Models.Configuration.Instance.SeatDefinitions)
            {
                cameras.Add(new Camera(seatDefinition.Moniker));
            }
            SeatsScanner = new SeatsScanner(cameras, new HumanDetector());
        }

        /// <summary>
        /// テスト用コンストラクタ
        /// </summary>
        /// <param name="seatsScanner"></param>
        public SeatsController(ISeatsScanner seatsScanner)
        {
            SeatsScanner = seatsScanner;
        }

        /// <summary>
        /// ConfigurationのSeatDefinitionsに登録されている全座席の状態(人がいるか)を取得し、
        /// 監視座席名と座席状態の配列をJson形式にシリアライズしてstringを返すメソッド
        /// </summary>
        /// <returns>SeatMonitoringApiのレスポンスメッセージ</returns>
        public HttpResponseMessage GetSeats()
        {
            var result = SeatsScanner.ScanAll()
                .Select(seat => new SeatsResult(seat.SeatDefinition.Name, seat.Status.ToString()))
                .ToList();


            return new HttpResponseMessage()
            {
                // JSON形式の文字列を固定で返す
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(result),
                    Encoding.UTF8,
                    "application/json"
                )
            };
        }
    }
}
