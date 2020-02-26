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
        /// <see cref="SeatStatus"/>から対応する文字列を取得するメソッド
        /// ・Exists："在席"
        /// ・NotExists："離席"
        /// ・Failure："状態取得失敗"
        /// </summary>
        /// <param name="seatStatus">文字列を取得したい<see cref="SeatStatus"/></param>
        /// <returns>引数に対応する文字列</returns>
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
        /// 該当する文字列を<see cref="SeatStatus"/>に変換するクラス
        /// </summary>
        /// <param name="status">変換したい文字列</param>
        /// <returns>変換された<see cref="SeatStatus"/>の値</returns>
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
