using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectWithOtherRectangles(this Rectangle candidate, List<Rectangle> rectangleList)
        {
            return rectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
        }


        public static double DistanceTo(this Rectangle rectangle, Vector cloudCenter)
        {
            var distVector = (Vector) rectangle + (Vector) rectangle.Size / 2 - cloudCenter;
            return Math.Sqrt(distVector.X* distVector.X + distVector.Y * distVector.Y);
        }
    }
}