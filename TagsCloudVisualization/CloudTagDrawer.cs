using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public static class CloudTagDrawer
    {
        public static void DrawToFile(Point cloudCenter, List<Rectangle> rectangleList, string name)
        {
            var bitmap = new Bitmap(800, 800);
            var g = Graphics.FromImage(bitmap);
            var selPen = new Pen(Color.Blue);
            g.DrawRectangle(new Pen(Color.Red), (int) cloudCenter.X, (int) cloudCenter.Y, 1, 1);
            foreach (var rectangle in rectangleList)
                g.DrawRectangle(selPen, rectangle);
            g.Dispose();
            bitmap.Save(name);
        }

    }
}