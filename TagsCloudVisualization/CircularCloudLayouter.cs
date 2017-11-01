using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Vector CloudCenter;
        private List<Rectangle> RectangleList;
        private readonly Random random = new Random();
        private double radius = 10;
        private const double AngleShift = Math.PI/18;
        private const int RadiusShift = 10;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException($"Coordinates must be non-negative, but was ({center.X},{center.Y})");

            CloudCenter = new Vector(center);
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException($"Size should be Positive, but was ({rectangleSize.Height}, {rectangleSize.Width})");

            var sizeVector = new Vector(rectangleSize);
            Rectangle newRectagle;
            if (RectangleList.Count == 0)
            {
                radius = Math.Min(rectangleSize.Height, rectangleSize.Width) / 2.0;
                var leftTop = CloudCenter - sizeVector / 2;
                newRectagle = new Rectangle(leftTop, sizeVector);
            }
            else
                newRectagle = FindPositionOfNewRectangle(sizeVector);
            RectangleList.Add(newRectagle);
            return newRectagle;
        }

        private Rectangle FindPositionOfNewRectangle(Vector sizeVector)
        {
            var curentRadius = radius;
            while (true)
            {
                var startAngle = random.NextDouble() * 2 * Math.PI;
                for (var angle = startAngle; angle < startAngle + 2*Math.PI; angle += AngleShift)
                {
                    var newRectangleCenter = CloudCenter + curentRadius * Vector.Angle(angle);
                    var leftTop = newRectangleCenter - sizeVector / 2;
                    var candidate = new Rectangle(leftTop, sizeVector);
                    if (!ColisionWithOtherRectangles(candidate))
                        return candidate;
                }
                curentRadius += RadiusShift;
            }
        }

//        ////CR(epeshk): эта и анлогичные реализации не должны проходить тесты
//        private Rectangle FindPositionOfNewRectangle(Vector sizeVector)
//        {
//            var vec = new Vector(1, 0);
//            while (true)
//            {
//                var point = CloudCenter + vec;
//                var candidate = new Rectangle(point, new Vector(1, 1));
//                if (!ColisionWithOtherRectangles(candidate))
//                    return candidate;
//                vec += new Vector(1, 0);
//            }
//        }

        private bool ColisionWithOtherRectangles(Rectangle candidate)
        {
            return RectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
        }
    }


   
}