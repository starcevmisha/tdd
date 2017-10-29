using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var cloudCenter = new Point(70, 70);
            var layout = new CircularCloudLayouter(cloudCenter);
            var rectangleList = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                rectangleList.Add(layout.PutNextRectangle(new Size(10,10)));
            }
            CloudTagDrawer.DrawToFile(cloudCenter, rectangleList, "1.bmp", 800, 800);
            CloudTagDrawer.DrawToForm(cloudCenter,rectangleList, 800, 800);

        }
    }
}
