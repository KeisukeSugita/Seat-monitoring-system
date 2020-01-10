using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SeatMonitoringAPI.Models
{
    public class HumanDetector
    {
        [DllImport("HumanDetector.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool detect(int rows, int cols, byte[] image);
        public bool Detect(Bitmap photo)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                photo.Save(memoryStream, ImageFormat.Bmp);
                byte[] image = memoryStream.ToArray();

                byte[] headerOffset = { image[10], image[11], image[12], image[13] };
                int offset = BitConverter.ToInt32(headerOffset, 0);

                var convertedImage = new byte[photo.Height * photo.Width * 3];
                for (int i = 0; i < convertedImage.Length; i++)
                {
                    convertedImage[i] = image[i + offset];
                }

                return detect(photo.Height, photo.Width, convertedImage);
            }
        }
    }
}