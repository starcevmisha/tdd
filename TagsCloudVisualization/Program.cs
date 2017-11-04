using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    class Program
    {
        private static readonly Dictionary<string, int> tagsDictionary = new Dictionary<string, int>()
        {
            {"Participation", 100},
            {"Usability", 50},
            {"Design", 40},
            {"Standartization", 30},
            {"Python", 20}
       
        };
        static void Main(string[] args)
        {
            var cloudCenter = new Point(400, 400);
            var layout = new CircularCloudLayouter(cloudCenter);
            var rectangleList = new List<Rectangle>();

            foreach (var tag in tagsDictionary)
            {
                var tagSize = TextRenderer.MeasureText(tag.Key,
                    new Font(new FontFamily("Arial"), tag.Value, FontStyle.Regular, GraphicsUnit.Pixel));
                rectangleList.Add(layout.PutNextRectangle(tagSize));
            }

            CloudTagDrawer.DrawToFile(cloudCenter, rectangleList, "1.bmp", 800, 800);
            CloudTagDrawer.DrawToForm(cloudCenter,rectangleList, tagsDictionary,800, 800);

        }
    }
}
