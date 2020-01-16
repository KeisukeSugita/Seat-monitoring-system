using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeatMonitoringAPI.Models;

namespace SeatMonitoringAPITest
{
    [TestClass]
    public class HumanDetectorTest
    {
        // 人が写っている写真を渡したとき、戻り値がtrueかのテスト
        [TestMethod]
        public void Detect_HumanInThePhoto_ReturnTrue()
        {
            using (var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\Exists画像.jpg"))
            {
                var humanDetector = new HumanDetector();

                Assert.IsTrue(humanDetector.Detect(photo));
            }
        }

        // 人が写っていない写真を渡したとき、戻り値がFalseかのテスト
        [TestMethod]
        public void Detect_HumanNotInThePhoto_ReturnFalse()
        {
            using (var photo = new Bitmap(@"C:\Users\z00s600157\Pictures\SeatMonitoringAPITest用画像\NotExists画像.jpg"))
            {
                var humanDetector = new HumanDetector();

                Assert.IsFalse(humanDetector.Detect(photo));
            }
        }
    }
}
