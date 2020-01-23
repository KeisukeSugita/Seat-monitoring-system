using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var ipAddress = Configuration.Instance.IpAddress;
            PeriodicNotifier.Destination destination = Update;
            PeriodicNotifier = new PeriodicNotifier(destination, new SeatMonitoringApiClient(ipAddress, new MyHttpClient()));
            InitializeComponent();
        }

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

        private void Update(List<Seat> seats, bool isSucceeded)
        {
            Invoke(new Action<List<Seat>, bool>(InvokeUpdate), seats, isSucceeded);
        }

        private void InvokeUpdate(List<Seat> seats, bool isSucceeded)
        {
            listView1.Items.Clear();

            if (!isSucceeded)
            {
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
                listView1.Items.AddRange(
                    seats.Select(seat => new ListViewItem(seat.Name)
                    {
                        ImageIndex = (int)seat.Status,
                        ToolTipText = GetText((ToolTipTexts)Enum.ToObject(typeof(ToolTipTexts), (int)seat.Status))
                    })
                    .ToArray()
                    );
            }
        }

        public void Close(object sender, EventArgs e)
        {
            PeriodicNotifier.Stop();
        }
    }
}