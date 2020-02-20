using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeatMonitoringApplication
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Hostの読み込み
            string host = ConfigurationManager.AppSettings["Host"];
            if (host == null)
            {
                MessageBox.Show(@"""Host""が読み込めませんでした", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 通知設定の読み込み
            string isNotify = ConfigurationManager.AppSettings["IsNotify"];
            if (isNotify == null)
            {
                MessageBox.Show(@"""isNotify""が読み込めませんでした", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var httpClient = new MyHttpClient();
            var toastNotificationManager = new ToastNotificationManagerWrapper();
            var seatToastNotifier = new SeatToastNotifier(toastNotificationManager, Application.ExecutablePath);
            
            var periodicNotifier = new PeriodicNotifier(new SeatMonitoringApiClient(host, httpClient));

            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsNotify"]))
                {
                    // 通知設定がtrueならトースト通知を通知先として追加する
                    periodicNotifier.Destination += seatToastNotifier.Notify;
                }
            }
            catch(FormatException)
            {
                // 通知設定のbool変換に失敗した場合
                MessageBox.Show(@"""isNotify""の値は""true""か""false""に設定してください", "起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(periodicNotifier));

            httpClient.Dispose();
        }
    }
}
