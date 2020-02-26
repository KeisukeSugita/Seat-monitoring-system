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

        // 前回の通知の監視座席名とその状態を、通知した座席に限らず全て保持する
        private List<Seat> pastSeats = null;

        // 前回の通知の監視座席名とその状態取得に成功していたか否かを保持する
        private bool pastSucceeded = true;

        private StatusIcon statusIcon = new StatusIcon();

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
        public void Notify(List<Seat> latestSeats, bool latestSucceeded)
        {
            // アプリ起動後初回の通知時
            if (pastSeats == null)
            {
                pastSeats = latestSeats;
                pastSucceeded = latestSucceeded;
                return;
            }

            // 状態取得に失敗した通知が来た時
            if (!latestSucceeded)
            {
                // 前回の通知が状態取得に成功していた場合、サーバエラーをトースト通知する
                if (pastSucceeded)
                {
                    toastNotificationManager.Show(applicationId, CreateToastNotification(null, latestSucceeded, pastSucceeded));
                    pastSucceeded = latestSucceeded;
                }
                return;
            }
            else
            {
                var statusChangeSeats = new List<Seat>();
                if (pastSucceeded)
                {
                    statusChangeSeats = latestSeats.Where(currentSeat =>
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
                    
                    foreach (Seat statusChangeSeat in statusChangeSeats)
                    {
                        toastNotificationManager.Show(applicationId, CreateToastNotification(statusChangeSeat, latestSucceeded, pastSucceeded));
                    }
                }
                // サーバエラーから復帰した場合
                else
                {
                    toastNotificationManager.Show(applicationId, CreateToastNotification(null, latestSucceeded, pastSucceeded));
                }

                pastSucceeded = latestSucceeded;
                pastSeats = latestSeats;
            }
        }

        /// <summary>
        /// トースト通知の内容となるXmlを作成するメソッド
        /// </summary>
        /// <param name="seat">監視座席名と座席状態</param>
        /// <param name="isSucceeded">監視座席の状態取得の可否</param>
        /// <returns>作成されたXml</returns>
        private XmlDocument CreateToastNotification(Seat seat, bool isSucceeded, bool wasSucceeded)
        {
            string icon = null;
            string text = null;

            if (isSucceeded && wasSucceeded)
            {
                switch (seat.status)
                {
                    case Seat.SeatStatus.Exists:
                        icon = statusIcon.GetExistIcon();
                        text = $@"""{seat.name}""さんは在席しています。";
                        break;

                    case Seat.SeatStatus.NotExists:
                        icon = statusIcon.GetNotExistIcon();
                        text = $@"""{seat.name}""さんが離席しました。";
                        break;

                    case Seat.SeatStatus.Failure:
                        icon = statusIcon.GetFailureIcon();
                        text = $@"""{seat.name}""さんの状態取得に失敗しました。";
                        break;

                    default:
                        break;
                }
            }
            else if (isSucceeded && !wasSucceeded)
            {
                icon = statusIcon.GetReturnFromErrorIcon();
                text = "サーバ接続エラーから復帰しました。";
            }
            else
            {
                icon = statusIcon.GetErrorIcon();
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
