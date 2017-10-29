﻿using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    //CR(epeshk): internal
    //CR(epeshk): добавить тесты на операции
    public class Vector
    {
        //CR(epeshk): readonly
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
            return new Vector(a.X+b.X, a.Y+b.Y);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }
        //CR(epeshk): double c?
        public static Vector operator /(Vector a, int c)
        {
            return new Vector(a.X / c, a.Y / c);
        }
        public static Vector operator *( double c, Vector a)
        {
            return new Vector(a.X * c, a.Y * c);
        }

        public static implicit operator Point(Vector vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static implicit operator Size(Vector vector)
        {
            return new Size((int)vector.X, (int)vector.Y);
        }

//CR(epeshk): возможно, понадобятся ещё эти операторы
//        public static implicit operator Vector(Size size);
//        public static implicit operator Vector(Point size);

        public static Vector Angle(double rad)
        {
            return new Vector(Math.Cos(rad), Math.Sin(rad));
        }                     
    }
}