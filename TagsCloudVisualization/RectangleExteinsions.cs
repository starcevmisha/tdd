using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectWithAny(this Rectangle candidate, IEnumerable<Rectangle> rectangleList)
        {
            return rectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
        }


        public static double DistanceTo(this Rectangle rectangle, Vector cloudCenter)
        {
            var distVector = rectangle.TopLeft() + (Vector) rectangle.Size / 2 - cloudCenter;
            return distVector.Length();
        }

        public static Vector TopLeft(this Rectangle rectangle)
        {
            return new Vector(rectangle.X, rectangle.Y);
        }
        public static Vector Center(this Rectangle rectangle)
        {
            return rectangle.TopLeft()+(Vector)rectangle.Size/2;
        }
        
    }

    [TestFixture]
    public class RectangleExteinsions_Should
    {
        [Test]
        public void IntersectWithAny_Test()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(5, 5, 10, 10),
                new Rectangle(15, 15, 10, 10)
            };
            var myRectangle = new Rectangle(12,12, 10,10);
            myRectangle.IntersectWithAny(rectangles).Should().BeTrue();
        }

        [Test]
        public void DistanceTo_Test()
        {
            var myRectangle = new Rectangle(0,0,10,10);
            var center = new Vector(9,8);
            myRectangle.DistanceTo(center).Should().Be(5);
        }

        [Test]
        public void TopLeft_Test()
        {
            var rectangle = new Rectangle(10,10,10,10);
            var expectedTopLeft = new Vector(10,10);
            rectangle.TopLeft().Should().Be(expectedTopLeft);
        }

        [Test]
        public void Center_Test()
        {
            var rectangle = new Rectangle(0, 0, 10, 10);
            var expectedCenter = new Vector(5, 5);
            rectangle.Center().Should().Be(expectedCenter);
        }
    }
}