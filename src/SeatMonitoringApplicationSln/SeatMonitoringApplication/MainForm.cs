using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeatMonitoringApplication
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// ToolTipで表示するテキストのenum
        /// </summary>
        public enum ToolTipTexts
        {
            [Description("在席")]
            Exist,

            [Description("離席")]
            NotExist,

            [Description("不明")] 
            Unknown,

            [Description("サーバ接続エラー")] 
            ConectingError
        }


        public IMyHttpClient httpClient = new MyHttpClient();

        /// <summary>
        /// ToolTipTextsから表示名を取得して返すメソッド
        /// </summary>
        /// <param name="toolTipText"></param>
        /// <returns>対応する表示名</returns>
        public string GetText(ToolTipTexts toolTipText)
        {
            var member = toolTipText.GetType().GetMember(toolTipText.ToString());
            var attributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = ((DescriptionAttribute)attributes[0]).Description;
            return description;
        }

        private IPeriodicNotifier PeriodicNotifier { get; set; }
        public MainForm()
        {
            PeriodicNotifier.Destination destination = Update;
            PeriodicNotifier = new PeriodicNotifier(destination, new SeatMonitoringApiClient(ConfigurationManager.AppSettings["Host"], httpClient));
            InitializeComponent();
        }

        /// <summary>
        /// テスト用コンストラクタ
        /// </summary>
        /// <param name="periodicNotifier"></param>
        public MainForm(IPeriodicNotifier periodicNotifier)
        {
            PeriodicNotifier = periodicNotifier;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.ShowItemToolTips = true;
            PeriodicNotifier.Start();
        }

        /// <summary>
        /// 画面を更新するメソッド
        /// UIスレッドで更新を行いたいため、InvokeUpdateメソッドをInvokeで呼び出す
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="isSucceeded"></param>
        private void Update(List<Seat> seats, bool isSucceeded)
        {
            Invoke((MethodInvoker)(() => {
                listView1.Items.Clear();

                if (!isSucceeded)
                {
                    // サーバ接続エラーを表す項目をListViewに追加する
                    listView1.Items.Add(
                        new ListViewItem("サーバへの接続に失敗しました。")
                        {
                            ImageIndex = (int)ToolTipTexts.ConectingError,
                            ToolTipText = GetText(ToolTipTexts.ConectingError)
                        }
                        );
                }
                else
                {
                    // seatsのデータをListViewの項目に追加する
                    listView1.Items.AddRange(
                        seats.Select(seat => new ListViewItem(seat.name)
                        {
                            ImageIndex = (int)seat.status,
                            ToolTipText = GetText((ToolTipTexts)Enum.ToObject(typeof(ToolTipTexts), (int)seat.status))
                        })
                        .ToArray()
                        );
                }
            }));
        }

        /// <summary>
        /// FormClosedイベントで呼ばれるメソッド
        /// 非同期で行っているPeriodicNotifierクラスの処理を終了させるため、
        /// PeriodicNotifier.Stopメソッドを呼び出す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Close(object sender, EventArgs e)
        {
            PeriodicNotifier.Stop();
        }


        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            httpClient.Dispose();
        }
    }
}