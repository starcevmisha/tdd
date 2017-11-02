//CR(epeshk): исправить ошибку в названии файла
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        //CR(epeshk): IntersectWithOtherRectangles -> IntersectWithAny?, List -> IEnumerable
        //CR(epeshk): добавить тесты на extension-методы, блок всё же tdd называется
        public static bool IntersectWithOtherRectangles(this Rectangle candidate, List<Rectangle> rectangleList)
        {
            return rectangleList.Any(rectangle => rectangle.IntersectsWith(candidate));
        }


        public static double DistanceTo(this Rectangle rectangle, Vector cloudCenter)
        {
            
            var distVector = (Vector) rectangle + (Vector) rectangle.Size / 2 - cloudCenter;
            //CR(epeshk): может быть сделать длину свойством вектора?
            return Math.Sqrt(distVector.X* distVector.X + distVector.Y * distVector.Y);
        }
    }
}