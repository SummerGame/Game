using System;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Точка
    /// </summary>
    public class Point : Figure
    {
        #region Свойства

        public double X { get; set; }

        public double Y { get; set; }

        #endregion

        #region Конструкторы

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        public static double Length(Point first, Point second)
        {
            return Math.Sqrt(Math.Pow(second.X - first.X ,2)+Math.Pow(second.Y - first.Y ,2));
        }

        public Point toInt()
        {
            return new Point((int)X, (int)Y);
        }
    }
}
