using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SeatMonitoringApplication
{
    /// <summary>
    /// SeatMonitoringApplication用のトースト通知を行うクラス
    /// </summary>
    public class SeatToastNotifier :ISeatToastNotifier
    {
        // トースト通知を出すアプリケーションを指定するID
        private readonly string applicationId;
        // 通知機能の部分をラップしたインターフェース
        private readonly IToastNotificationManagerWrapper toastNotificationManager;

        // 過去1回分の監視座席名とその状態を保持する
        private List<Seat> pastSeats = null;

        /// <summary>
        /// フィールドの初期化を行うコンストラクタ
        /// </summary>
        /// <param name="toastNotificationManager">トースト通知機能をラップしたインターフェース</param>
        /// <param name="applicationId">トースト通知を出すアプリケーションを指定するID</param>
        public SeatToastNotifier(IToastNotificationManagerWrapper toastNotificationManager, string applicationId)
        {
            this.toastNotificationManager = toastNotificationManager;
            this.applicationId = applicationId;
        }

        /// <summary>
        /// 状態が変化している座席についてトースト通知を送信するメソッド
        /// </summary>
        /// <param name="currentSeats">監視座席名とその状態のリスト</param>
        /// <param name="isSucceeded">監視座席の状態取得の可否</param>
        public void Notify(List<Seat> currentSeats, bool isSucceeded)
        {
            if (pastSeats == null)
            {
                pastSeats = currentSeats;
                return;
            }

            if (!isSucceeded)
            {
                toastNotificationManager.Show(applicationId, CreateToastNotification(null, isSucceeded));
                pastSeats = null;
                return;
            }
            else
            {
                var statusChangeSeats = currentSeats.Where(currentSeat =>
                {
                    foreach (var pastSeat in pastSeats)
                    {
                        if (pastSeat.name == currentSeat.name)
                        {
                            return pastSeat.status.CompareTo(currentSeat.status) != 0;
                        }
                    }
                    return false;
                })
                .ToList();

                statusChangeSeats.ForEach(statusChangeSeat => toastNotificationManager.Show(applicationId, CreateToastNotification(statusChangeSeat, isSucceeded)));

                pastSeats = currentSeats;
            }
        }

        /// <summary>
        /// トースト通知の内容となるXmlを作成するメソッド
        /// </summary>
        /// <param name="seat">監視座席名と座席状態</param>
        /// <param name="isSucceeded">監視座席の状態取得の可否</param>
        /// <returns></returns>
        private XmlDocument CreateToastNotification(Seat seat, bool isSucceeded)
        {
            string iconName;
            string text = null;

            if (isSucceeded)
            {
                iconName = seat.SeatStatusLabel[seat.status];
                switch (seat.status)
                {
                    case Seat.SeatStatus.Exists:
                        text = $@"""{seat.name}""さんは在席しています。";
                        break;

                    case Seat.SeatStatus.NotExists:
                        text = $@"""{seat.name}""さんが離席しました。";
                        break;

                    case Seat.SeatStatus.Failure:
                        text = $@"""{seat.name}""さんの状態取得に失敗しました。";
                        break;

                    default:
                        break;
                }
            }
            else
            {
                iconName = "サーバ接続エラー";
                text = "サーバへの接続に失敗しました。";
            }

            var toastContent = new ToastContent()
            {
                Header = new ToastHeader("0001", "座席監視アプリ", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = $@"{Application.StartupPath}\Icons\{iconName}アイコン.png",
                            HintCrop = ToastGenericAppLogoCrop.Default
                        },

                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = text,
                            }
                        }
                    }
                }
            };

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastContent.GetContent());

            return xmlDoc;
        }
    }
}
