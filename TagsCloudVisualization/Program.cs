using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var layout = new CircularCloudLayouter(new Point(400, 400));
            layout.PutNextRectangle(new Size(500, 90));
            for (var i = 0; i < 60; i++)
            {
                layout.PutNextRectangle(new Size(random.Next(40,160), random.Next(20,60)));
            }
            layout.DrawCloudTag();
            
        }
    }
}
