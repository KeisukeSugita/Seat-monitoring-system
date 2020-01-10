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
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(@"[{""Moniker"":""USB#VID_046D&PID_0826&MI_02#6&24BF100&0&0002"",""Name"":""杉田 圭輔""},
{""Moniker"":""USB#VID_046D&PID_0826&MI_02#6&24BF100&0&0003"",""Name"":""Keisuke Sugita""}]"));
            using (var streamReader = new StreamReader(memoryStream))
            {
                Configuration.initialize(streamReader);
                var configuration = Configuration.Instance.SeatDefinitions;
                Assert.AreEqual(configuration[0].Moniker, "USB#VID_046D&PID_0826&MI_02#6&24BF100&0&0002");
                Assert.AreEqual(configuration[0].Name, "杉田 圭輔");
                Assert.AreEqual(configuration[1].Moniker, "USB#VID_046D&PID_0826&MI_02#6&24BF100&0&0003");
                Assert.AreEqual(configuration[1].Name, "Keisuke Sugita");
            }
        }
    }
}
