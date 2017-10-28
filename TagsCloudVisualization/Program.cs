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
            var cloudCenter = new Point(400, 400);
            var layout = new CircularCloudLayouter(cloudCenter);
            var rectangleList = new List<Rectangle>();
            rectangleList.Add(layout.PutNextRectangle(new Size(500, 90)));
            for (var i = 0; i < 60; i++)
            {
                rectangleList.Add(layout.PutNextRectangle(new Size(random.Next(40,160), random.Next(20,60))));
            }
            CloudTagDrawer.DrawToFile(cloudCenter, rectangleList, "1.bmp");

        }
    }
}
