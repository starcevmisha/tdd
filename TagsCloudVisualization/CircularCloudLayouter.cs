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
        public Point CloudCenter;
        public List<Rectangle> RectangleList;
        private readonly Random random = new Random();
        private double radius = 10;
        private int dDeg = 10;
        private int dRadius = 5;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be nonnegative.");

            CloudCenter = center;
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size should be non-negative");

            Rectangle newRectagle;
            if (RectangleList.Count == 0)
            {
                radius = Math.Min(rectangleSize.Height, rectangleSize.Width) / 2;
                newRectagle = new Rectangle(new Point(CloudCenter.X - rectangleSize.Width / 2,
                        CloudCenter.Y - rectangleSize.Height / 2),
                    rectangleSize);
            }
            else
                newRectagle = FindPositionOfNewRectangle(rectangleSize);

            RectangleList.Add(newRectagle);
            return newRectagle;
        }

        private Rectangle FindPositionOfNewRectangle(Size rectangleSize)
        {
            var curentRadius = radius;
            var newRectagle = new Rectangle();
            var done = false;
            while (!done)
            {
                var startDeg = random.Next(360);
                for (var deg = startDeg; deg < startDeg + 360; deg += dDeg)
                {
                    var rad = deg / Math.PI * 180.0;
                    var newRectangleCenterX = (int) (CloudCenter.X + curentRadius * Math.Cos(rad));
                    var newRectangleCenterY = (int) (CloudCenter.Y + curentRadius * Math.Sin(rad));

                    var leftTopX = newRectangleCenterX - rectangleSize.Width / 2;
                    var leftTopY = newRectangleCenterY - rectangleSize.Height / 2;

                    if (leftTopX < 0 || leftTopY / 2 < 0)
                        continue;
                    var candidate = new Rectangle(new Point(leftTopX, leftTopY), rectangleSize);
                    if (!ColisionWithOtherRectangles(candidate))
                    {
                        newRectagle = candidate;
                        done = true;
                        break;
                    }
                }
                curentRadius += dRadius;
            }
            return newRectagle;
        }

        public void DrawCloudTag()
        {
            var bitmap = new Bitmap(800, 800);
            var g = Graphics.FromImage(bitmap);

            var selPen = new Pen(Color.Blue);
            g.DrawRectangle(new Pen(Color.Red), CloudCenter.X, CloudCenter.Y, 1, 1);
            foreach (var rectangle in RectangleList)
                g.DrawRectangle(selPen, rectangle);
            g.Dispose();
            bitmap.Save("1.bmp");
        }

        public bool ColisionWithOtherRectangles(Rectangle candidate)
        {
            return RectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
        }
    }


    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        [Test]
        public void TestConstructor_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(-1, -1)));
        }

        [Test]
        public void PutNextRectangle_ThrowArgumentException_OnNonPositiveSize()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var size = new Size(-10, 10);
            Assert.Throws<ArgumentException>(() => layout.PutNextRectangle(size));
        }

        [Test]
        public void PutNextRectangle_ShouldContainsOneRectangle_AfterFirstAdding()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            layout.PutNextRectangle(new Size(20, 25));
            layout.RectangleList.Count.Should().Be(1);
        }

        [Test]
        public void PutNextReactangle_FirstRectaangleShouldBeOnCloudCenter()
        {
            var layout = new CircularCloudLayouter(new Point(100, 100));
            layout.PutNextRectangle(new Size(10, 10));

            var firstRectangle = layout.RectangleList[0];
            var centerOfRectangle = new Point(firstRectangle.X + firstRectangle.Width / 2,
                firstRectangle.Y + firstRectangle.Height / 2);
            centerOfRectangle.ShouldBeEquivalentTo(layout.CloudCenter);
        }


        [Test]
        public void PutNextRectangle_PairwiseIntersectionShouldBeFalse_OnBigNumberOfRectangles()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            for (var i = 0; i < 10; i++)
                layout.PutNextRectangle(new Size(10, 10));
            layout.DrawCloudTag();
            PairwiseIntersection(layout).Should().BeFalse();
        }

        private static bool PairwiseIntersection(CircularCloudLayouter layout)
        {
            var colision = false;
            for (var i = 0; i < layout.RectangleList.Count; i++)
            {
                for (var j = 0; j < layout.RectangleList.Count; j++)
                    if (layout.RectangleList[j].IntersectsWith(layout.RectangleList[i]) && i != j)
                    {
                        colision = true;
                        break;
                    }
                if (colision)
                    break;
            }
            return colision;
        }
    }
}