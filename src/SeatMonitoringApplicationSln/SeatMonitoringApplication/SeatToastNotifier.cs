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

        /// <summary>
        /// <see cref="Notify(List{Seat}, bool)"/>に渡された第1引数のseatsを保持する
        /// </summary>
        private List<Seat> latestSeats = null;

        /// <summary>
        /// <see cref="Notify(List{Seat}, bool)"/>に渡された第2引数のisSucceededを保持する
        /// </summary>
        private bool latestIsSucceeded = true;

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
        /// <see cref="ISeatToastNotifier.Notify(List{Seat}, bool)"/>
        /// </summary>
        public void Notify(List<Seat> Seats, bool isSucceeded)
        {
            // アプリ起動後初回の通知時
            if (latestSeats == null)
            {
                latestSeats = Seats;
                latestIsSucceeded = isSucceeded;
                return;
            }

            // 状態取得に失敗した通知が来た時
            if (!isSucceeded)
            {
                // 前回の通知が状態取得に成功していた場合、サーバエラーをトースト通知する
                if (latestIsSucceeded)
                {
                    toastNotificationManager.Show(applicationId, CreateToastNotification(null, isSucceeded));
                    latestIsSucceeded = isSucceeded;
                }
                return;
            }
            else
            {
                var statusChangeSeats = new List<Seat>();
                if (latestIsSucceeded)
                {
                    statusChangeSeats = Seats.Where(currentSeat =>
                    {
                        foreach (var pastSeat in latestSeats)
                        {
                            if (pastSeat.name == currentSeat.name)
                            {
                                return pastSeat.status.CompareTo(currentSeat.status) != 0;
                            }
                        }
                        return false;
                    })
                    .ToList();
                    
                    foreach (Seat statusChangeSeat in statusChangeSeats)
                    {
                        toastNotificationManager.Show(applicationId, CreateToastNotification(statusChangeSeat, isSucceeded));
                    }
                }
                // サーバエラーから復帰した場合
                else
                {
                    toastNotificationManager.Show(applicationId, CreateToastNotification(null, isSucceeded));
                }

                latestIsSucceeded = isSucceeded;
                latestSeats = Seats;
            }
        }

        /// <summary>
        /// トースト通知の内容となるXmlを作成するメソッド
        /// </summary>
        /// <param name="seat">監視座席名と座席状態</param>
        /// <param name="isSucceeded">監視座席の状態取得の可否</param>
        /// <returns>作成されたXml</returns>
        private XmlDocument CreateToastNotification(Seat seat, bool isSucceeded)
        {
            string icon = null;
            string text = null;

            if (isSucceeded && latestIsSucceeded)
            {
                switch (seat.status)
                {
                    case Seat.SeatStatus.Exists:
                        icon = StatusIcon.GetExistIcon();
                        text = $@"""{seat.name}""さんは在席しています。";
                        break;

                    case Seat.SeatStatus.NotExists:
                        icon = StatusIcon.GetNotExistIcon();
                        text = $@"""{seat.name}""さんが離席しました。";
                        break;

                    case Seat.SeatStatus.Failure:
                        icon = StatusIcon.GetFailureIcon();
                        text = $@"""{seat.name}""さんの状態取得に失敗しました。";
                        break;

                    default:
                        break;
                }
            }
            else if (isSucceeded && !latestIsSucceeded)
            {
                icon = StatusIcon.GetReturnFromErrorIcon();
                text = "サーバ接続エラーから復帰しました。";
            }
            else
            {
                icon = StatusIcon.GetErrorIcon();
                text = "サーバへの接続に失敗しました。";
            }



            var toastContent = new ToastContent()
            {
                Header = new ToastHeader("Seat Monitoring Application-Toast Notification", "座席監視アプリ", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = icon,
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
