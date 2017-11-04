using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Vector_Tests
    {
        private const double Precision = 1e14;
        [Test]
        public void Vector_AdditionTest()
        {
            var a = new Vector(2, 3);
            var b = new Vector(1, 7);
            var sum = a + b;
            var expected = new Vector(3, 10);
            sum.ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
        }

        [Test]
        public void Vector_SubtractionTest()
        {
            var a = new Vector(2, 3);
            var b = new Vector(1, 7);
            var diff = a - b;
            var expected = new Vector(1, -4);
            diff.ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
        }

        [Test]
        public void Vector_MultiplicationByAConstantTest()
        {
            var a = new Vector(2, 3);
            var product = 2 * a;
            var expected = new Vector(4, 6);
            product.ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
        }

        [Test]
        public void Vector_DivideByAConstantTest()
        {
            var a = new Vector(2, 4);
            var division = a / 2;
            var expected = new Vector(1, 2);
            division.ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
        }

        [Test]
        public void Vector_LengthTest()
        {
            var vector = new Vector(3, 4);
            var expected = 5;
            vector.Length().ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
        }
        [Test]
        public void Vector_ReturnCorrectAngle()
        {
            var angle = Vector.Angle(Math.PI);
            var expected = new Vector(-1, 0);
            angle.ShouldBeEquivalentTo(expected, options => options
                .WithStrictOrdering()
                .Using<double>(ctx => ctx.Subject.Should()
                    .BeApproximately(ctx.Expectation, Precision))
                .When(o => o.SelectedMemberPath == "X" ||
                           o.SelectedMemberPath == "Y"));
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