﻿using System;
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
        private IPeriodicNotifier PeriodicNotifier { get; set; }
        
        /// <summary>
        /// PeriodicNotifierクラスのインスタンスをフィールドに格納し、
        /// UpdateメソッドをPeriodicNotifierクラスの通知先として追加するコンストラクタ
        /// </summary>
        /// <param name="periodicNotifier"></param>
        public MainForm(IPeriodicNotifier periodicNotifier)
        {
            PeriodicNotifier = periodicNotifier;
            PeriodicNotifier.Destination += Update;
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
        /// UIスレッドで更新を行いたいため、Invokeで処理を行う
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
                        new ListViewItem("サーバ接続失敗")
                        {
                            ImageKey = "サーバ接続エラーアイコン.png",
                            ToolTipText = "サーバ接続エラー"
                        }
                        );
                }
                else
                {
                    // seatsのデータをListViewの項目に追加する
                    listView1.Items.AddRange(
                        seats.Select(seat => new ListViewItem(seat.name)
                        {
                            ImageKey = $"{seat.SeatStatusLabel[seat.status]}アイコン.png",
                            ToolTipText = seat.SeatStatusLabel[seat.status]
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
        }
    }
}