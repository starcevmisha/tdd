using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Vector
    {
        public double X;
        public double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Vector(Size s)
        {
            X = s.Width;
            Y = s.Height;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator /(Vector a, double c)
        {
            return new Vector(a.X / c, a.Y / c);
        }

        public static Vector operator *(double c, Vector a)
        {
            return new Vector(a.X * c, a.Y * c);
        }

        public static implicit operator Point(Vector vector)
        {
            return new Point((int) vector.X, (int) vector.Y);
        }

        public static implicit operator Size(Vector vector)
        {
            return new Size((int) vector.X, (int) vector.Y);
        }

        public static implicit operator Vector(Size size)
        {
            return new Vector(size);
        }

        public static implicit operator Vector(Point point)
        {
            return new Vector(point);
        }

        public static implicit operator Vector(Rectangle rectangle)
        {
            return new Vector(rectangle.X, rectangle.Y);
        }



        public static Vector Angle(double rad)
        {
            return new Vector(Math.Cos(rad), Math.Sin(rad));
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var v = obj as Vector;
            if (v == null)
            {
                return false;
            }
            return (Math.Abs(X - v.X) < 1E-14) && (Math.Abs(Y - v.Y) < 1E-14);
        }



        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }

    [TestFixture]
    public class Vector_Should
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
            Vector b = a/0;
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
        public void Vector_ReturnCorrectAngle()
        {
            var angle = Vector.Angle(Math.PI);
            var expectedAngle = new Vector(-1,0);
            angle.Should().Be(expectedAngle);
        }

        [Test]
        public void Vector_CastToPointTest()
        {
            var point = new Point(1,2);
            var vector = new Vector(1,2);
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