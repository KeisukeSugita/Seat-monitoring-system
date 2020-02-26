using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 登録した通知先に<see cref="Start"/>内の処理から通知をするインターフェース
    /// </summary>
    public interface IPeriodicNotifier
    {
        /// <summary>
        ///  <see cref="Start"/>の中で定期的に実行される処理
        /// </summary>
        Action<List<Seat>, bool> Destination { get; set; }

        /// <summary>
        /// "座席状態の取得と結果の通知を定期的に行う処理"を非同期に開始するメソッド
        /// <see cref="Stop"/>を呼び出すことで非同期処理を終了させる
        /// </summary>
        void Start();

        /// <summary>
        /// <see cref="Start"/>で非同期に実行されている処理を終了させるメソッド
        /// </summary>
        void Stop();
    }
}
