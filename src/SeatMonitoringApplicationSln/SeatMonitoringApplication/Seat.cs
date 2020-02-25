using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// 座席の名前と状態を保持するクラス
    /// </summary>
    public class Seat
    {
        public enum SeatStatus
        {
            Exists,
            NotExists,
            Failure,
        }

        /// <summary>
        /// <see cref="SeatStatus"/>の値に対応する文字列を定義したDictionary
        /// </summary>
        private readonly Dictionary<SeatStatus, string> SeatStatusLabel = new Dictionary<SeatStatus, string>
        {
            { SeatStatus.Exists, "在席" },
            { SeatStatus.NotExists, "離席" },
            { SeatStatus.Failure, "状態取得失敗" },
        };

        /// <summary>
        /// <see cref="SeatStatus"/>から<see cref="SeatStatusLabel"/>に定義されている文字列を取得するメソッド
        /// </summary>
        /// <param name="seatStatus"></param>
        /// <returns></returns>
        public string GetLabel(SeatStatus seatStatus)
        {
            return SeatStatusLabel[seatStatus];
        }


        [DataMember(Name = "Name")]
        public readonly string name;
        [DataMember(Name = "Status")]
        public readonly SeatStatus status;


        public Seat(string name, SeatStatus status)
        {
            this.name = name;
            this.status = status;
        }

        public Seat(string name, string status)
        {
            this.name = name;
            this.status = FromString(status);
        }

        /// <summary>
        /// 該当する文字列をenum SeatStatusに変換するクラス
        /// status：enum SeatStatusに該当する文字列
        /// </summary>
        /// <param name="status"></param>
        /// <returns>変換されたenum SeatStatusの値</returns>
        public SeatStatus FromString(string status)
        {
            if (status == Seat.SeatStatus.Exists.ToString())
            {
                return Seat.SeatStatus.Exists;
            }
            else if (status == Seat.SeatStatus.NotExists.ToString())
            {
                return Seat.SeatStatus.NotExists;
            }
            else if (status == Seat.SeatStatus.Failure.ToString())
            {
                return Seat.SeatStatus.Failure;
            }
            else
            {
                throw new InvalidOperationException("指定された文字列はSetStatusに存在しません。");
            }
        }
    }
}
