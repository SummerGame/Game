using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс, содержащий методы для нахождения или проверки пересечений между фигурами 
    /// </summary>
    public class Intersect
    {

        #region Функции поиска точек пересечения

        ///<summary>
        /// Поиск пересечения отрезков
        ///</summary>        
        public static List<Point> GetIntersection(Segment first, Segment second)
        {
            Point vectorFirst = new Point(first.End.X - first.Begin.X, first.End.Y - first.Begin.Y);
            Point vectorSecond = new Point(second.End.X - second.Begin.X, second.End.Y - second.Begin.Y);
            var poList = new List<Point>();
            if (vectorFirst.X / vectorSecond.X == vectorFirst.Y / vectorSecond.Y)
            {
                if (Point.Length(first.Begin, first.End) > Point.Length(second.Begin, second.End))
                {
                    if (IsIntersected(second.Begin, first)) poList.Add(second.Begin);
                    if (IsIntersected(second.End, first)) poList.Add(second.End);
                }
                else
                {
                    if (IsIntersected(first.Begin, second)) poList.Add(first.Begin);
                    if (IsIntersected(first.End, second)) poList.Add(first.End);
                }
            }
            double p = 0, t = 0;
            double a1 = 0, a2 = 0, b1 = 0, b2 = 0, d1 = 0, d2 = 0;
            //Point vectorFirst = new Point(first.End.X - first.Begin.X, first.End.Y - first.Begin.Y);
            //Point vectorSecond = new Point(second.End.X - second.Begin.X, second.End.Y - second.Begin.Y);
            //var poList = new List<Point>();
            d1 = second.Begin.X - first.Begin.X;
            d2 = second.Begin.Y - first.Begin.Y;
            a1 = vectorFirst.X;
            a2 = vectorFirst.Y;
            b1 = vectorSecond.X;
            b2 = vectorSecond.Y;
            if (a1 == 0)
            {
                p = d1 / b1;
                t = (d2 - p * b2) / a2;
            }
            else
            {
                p = d2 - (d1 / a1) * a2;
                p = p / (b2 - (b1 * a2) / a1);

                t = (d1 - b1 * p) / a1;

            }
            p = -p;
            if ((IsIntersected(new Point(first.Begin.X + vectorFirst.X * t, first.Begin.Y + vectorFirst.Y * t), first))
                && (IsIntersected(new Point(second.Begin.X + vectorSecond.X * p, second.Begin.Y + vectorSecond.Y * p), second)))
            {
                Point pointInter = new Point(first.Begin.X + vectorFirst.X * t, first.Begin.Y + vectorFirst.Y * t);
                poList.Add(pointInter);
            }
            //if ((p <= 1) && (p >= 0) && (t <= 1) && (t >= 0))

            return poList;
        }

        /// <summary>
        /// Проверка пересечения двух отрезков
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsLinePartsIntersected(Point a, Point b, Point c, Point d)
        {
            double common = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if (common == 0) return false;

            double rH = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            double sH = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);

            double r = rH / common;
            double s = sH / common;

            if (r >= 0 && r <= 1 && s >= 0 && s <= 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Поиск точки основания стрелочки 
        /// Берется на расстонии 1
        /// </summary>
        /// <param name="begin">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        /// <returns>точка отрезка, лежащая на основании треугольника</returns>
        public static Point GetArrowBase(Point begin, Point end)
        {
            Segment segment = new Segment(begin,end);
            Circle circle = new Circle(end,1);
            var c = GetIntersection(segment, circle);
            return c[0];
        }
        ///<summary>
        /// Поиск пересечений Полигона и Ломаной
        /// </summary>        
        public static List<Point> GetIntersection(ConvexPolygon polygon, Polyline polyline)
        {
            return Intersect.GetIntersection(polygon, polyline.Points.ToArray());
        }

        ///<summary>
        /// Поиск точек пересечений Полигона с Полигоном
        /// </summary>
        public static List<Point> GetIntersection(ConvexPolygon fpolygon, ConvexPolygon spolygon)
        {
            var poList = new List<Point>();
            var fpolySegm = Polyline.PointsToSegments(fpolygon.Points);
            var spolySegm = Polyline.PointsToSegments(spolygon.Points);

            foreach (var fsegment in fpolySegm)
            {
                foreach (var ssegment in spolySegm)
                {
                    if ((GetIntersection(fsegment, ssegment)).Count > 0) poList.Add(GetIntersection(fsegment, ssegment)[0]);
                }
            }

            return poList;

        }

        ///<summary>
        /// Поиск точек пересечений Полигона со списком точек (array)
        ///</summary>
        public static List<Point> GetIntersection(ConvexPolygon polygon, params Point[] arr)
        {
            switch (arr.Count())
            {
                case 0: // Nothing
                    return new List<Point>();
                case 1: // Point
                    if (IsIntersected(arr[0], polygon))
                    {
                        return new List<Point> { arr[0] };
                    }
                    else
                    {
                        return new List<Point>();
                    }
                case 2: // Segment
                    return GetIntersection(Polyline.PointsToSegments(arr.ToList())[0], polygon);
                default: // Polyline
                    return GetIntersection(new Polyline(arr.ToList()), polygon);

            }

        }
        
        ///<summary>
        /// Пересечение отрезка и круга
        /// </summary>
        public static List<Point> GetIntersection(Segment segment, Circle circle)
        {
            double a, b, t, diskr;
            Point vector = new Point(segment.End.X - segment.Begin.X, segment.End.Y - segment.Begin.Y);
            t = Math.Pow(circle.Radius, 2) - Math.Pow(circle.Center.X, 2) - Math.Pow(circle.Center.Y, 2);
            t = t - segment.Begin.X * segment.Begin.X - segment.Begin.Y * segment.Begin.Y + 2 * vector.X * circle.Center.X + 2 * vector.Y * circle.Center.Y;
            a = vector.X + vector.Y;
            b = segment.Begin.X * vector.X + segment.Begin.Y * vector.Y - vector.X * circle.Center.X -
                vector.Y * circle.Center.Y;
            diskr = b * b - 4 * a * t;
            if (diskr < 0) return new List<Point>();
            else if (diskr == 0)
            {
                double k;// Коэффицент при векторе
                k = -(b / (2 * a));
                if ((k > 0) || (k < 1)) return new List<Point> { new Point(segment.Begin.X + vector.X * k, segment.Begin.Y + vector.Y * k) };
                else return new List<Point>();
            }
            else
            {
                double k1, k2;
                k1 = (-b - diskr) / (2 * a); k2 = (diskr - b) / (2 * a);
                Point point1 = new Point(segment.Begin.X + k1 * vector.X, segment.Begin.Y + k1 * vector.Y);
                Point point2 = new Point(segment.Begin.X + k2 * vector.X, segment.Begin.Y + k2 * vector.Y);
                List<Point> spisk = new List<Point>();
                if ((k1 > 0) || (k1 < 1)) spisk.Add(point1);
                if ((k2 > 0) || (k2 < 1)) spisk.Add(point2);
                return spisk;
            }
        }

        /// <summary>
        /// Проверка пересечения отрезка и многоугольника
        /// </summary>
        /// <returns>Возвращает список точек пересечения</returns>
        public static List<Point> GetIntersection(Segment segment, ConvexPolygon polygon)
        {
            var poList = new List<Point>();
            var polySegments = Polyline.PointsToSegments(polygon.Points);
            var normals = polygon.Normals;

            for (int index = 0; index < polySegments.Count; index++)
            {
                 if ((!isLeft(polySegments[index], normals[index], segment.Begin)) && (!isLeft(polySegments[index], normals[index], segment.End)))
                    return poList;
                else
                {
                    if (GetIntersection(segment, polySegments[index]).Count > 0) poList.Add((GetIntersection(segment, polySegments[index]))[0]);
                }
            }
            if (IsIntersected(segment.Begin, polygon)) poList.Add(segment.Begin);
            if (IsIntersected(segment.End, polygon)) poList.Add(segment.End);
            for (int j = 0; j < poList.Count; j++)
            {
                for (int k = j + 1; k < poList.Count; k++)
                {
                    if ((poList[j].X == poList[k].X) && (poList[j].Y == poList[k].Y)) poList.Remove(poList[j]);
                }
            }

            return poList;
        }

        /// <summary>
        /// Проверка пересечения ломаной и многоугольника
        /// </summary>
        /// <returns>Возвращает список точек пересечения</returns>
        public static List<Point> GetIntersection(Polyline poly, ConvexPolygon polygon)
        {
            var poList = new List<Point>();
            var polySegm = Polyline.PointsToSegments(poly.Points);
            foreach (var segment in polySegm)
            {
                if (GetIntersection(segment, polygon).Count > 0) poList.AddRange(GetIntersection(segment, polygon));
            }

            return poList;
        }
        
        #endregion

        #region Функции установления пересечения

        ///<summary>
        ///Проверка пересечения двух отрезков
        /// </summary>
        /// <returns>Возвращает true, если отрезки пересекаются</returns>
        public static bool IsIntersected(Segment first, Segment second)
        {
            return GetIntersection(first, second).Count > 0;
        }

        ///<summary>
        /// Установление пересечения Полигона и Ломанной
        /// </summary>    
        public static bool IsIntersected(ConvexPolygon polygon, Polyline polyline)
        {
            return GetIntersection(polygon, polyline).Count > 0;
        }

        ///<summary>
        /// Установление пересечения Полигона со списком точек (array)
        /// </summary>  
        
        public static bool IsIntersected(ConvexPolygon polygon, params Point[] arr)
        {
            return GetIntersection(polygon, arr).Count > 0;
        }

        ///<summary>
        /// Равенство точек
        /// </summary>  
        public static bool IsIntersected(Point first, Point second)
        {
            return ((first.X == second.X) && (first.Y == second.Y));
        }

        /// <summary>
        /// Проверка принадлежности точки отрезку
        /// </summary>
        public static bool IsIntersected(Point cur, Segment line)
        {
            var begin = line.Begin;
            var end = line.End;
            var v = new Point(end.X - begin.X, end.Y - begin.Y);

            if (Point.Length(line.Begin, cur) + Point.Length(line.End, cur) == Point.Length(line.Begin, line.End))
                return true;
            return false;

            //var t = new Point(0, 0); // !!
            //if (v.X != 0)
            //{
            //    if (v.Y != 0)
            //    {
            //        t = new Point(((cur.X - begin.X) / v.X), ((cur.Y - begin.Y) / v.Y));

            //    }
            //    else t = new Point(((cur.X - begin.X) / v.X), 0);
            //}
            //else
            //{
            //    if (v.Y != 0)
            //    {
            //        t = new Point(0, ((cur.Y - begin.Y) / v.Y));
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return t.Y >= 0 && t.Y <= 1 && t.X <= 1 && t.X >= 0;

        }

        // Пересечение точки и круга
        public static bool IsIntersected(Point point, Circle circle)
        {
            if (Point.Length(point, circle.Center) <= circle.Radius) return true;
            else return false;
        }

        // Пересечение точки и полигона
        public static bool IsIntersected(Point Point, ConvexPolygon polygon)
        {
            var segments = Polyline.PointsToSegments(polygon.Points);
            var normals = polygon.Normals;
            for (int index = 0; index < segments.Count; index++)
            {
                if (!isLeft(segments[index], normals[index], Point)) return false;
            }
            return true;
        }

        // Пересечение круга и круга
        public static bool IsIntersected(Circle circlef, Circle circlel)
        {
            if (Point.Length(circlef.Center, circlel.Center) <= circlef.Radius + circlel.Radius)
                return true;
            else return false;
        }

        // Пересечение отрезка и полигона
        private static bool IsIntersected(Segment segment, ConvexPolygon polygon)
        {
            return GetIntersection(segment, polygon).Count > 0;
        }

        // Пересечение отрезка и круга
        private static bool IsIntersected(Segment segment, Circle circle)
        {
            return GetIntersection(segment, circle).Count > 0;
        }

        // Пересечение полигона и круга
        public static bool IsIntersected(ConvexPolygon polygon, Circle circle)
        {
            var segments = Polyline.PointsToSegments(polygon.Points);
            foreach (var segment in segments)
            {
                if (IsIntersected(segment, circle)) return true;
            }
            return false;
        }

        #endregion

        #region Вспомогательные функции


        /// <summary>
        /// Находится ли точка слева по напр. левой нормали (для intersect(ConvexPolygon polygon, params Point[] arr) )
        /// </summary>
        private static bool isLeft(Segment segment, Point normal, Point point)
        {
            var vector = new Point(point.X - segment.Begin.X, point.Y - segment.Begin.Y);
            var scalar = vector.X * normal.X + vector.Y * normal.Y;
            return scalar >= 0;
        }

        /// <summary>
        /// Вычисление координат левой нормали для данного отрезка
        /// </summary>
        private static Point LeftNormal(Segment segment)
        {
            double x = segment.End.X - segment.Begin.X;
            double y = segment.End.Y - segment.Begin.Y;
            return new Point(-y, x);
        }

        #endregion
       
        
        /// <summary>
        /// Функция для поиска пересечений ломанной с фигурами
        /// Общего вида для пути
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static List<Point> GetIntersection(Figure figure, Segment segment)
        {
            if (figure is Circle)
            {
                return GetIntersection(segment, (Circle) figure);
            }
            else if (figure is ConvexPolygon)
            {
                return GetIntersection(segment,(ConvexPolygon)figure);
            }

            return new List<Point>();
        }

        /// <summary>
        ///  Функция для проверки наличия пересечения точки с фигурами
        /// </summary>
        public static bool IsIntersected(Point point, Figure figure)
        {
            if (figure is Circle) return IsIntersected(point, figure as Circle);
            else if (figure is ConvexPolygon) return IsIntersected(point, figure as ConvexPolygon);
            else if (figure is Point) return IsIntersected(point, figure as Point);
            else if (figure is Polyline) return IsIntersected(point, figure as Polyline);
            else if (figure is Segment) return IsIntersected(point, figure as Segment);
            else return false;
        }

        /// <summary>
        /// Функция для проверки пересечения двух полигонов
        /// </summary>
        public static bool IsIntersected(ConvexPolygon curPolygon, ConvexPolygon movPolygon)
        {
            return GetIntersection(curPolygon, movPolygon).Count > 0;
        }

        /// <summary>
        /// Функциядля проверки пересечения полигона и фигуры
        /// </summary>
        public static bool IsIntersected(ConvexPolygon polygon, Figure figure)
        {
            if (figure is Circle) return IsIntersected(polygon, figure as Circle);
            else if (figure is ConvexPolygon) return IsIntersected(polygon, figure as ConvexPolygon);
            else if (figure is Point) return IsIntersected(polygon, figure as Point);
            else if (figure is Polyline) return IsIntersected(polygon, figure as Polyline);
            else if (figure is Segment) return IsIntersected( figure as Segment , polygon);
            else return false;
        }

    }
}
