using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 指定されたメソッド(Destination)に<see cref="SeatMonitoringApiClient.GetSeats"/>の戻り値を通知するインターフェース
    /// </summary>
    public interface IPeriodicNotifier
    {
        /// <summary>
        ///  Startメソッドの中で定期的に実行されるメソッド
        ///  座席の状態を通知させたいメソッドを登録できる
        /// </summary>
        Action<List<Seat>, bool> Destination { get; set; }

        /// <summary>
        /// 座席状態の取得と結果の通知を定期的に非同期に行うメソッド
        /// Stopメソッドを呼び出すことで非同期処理を終了させる
        /// </summary>
        void Start();

        /// <summary>
        /// Startメソッドで非同期に実行されている処理を終了させるメソッド
        /// </summary>
        void Stop();
    }
}
