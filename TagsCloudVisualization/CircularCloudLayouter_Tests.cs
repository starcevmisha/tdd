using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        [TestCase(-1, 3, TestName = "Negative X coordinate")]
        [TestCase(1, -3, TestName = "Negative Y coordinate")]
        public void TestConstructor_ThrowArgumentException(int x, int y)
        {
            var cloudCenter = new Point(x, y);
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(cloudCenter));
        }

        [TestCase(-10, 10, TestName = "Negative width")]
        [TestCase(10, -10, TestName = "Negative height")]
        [TestCase(0, 10, TestName = "Zero width")]
        [TestCase(10, 0, TestName = "Zero height")]
        public void PutNextRectangle_ThrowArgumentException_OnNonPositiveSize(int width, int height)
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => layout.PutNextRectangle(size));
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle_WithCorrectSize()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var rectangle = layout.PutNextRectangle(new Size(20, 25));
            rectangle.Size.ShouldBeEquivalentTo(new Size(20, 25));
        }

        private static readonly object[] RectanglesCases =
        {
            new object[] { new int[]{1, 2, 3, 4}, 2},
            new object[] { new int[]{ 1, 2, 3, 4, 4, 2 }, 3},
            new object[] { new int[]{ 1, 2, 3, 4, 4, 2, 5, 7, 10, 15, 1, 2 }, 6},
        };

        [Test, TestCaseSource("RectanglesCases")]
        public void PutNextRectangle_AddManyRectangles(int[] sizePairArray, int expectedResult)
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var rectangleList = new List<Rectangle>();
            for (int i = 0; i < sizePairArray.Length; i += 2)
                //CR(epeshk): объекты Size можно создавать в тесткейсах. Если запись с Size выглядит громоздкой - используй tuple из C# 7
                rectangleList.Add(layout.PutNextRectangle(new Size(sizePairArray[i], sizePairArray[i + 1])));
            rectangleList.Count.Should().Be(expectedResult);
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldBeOnCloudCenter()
        {
            var cloudCenter = new Point(100, 100);
            var layout = new CircularCloudLayouter(cloudCenter);
            var firstRectangle = layout.PutNextRectangle(new Size(10, 10));
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
        public void PutNextRectangle_ShouldReturnRectangleWithCorrectSize()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var size = new Size(20, 25);
            var firstRectangle = layout.PutNextRectangle(new Size(20, 25));
            var secondRectangle = layout.PutNextRectangle(new Size(20, 25));
            secondRectangle.Size.Should().Be(size);

        }


        [Test]
        public void PutNextRectangle_PairwiseIntersectionShouldBeFalse_OnBigNumberOfRectangles()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            var rectangleList = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
                rectangleList.Add(layout.PutNextRectangle(new Size(10, 10)));

            PairwiseIntersection(rectangleList).Should().BeFalse();
        }

        [Test, Timeout(1000)]
        public void AddALotOfRectangles()
        {
            var layout = new CircularCloudLayouter(new Point(10, 10));
            for (var i = 1; i < 500; i++)
                layout.PutNextRectangle(new Size(10, 10));
        }

        [Test]
        public void PutNextRectangle_AllRectanglesShouldBeInTheFormOfNotBigCircle()
        {
            //CR(epeshk): использовать константы. const int rectanglesCount = 100, ...
            // Общая площадь всех прямоугольников = 100*10*10 
            // Радиус круга с такой площадью есть sqrt(100*10*10/pi)
            // Так как Облако не плотное возьмем 2 таких радиуса
            var cloudCenter = new Point(50, 50);
            var layout = new CircularCloudLayouter(cloudCenter);
            var maxDistance = 0.0;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = layout.PutNextRectangle(new Size(10, 10));
                var distance = DistanceToCenter(rectangle, cloudCenter);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
            //CR(epeshk): уменьшить константу, т.к по условию "Облако должно быть плотным, чем плотнее, тем лучше."
            var expectedRadius = 2*Math.Sqrt(100 * 10 * 10 / Math.PI);
            maxDistance.Should().BeLessThan(expectedRadius);
        }

        public double DistanceToCenter(Rectangle rectangle, Point cloudCenter)
        {
            //CR(epeshk): сделать вычисления через класс Vector и extension методы к Rectangle
            return Math.Sqrt((rectangle.X + rectangle.Width / 2 - cloudCenter.X) *
                             (rectangle.X + rectangle.Width / 2 - cloudCenter.X) +
                             (rectangle.Y + rectangle.Height / 2 - cloudCenter.Y) *
                             (rectangle.Y + rectangle.Height / 2 - cloudCenter.Y));
        }

        private static bool PairwiseIntersection(List<Rectangle> rectangleList)
        {
            for (var i = 0; i < rectangleList.Count; i++)
                for (var j = i + 1; j < rectangleList.Count; j++)
                    if (rectangleList[j].IntersectsWith(rectangleList[i]))
                        return true;
            return false;
        }
    }
}
