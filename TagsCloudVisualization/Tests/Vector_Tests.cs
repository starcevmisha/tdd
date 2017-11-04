using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Vector_Tests
    {
        [Test]
        public void Vector_AdditionTest()
        {
            var a = new Vector(2, 3);
            var b = new Vector(1, 7);
            var sum = a + b;
            var expectedSum = new Vector(3, 10);
            sum.Should().Be(expectedSum);
        }

        [Test]
        public void Vector_SubtractionTest()
        {
            var a = new Vector(2, 3);
            var b = new Vector(1, 7);
            var diff = a - b;
            var expectedDiff = new Vector(1, -4);
            diff.Should().Be(expectedDiff);
        }

        [Test]
        public void Vector_ShouldThrowArgumentException_WhenDividedByZero()
        {
            var a = new Vector(2, 3);
            Vector b = a / 0;
        }

        [Test]
        public void Vector_MultiplicationByAConstantTest()
        {
            var a = new Vector(2, 3);
            var product = 2 * a;
            var expected = new Vector(4, 6);
            product.Should().Be(expected);
        }

        [Test]
        public void Vector_DivideByAConstantTest()
        {
            var a = new Vector(2, 4);
            var division = a / 2;
            var expected = new Vector(1, 2);
            division.Should().Be(expected);
        }

        [Test]
        public void Vector_LengthTest()
        {
            var vector = new Vector(3, 4);
            var expectedLength = 5;
            vector.Length().Should().Be(expectedLength);
        }
        [Test]
        public void Vector_ReturnCorrectAngle()
        {
            var angle = Vector.Angle(Math.PI);
            var expectedAngle = new Vector(-1, 0);
            angle.Should().Be(expectedAngle);
        }

        [Test]
        public void Vector_CastToPointTest()
        {
            var point = new Point(1, 2);
            var vector = new Vector(1, 2);
            var pointVector = (Point)(vector);
            pointVector.Should().Be(point);
        }

        [Test]
        public void Vector_CastToSizeTest()
        {
            var size = new Size(1, 2);
            var vector = new Vector(1, 2);
            var pointVector = (Size)(vector);
            pointVector.Should().Be(size);
        }
    }
}