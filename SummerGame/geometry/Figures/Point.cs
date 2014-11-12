using System;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Точка
    /// </summary>
    public class Point : Figure
    {
        static double epsilon = 0.001;

        #region Свойства

        /// <summary>
        /// координаты точки
        /// </summary>

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

        #region Операторы

        public static bool operator !=(Point first, Point second)
        {
            if (Math.Abs(first.X - second.X) >= double.Epsilon && Math.Abs(first.Y - second.Y) >= double.Epsilon)
                return true;
            return false;
        }

        public static bool operator == (Point first, Point second)
        {
            if (Math.Abs(first.X - second.X) < double.Epsilon && Math.Abs(first.Y - second.Y) < double.Epsilon)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// Нахождение расстояния между двумя точками
        /// </summary>
        /// <returns>расстояние</returns>
        public static double Length(Point first, Point second)
        {
            return Math.Sqrt(Math.Pow(second.X - first.X ,2)+Math.Pow(second.Y - first.Y ,2));
        }

        /// <summary>
        /// Получает точку с целыми координатами
        /// </summary>
        public Point toInt()
        {
            return new Point((int)X, (int)Y);
        }

        ///<summary>
        ///Результат функции указывает, с какой стороны от AB лежит точка C
        ///Значение true означает, что точка лежит слева от отрезка или на отрезке
        ///false - справа от отрезка
        /// </summary>
        public static bool getDirect(Point A, Point B, Point C)
        {
            Vector vector1 = new Vector (B.X - A.X, B.Y - A.Y);
            Vector vector2 = new Vector (C.X - B.X, C.Y - B.Y);
            Vector leftNorm = Vector.LeftNormal(vector1);
            var first = leftNorm * vector2;
            if (first >= 0)
                return true;
            return false;
        }
    }
}
