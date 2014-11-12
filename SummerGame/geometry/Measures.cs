using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;


namespace Geometry
{
    public static class Measures
    {
        #region Расстояния

        /// <summary>
        /// Вычисляет расстояние между двумя точками
        /// </summary>
        /// <param name="a">первая точка</param>
        /// <param name="b">вторая точка</param>
        /// <returns>Расстояние</returns>
        public static double Distance(Point a, Point b)
        {
            var dx = a.X - b.X; var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Вычисляет расстояние между двумя точками
        /// </summary>
        /// <param name="ax">первая координата первой точки</param>
        /// <param name="ay">вторая координата первой точки</param>
        /// <param name="bx">первая координата второй точки</param>
        /// <param name="by">вторая координата второй точки</param>
        /// <returns>Расстояние</returns>
        public static double Distance( double ax, double ay, double bx, double by )
        {
            var dx = ax - bx; var dy = ay - by;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        #endregion

        #region Углы

        /// <summary>
        /// Конверсия из радиан в градусы
        /// </summary>
        /// <param name="radians">угол в радианах</param>
        /// <returns>угол в градусах</returns>
        public static double ToDegrees(double radians)
        {
            return 180 * radians / Math.PI;
        }

        /// <summary>
        /// Конверсия из градусов в радианы
        /// </summary>
        /// <param name="degrees">угол в градусах</param>
        /// <returns>угол в радианах</returns>
        public static double ToRadians(double degrees)
        {
            return Math.PI * degrees / 180;
        }

        /// <summary>
        /// Угол, отсчитанный от первого вектора ко второму против часовой стрелки
        /// </summary>
        /// <param name="v1">первый вектор</param>
        /// <param name="v2">второй вектор</param>
        /// <returns>угол в радианах из интервала (-pi, +pi]</returns>
        public static double AngleBetween(Vector v1, Vector v2)
        {
            Vector vn1 = v1.Normalise();
            Vector n1 = Vector.LeftNormal(vn1);
            Vector vn2 = v2.Normalise();
            double sc = vn1 * vn2;
            double ht = n1 * vn2;
            return (ht >= 0) ? Math.Acos(sc) : -Math.Acos(sc);
        }

        /// <summary>
        /// Угол, отсчитанный от орта оси X к данному вектору против часовой стрелки
        /// </summary>
        /// <param name="v"> вектор</param>
        /// <returns>угол в радианах из интервала (-pi, +pi]</returns>
        public static double AngleFromXOrto(Vector v)
        {
            Vector vn = v.Normalise();
            return (vn.Y >= 0) ? Math.Acos(vn.X) : -Math.Acos(vn.X);
        }

        #endregion
    }
}
