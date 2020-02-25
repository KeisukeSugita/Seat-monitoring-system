using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 指定されたメソッド(Destination)に結果を通知するインターフェース
    /// </summary>
    public interface IPeriodicNotifier
    {
        Action<List<Seat>, bool> Destination { get; set; }

        /// <summary>
        /// 座席状態の取得と結果の通知を定期的に非同期に行うメソッド
        /// </summary>
        void Start();

        /// <summary>
        /// Startメソッドを終了させるメソッド
        /// </summary>
        void Stop();
    }
}
