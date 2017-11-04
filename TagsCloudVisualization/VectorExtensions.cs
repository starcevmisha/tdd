using System;

namespace TagsCloudVisualization
{
    public static class VectorExtensions
    {
        public static bool Equals(this Vector vector, Object obj, double eps)
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
            return Math.Abs(vector.X - v.X) < eps && Math.Abs(vector.Y - v.Y) < eps;
        }
    }
}