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
        private int dDeg = 10;
        private int dRadius = 5;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates must be nonnegative.");

            CloudCenter = new Vector(center);
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size should be non-negative");

            var sizeVector = new Vector(rectangleSize);
            Rectangle newRectagle;
            if (RectangleList.Count == 0)
            {
                radius = Math.Min(rectangleSize.Height, rectangleSize.Width) / 2.0;
                var leftTop = CloudCenter - sizeVector / 2;
                newRectagle = new Rectangle(leftTop.ToPoint(), sizeVector.ToSize());
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
                var startDeg = random.Next(360);
                for (var deg = startDeg; deg < startDeg + 360; deg += dDeg)
                {
                    var rad = deg / Math.PI * 180.0;
                    var newRectangleCenter = new Vector(
                        (int)(CloudCenter.X + curentRadius * Math.Cos(rad)),
                        (int)(CloudCenter.Y + curentRadius * Math.Sin(rad)));
                    var leftTop = newRectangleCenter - sizeVector / 2;
                    if (leftTop.X < 0 || leftTop.Y / 2 < 0)
                        continue;
                    var candidate = new Rectangle(leftTop.ToPoint(), sizeVector.ToSize());
                    if (!ColisionWithOtherRectangles(candidate))
                        return candidate;
                }
                curentRadius += dRadius;
            }
        }

        private bool ColisionWithOtherRectangles(Rectangle candidate)
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
        public void PutNextRectangle_ShouldReturnRectangle_WithCorrectSize()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var rectangle = layout.PutNextRectangle(new Size(20, 25));
            rectangle.Size.ShouldBeEquivalentTo(new Size(20,25));

        }
        [Test]
        public void PutNextReactangle_FirstRectangle_ShouldBeOnCloudCenter()
        {
            var cloudCenter = new Point(100, 100);
            var layout = new CircularCloudLayouter(cloudCenter);           
            var firstRectangle = layout.PutNextRectangle(new Size(10,10));
            var centerOfRectangle = new Point(firstRectangle.X + firstRectangle.Width / 2,
                firstRectangle.Y + firstRectangle.Height / 2);

            centerOfRectangle.ShouldBeEquivalentTo(cloudCenter);
        }

        [Test]
        public void PutNextReactangle_TwoAddedRectangles_ShouldNotIntersect()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var firstRectangle = layout.PutNextRectangle(new Size(20, 25));
            var secondRectangle = layout.PutNextRectangle(new Size(20, 25));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PairwiseIntersectionShouldBeFalse_OnBigNumberOfRectangles()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var rectangleList = new List<Rectangle>();
            for (var i = 0; i < 10; i++)
                rectangleList.Add(layout.PutNextRectangle(new Size(10, 10)));

            PairwiseIntersection(rectangleList).Should().BeFalse();
        }

        private static bool PairwiseIntersection(List<Rectangle> rectangleList)
        {
            for (var i = 0; i < rectangleList.Count; i++)
                for (var j = i+1; j <rectangleList.Count; j++)
                    if (rectangleList[j].IntersectsWith(rectangleList[i]) && i != j)
                        return true;
            return false;
        }
    }
}