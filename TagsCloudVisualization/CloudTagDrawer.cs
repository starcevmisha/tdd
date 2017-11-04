using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public static class CloudTagDrawer
    {
        public static void DrawToFile(Point cloudCenter, List<Rectangle> rectangleList, string name, int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                DrawRectangleOnBitmap(cloudCenter, rectangleList, bitmap);
                bitmap.Save(name);
            }
        }

        public static void DrawToForm(Point cloudCenter, List<Rectangle> rectangleList, Dictionary<string, int> tagsDictionary, int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                DrawRectangleOnBitmap(cloudCenter, rectangleList, bitmap);
                Form aForm = new Form();
                aForm.Width = 800;
                aForm.Height = 800;
                aForm.BackgroundImage = bitmap;
                aForm.ShowDialog();
            }
        }

        private static void DrawRectangleOnBitmap(Point cloudCenter, List<Rectangle> rectangleList, Bitmap bitmap)
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                var selPen = new Pen(Color.Blue);
//                g.DrawString("Hello");
                g.DrawRectangle(new Pen(Color.Red), (int) cloudCenter.X, (int) cloudCenter.Y, 1, 1);
                foreach (var rectangle in rectangleList)
                    g.DrawRectangle(selPen, rectangle);
            }
        }
    }
}