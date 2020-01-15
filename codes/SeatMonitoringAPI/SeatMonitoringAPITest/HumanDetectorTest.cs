using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class HumanDetectorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\Exists画像.jpg"))
            {
                var humanDetector = new HumanDetector();

                Assert.IsTrue(humanDetector.Detect(photo));
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\NotExists画像.jpg"))
            {
                var humanDetector = new HumanDetector();

                Assert.IsFalse(humanDetector.Detect(photo));
            }
        }
    }
}
