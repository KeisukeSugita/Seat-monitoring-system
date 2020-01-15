using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                var configuration = Configuration.Instance.SeatDefinitions;
            }
            catch(InvalidOperationException e)
            {
                Assert.AreEqual(e.Message, "Configurationが初期化されていません。");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""}]"));
            using (var streamReader = new StreamReader(memoryStream))
            {
                Configuration.Initialize(streamReader);
                var configuration = Configuration.Instance.SeatDefinitions;
                Assert.AreEqual(configuration[0].Moniker, "usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002");
                Assert.AreEqual(configuration[0].Name, "杉田 圭輔");
                Assert.AreEqual(configuration[1].Moniker, "usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003");
                Assert.AreEqual(configuration[1].Name, "Keisuke Sugita");
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""usb#vid_046d&pid_0826&mi_02#6&24bf100&0&0003"",""Name"":""Keisuke Sugita""}]"));
            using (var streamReader = new StreamReader(memoryStream))
            {
                try
                {
                    Configuration.Initialize(streamReader);
                }
                catch(InvalidOperationException e)
                {
                    Assert.AreEqual(e.Message, "Configurationは既に初期化されています。");
                }
            }
        }
    }
}
