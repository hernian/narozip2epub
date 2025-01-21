using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace narozip2mobi
{
    internal class CoverImageGenerator(string title)
    {
        private const int PICS_WIDTH_COVER = 1600;
        private const int PICS_HEIGHT_COVER = 2560;
        private const int MARGIN_HORZ = 64;
        private const int MARGIN_VART = 64;
        private const string FONT_NAME = "メイリオ";
        private const float FONT_SIZE = 192.0F;
        private const GraphicsUnit FONT_UNIT = GraphicsUnit.Pixel;

        private readonly string _title = title;

        public void Generate(string pathCoverImg)
        {
            var imageCover = new Bitmap(PICS_WIDTH_COVER, PICS_HEIGHT_COVER, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(imageCover))
            {
                g.Clear(Color.White);
                var left = MARGIN_HORZ;
                var top = MARGIN_VART;
                var width = PICS_WIDTH_COVER - MARGIN_HORZ * 2;
                var height = PICS_HEIGHT_COVER - MARGIN_VART * 2;
                var rect = new Rectangle(left, top, width, height);
                var font = new Font(FONT_NAME, FONT_SIZE, FontStyle.Regular, FONT_UNIT);
                var brush = new SolidBrush(Color.White);
                var stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                };
                g.DrawString(_title, font, Brushes.Black, rect, stringFormat);
            }
            imageCover.Save(pathCoverImg, ImageFormat.Jpeg);
        }
    }
}

