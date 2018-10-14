using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.OpenCV.ImageComposition
{
    /// <summary>
    /// defines image paths
    /// </summary>
    public static class Globals
    {
        public static string UNDERLYING_IMG_PATH = @"..\..\resource\underlying.png";
        public static string OVERLAY_IMG_PATH = @"..\..\resource\overlay.png";
    }

    class Program
    {
        /// <summary>
        /// function AlphaBlending()
        /// </summary>
        /// <param name="underlying_image"></param>
        /// <param name="overlay_image"></param>
        /// <returns></returns>
        private static Mat AlphaBlending(Mat underlying_image, Mat overlay_image)
        {

            Mat result_image = underlying_image.Clone();

            int imageWidth = underlying_image.Width;
            int imageHeight = underlying_image.Height;
            int i, j;

            for (i = 0; i < imageWidth; i++)
            {
                for (j = 0; j < imageHeight; j++)
                {
                    var underlying_color = underlying_image.Get<Vec4b>(j, i);
                    var overlay_color = overlay_image.Get<Vec4b>(j, i);
                    var result_color = new Vec4b();

                    int alpha = (int)(overlay_color.Item3 / 255);

                    // calculate alpha blending
                    result_color.Item0 = (byte)(overlay_color.Item0 * alpha + underlying_color.Item0 * (1 - alpha));
                    result_color.Item1 = (byte)(overlay_color.Item1 * alpha + underlying_color.Item1 * (1 - alpha));
                    result_color.Item2 = (byte)(overlay_color.Item2 * alpha + underlying_color.Item2 * (1 - alpha));
                    result_color.Item3 = (byte)255;

                    result_image.Set(j, i, result_color);
                }
            }

            return result_image;

        }

        /// <summary>
        /// function Main()
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Mat underlying_image = Cv2.ImRead(Globals.UNDERLYING_IMG_PATH, ImreadModes.Unchanged);
            Mat overlay_image = Cv2.ImRead(Globals.OVERLAY_IMG_PATH, ImreadModes.Unchanged);

            Mat result_image = AlphaBlending(underlying_image, overlay_image);

            // display image in a window
            using (var window = new Window("window", image: result_image, flags: WindowMode.AutoSize))
            {
                Cv2.WaitKey();
            }

        }
    }
}