using System.Drawing;

namespace TagsCloudVisualization
{
    public class Vector
    {
        public int X;
        public int Y;

        public Vector(int x, int y)
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
            return new Vector(a.X+b.X, a.Y+b.Y);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }
        public static Vector operator /(Vector a, int c)
        {
            return new Vector(a.X / c, a.Y / c);
        }
        public static Vector operator *(Vector a, int c)
        {
            return new Vector(a.X * c, a.Y * c);
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public Size ToSize()
        {
            return new Size(X, Y);
        }
    }
}