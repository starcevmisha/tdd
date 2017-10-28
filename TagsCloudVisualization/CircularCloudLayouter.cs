using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point cloudCenter;
        public List<Rectangle> rectangleList;
        private Random random = new Random();
        private double radius = 10;
        private int dDeg = 10;
        private int dRadius = 5;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be nonnegative.");

            cloudCenter = center;
            rectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size should be non-negative");

            Rectangle newRectagle;
            if (rectangleList.Count == 0)
            {
                radius = Math.Min(rectangleSize.Height, rectangleSize.Width) / 2;
                newRectagle = new Rectangle(new Point(cloudCenter.X - rectangleSize.Width / 2,
                        cloudCenter.Y - rectangleSize.Height / 2),
                    rectangleSize);
            }
            else
            {
                newRectagle = FindPositionOfNewRectangle(rectangleSize);
            }
            rectangleList.Add(newRectagle);
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
                    var rad = (double) deg / Math.PI * 180.0;
                    var newRectangleCenterX = (int) (cloudCenter.X + curentRadius * Math.Cos(rad));
                    var newRectangleCenterY = (int) (cloudCenter.Y + curentRadius * Math.Sin(rad));

                    var leftTopX = newRectangleCenterX - rectangleSize.Width / 2;
                    var leftTopY = newRectangleCenterY - rectangleSize.Height / 2;

                    if (leftTopX < 0 || leftTopY/2 < 0)
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
            g.DrawRectangle(new Pen(Color.Red), cloudCenter.X, cloudCenter.Y, 1, 1);
            foreach (var rectangle in rectangleList)
                g.DrawRectangle(selPen, rectangle);
            g.Dispose();
            bitmap.Save("1.bmp");
        }

        public bool ColisionWithOtherRectangles(Rectangle candidate)
        {
            return rectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
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
            var size = new Size(-10,10);
            Assert.Throws<ArgumentException>(() => layout.PutNextRectangle(size));
        }

        [Test]
        public void PutNextRectangle_ShouldContainsOneRectangle_AfterFirstAdding()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            layout.PutNextRectangle(new Size(20, 25));
            layout.rectangleList.Count.Should().Be(1);
        }

        [Test]
        public void PutNextReactangle_FirstRectaangleShouldBeOnCloudCenter()
        {
            var layout = new CircularCloudLayouter(new Point(100, 100));
            layout.PutNextRectangle(new Size(10, 10));

            var firstRectangle = layout.rectangleList[0];
            var centerOfRectangle = new Point(firstRectangle.X + firstRectangle.Width / 2,
                firstRectangle.Y + firstRectangle.Height / 2);
            centerOfRectangle.ShouldBeEquivalentTo(layout.cloudCenter);
        }


        [Test]
        public void PutNextRectangle_PairwiseIntersectionShouldBeFalse_OnBigNumberOfRectangles()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            for (var i = 0; i < 10; i++)
                layout.PutNextRectangle(new Size(10, 10));
            layout.DrawCloudTag();
            pairwiseIntersection(layout).Should().BeFalse();
        }

        private static bool pairwiseIntersection(CircularCloudLayouter layout)
        {
            var colision = false;
            for (var i = 0; i < layout.rectangleList.Count; i++)
            {
                for (var j = 0; j < layout.rectangleList.Count; j++)
                    if (layout.rectangleList[j].IntersectsWith(layout.rectangleList[i]) && i != j)
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