using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;

namespace Geometry.Figures
{
    public class Intersect
    {

        #region Функции поиска точек пересечения

        // Поиск пересечения отрезков
        //public static List<Point> GetIntersection(Segment first, Segment second)
        //{
        //    double a1, b1; // параметрические координаты первой прямой
        //    double a2, b2; // параметрические координаты второй прямой
        //    double x, y;// коодинаты пересечения двух прямых
        //    if ((first.Begin.X != first.End.X) && (second.Begin.X != second.End.X)) // проверка на совпадение коээфицентов при X
        //    {
        //        a1 = (first.Begin.Y - first.End.Y) / (first.Begin.X - first.End.X);
        //        b1 = first.Begin.Y - first.Begin.X * a1;
        //        a2 = (second.Begin.Y - second.End.Y) / (second.Begin.X - second.End.X);
        //        b2 = second.Begin.Y - second.Begin.X * a2;

        //        if (a1 != a2)
        //        {
        //            x = (b2 - b1) / (a1 - a2);
        //            y = a1 * x + b1;
        //            Point inter = new Point(x, y);
        //            if ((Intersect.IsIntersected(inter, first)) && (Intersect.IsIntersected(inter, second)))
        //            {
        //                return new List<Point> { inter };
        //            }
        //        }

        //    }
        //    return new List<Point>();
        //}



        //Поиск точки основания стрелочки
        public static Point GetArrowBase(Point begin, Point end)
        {
            Segment segment = new Segment(begin, end);
            Circle circle = new Circle(end, 1);
            var c = GetIntersection(segment, circle);
            return c[0];
        }
        // Поиск пересечения отрезков
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
        
        
        //public static List<Point> GetIntersection(Segment first, Segment second)
        //{
        //    Point vectorFirst = new Point(first.End.X - first.Begin.X, first.End.Y - first.Begin.Y);
        //    Point vectorSecond = new Point(second.End.X - second.Begin.X, second.End.Y - second.Begin.Y);
        //    var poList = new List<Point>();
        //    if (vectorFirst.X / vectorSecond.X == vectorFirst.Y / vectorSecond.Y)
        //    {
        //        if (Point.Length(first.Begin, first.End) > Point.Length(second.Begin, second.End))
        //        {
        //            if (IsIntersected(second.Begin, first)) poList.Add(second.Begin);
        //            if (IsIntersected(second.End, first)) poList.Add(second.End);
        //        }
        //        else
        //        {
        //            if (IsIntersected(first.Begin, second)) poList.Add(first.Begin);
        //            if (IsIntersected(first.End, second)) poList.Add(first.End);
        //        }
        //    }
        //    double p = 0, t = 0;
        //    double a1 = 0, a2 = 0, b1 = 0, b2 = 0, d1 = 0, d2 = 0;
        //    d1 = second.Begin.X - first.Begin.X;
        //    d2 = second.Begin.Y - first.Begin.Y;
        //    a1 = vectorFirst.X;
        //    a2 = vectorFirst.Y;
        //    b1 = vectorSecond.X;
        //    b2 = vectorSecond.Y;
        //    if (a1 == 0)
        //    {
        //        p = d1 / b1;
        //        t = (d2 - p * b2) / a2;
        //    }
        //    else
        //    {
        //    p = d2 - (d1 / a1) * a2;
        //    p = p / (b2 - (b1 * a2) / a1);

        //    t = (d1 - b1 * p) / a1;
            
        //    }
        //    p = -p;
        //    if ((IsIntersected(new Point(first.Begin.X + vectorFirst.X * t, first.Begin.Y + vectorFirst.Y * t), first))
        //        && (IsIntersected(new Point(second.Begin.X + vectorSecond.X * p, second.Begin.Y + vectorSecond.Y * p), second)))
        //    {
        //        Point pointInter = new Point(first.Begin.X + vectorFirst.X * t, first.Begin.Y + vectorFirst.Y * t);
        //        poList.Add(pointInter);
        //    }
        //    //if ((p <= 1) && (p >= 0) && (t <= 1) && (t >= 0))



        //    return poList;
        //}

        //public static List<Point> GetIntersection(Segment first, Segment second)
        //{
        //    //Point vectorFirst = new Point(first.End.X - first.Begin.X, first.End.Y - first.Begin.Y);
        //    //Point vectorSecond = new Point(second.End.X - second.Begin.X, second.End.Y - second.Begin.Y);
        //    Point firstNorm = LeftNormal(first);
        //    Point secondNorm = LeftNormal(second);
        //    List<Point> pointli = new List<Point>();


        //    if (((isLeft(first, firstNorm, second.Begin)) && (!(isLeft(first, firstNorm, second.End))))
        //        || (!(isLeft(first, firstNorm, second.Begin)) && (isLeft(first, firstNorm, second.End))))
        //    {
        //        if (((isLeft(second, secondNorm, first.Begin)) && (!(isLeft(second, secondNorm, first.End))))
        //            || (!(isLeft(second, secondNorm, first.Begin)) && (isLeft(second, secondNorm, first.End))))
        //        {
        //            Point vector = new Point(-firstNorm.X, -firstNorm.Y);
        //            pointli.Add();
        //        }
        //    }
        //}




        // Поиск пересечений Полигона и Ломанной
        public static List<Point> GetIntersection(ConvexPolygon polygon, Polyline polyline)
        {
            return Intersect.GetIntersection(polygon, polyline.Points.ToArray());
        }

        // Поиск точек пересечений Полигона с Полигоном
        public static List<Point> GetIntersection(ConvexPolygon fpolygon, ConvexPolygon spolygon)
        {
            var poList = new List<Point>();
            var fpolySegm = Polyline.PointsToSegments(fpolygon.Points);
            var spolySegm = Polyline.PointsToSegments(spolygon.Points);

            foreach (var fsegment in fpolySegm)
            {
                foreach (var ssegment in spolySegm)
                {
                    if ((GetIntersection(fsegment, ssegment)).Count>0) poList.Add(GetIntersection(fsegment, ssegment)[0]);
                }
                //for (int index = 0; index < polySegments.Count; index++)
                //{
                //    if ((!isLeft(polySegments[index], normals[index], segment.Begin)) &&
                //        (!isLeft(polySegments[index], normals[index], segment.End)))
                //    {
                //        index = polySegments.Count;
                //    }
                //    else
                //    {
                //        if (GetIntersection(segment, polySegments[index]).Count > 0)
                //            poList.Add((GetIntersection(segment, polySegments[index]))[0]);
                //    }
                //}
                //if (GetIntersection(segment, spolygon).Count > 0) poList.AddRange(GetIntersection(segment, spolygon));
                //if (poList.Count == 0)
                //{
                //    poList.Add(polySegm[0].Begin);
                //    poList.Add(polySegm.Last().End);
                //}
            }

            return poList;

        }

        // Поиск точек пересечений Полигона со списком точек (array)
        public static List<Point> GetIntersection(ConvexPolygon polygon, params Point[] arr)
        {
            int type = 0; // flag of ConvexPolygon type intersect (Polyline,Segment,Point)
            var points = new List<Point>();
            var segments = Polyline.PointsToSegments(polygon.Points);
            var normals = polygon.Normals;
            switch (arr.Count())
            {
                case 0: // Nothing
                    return new List<Point>();
                case 1: // Point
                    break;
                case 2: // Segment
                    type = 1;
                    break;
                default: // Polyline
                    type = 2;

                    var PolylineSegments = Polyline.PointsToSegments(arr.ToList());

                    foreach (var polylineSegment in PolylineSegments)
                    {
                        points.AddRange(Intersect.GetIntersection(polygon, polylineSegment.Begin, polylineSegment.End));
                    }
                    break;
            }

            for (int index = 0; index < segments.Count; index++)
            {
                var t = segments[index];

                if (type == 0)
                {
                    if (!isLeft(t, normals[index], arr[0])) return new List<Point>();
                }
                else if (type == 1)
                {
                    if (isLeft(t, normals[index], arr[0]) && !isLeft(t, normals[index], arr[1]) || !isLeft(t, normals[index], arr[0]) && isLeft(t, normals[index], arr[1]))
                    {
                        foreach (var segment in segments)
                        {
                                //if (Intersect.GetIntersection(new Segment(arr[0], arr[1]), segment).Count > 0) points.AddRange(Intersect.GetIntersection(new Segment(arr[0], arr[1]), segment));
                        
                            var newSegment = new Segment(arr[0], arr[1]);

                            if (GetIntersection(newSegment, segment).Count>0) points.AddRange(Intersect.GetIntersection(new Segment(arr[0], arr[1]), segment));
                            
                        }
                        break;
                    }
                    else if (!isLeft(t, normals[index], arr[0]) && !isLeft(t, normals[index], arr[1]))
                    {
                        return new List<Point>();
                    }
                }
            }

            if (type == 0)
                points.Add(arr[0]);
            if (type == 1)
                points.AddRange(new List<Point> { arr[0], arr[1] });

            return points;
        }

        //public static List<Point> GetIntersection(ConvexPolygon polygon, Segment segment)
        //{
            
        //}

        // Пересечение отрезка и круга
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

        #endregion

        #region Функции установления пересечения

        // Пересечение двух отрезков
        public static bool IsIntersected(Segment first, Segment second)
        {
            return GetIntersection(first, second).Count > 0;
        }

        // Установление пересечения Полигона и Ломанной
        public static bool IsIntersected(ConvexPolygon polygon, Polyline polyline)
        {
            return GetIntersection(polygon, polyline).Count > 0;
        }

        // Установление пересечения Полигона со списком точек (array)
        public static bool IsIntersected(ConvexPolygon polygon, params Point[] arr)
        {
            return GetIntersection(polygon, arr).Count > 0;
        }

        // Пересечение двух точек
        public static bool IsIntersected(Point first, Point second)
        {
            return ((first.X == second.X) && (first.Y == second.Y));
        }

        // Пересечение точки и отрезка
        public static bool IsIntersected(Point cur, Segment line)
        {
            var begin = line.Begin;
            var end = line.End;
            var v = new Point(end.X - begin.X, end.Y - begin.Y);
            var t = new Point(0, 0);
            if (v.X != 0) 
            {
                if (v.Y !=0)
                {
                    t = new Point(((cur.X - begin.X)/v.X), ((cur.Y - begin.Y)/v.Y));
                    
                }
                else t = new Point(((cur.X - begin.X) / v.X), 0);
            }
            else
            {
                if (v.Y != 0)
                {
                    t = new Point(0, ((cur.Y - begin.Y)/v.Y));
                }
                else
                {
                    return false;
                }
            }
            return t.Y>=0 && t.Y<=1 && t.X <= 1 && t.X >= 0;
            
        }

        // Пересечение точки и круга
        public static bool IsIntersected(Point point, Circle circle)
        {
            if (Point.Length(point, circle.Center) <= circle.Radius) return true;
            else return false;
        }

        public static bool IsIntersected(Point Point,ConvexPolygon polygon)
        {
            var segments = Polyline.PointsToSegments(polygon.Points);
            var normals = polygon.Normals;
            for (int index = 0; index < segments.Count; index++)
            {
                if (!isLeft(segments[index], normals[index], Point)) return false;
            }
            return true;
        }

        public static List<Point> GetIntersection(Segment segment, ConvexPolygon polygon)
        {
            var poList = new List<Point>();
            var polySegments = Polyline.PointsToSegments(polygon.Points);
            var normals = polygon.Normals;

                for (int index = 0; index < polySegments.Count; index++)
            {
                if ((!isLeft(polySegments[index], normals[index], segment.Begin))&&(!isLeft(polySegments[index], normals[index], segment.End)))
                return poList;
                else
                {
                    if (GetIntersection(segment, polySegments[index]).Count > 0) poList.Add((GetIntersection(segment, polySegments[index]))[0]);
                }
            }
            //if (poList.Count==0)
            //{
            //    poList.Add(segment.Begin);
            //    poList.Add(segment.End);
            //}
            if (IsIntersected(segment.Begin, polygon)) poList.Add(segment.Begin);
            if (IsIntersected(segment.End, polygon)) poList.Add(segment.End); ;

            return poList;
        }

        public static List<Point> GetIntersection(Polyline poly,ConvexPolygon polygon)
        {
            var poList = new List<Point>();
            var polySegm = Polyline.PointsToSegments(poly.Points);
            polySegm = polySegm.GetRange(0, polySegm.Count - 1);
            foreach (var segment in polySegm)
            {
                //for (int index = 0; index < polySegments.Count; index++)
                //{
                //    if ((!isLeft(polySegments[index], normals[index], segment.Begin)) &&
                //        (!isLeft(polySegments[index], normals[index], segment.End)))
                //    {
                //        index = polySegments.Count;
                //    }
                //    else
                //    {
                //        if (GetIntersection(segment, polySegments[index]).Count > 0)
                //            poList.Add((GetIntersection(segment, polySegments[index]))[0]);
                //    }
                //}
                if (GetIntersection(segment,polygon).Count>0) poList.AddRange(GetIntersection(segment,polygon));
                //if (poList.Count == 0)
                //{
                //    poList.Add(polySegm[0].Begin);
                //    poList.Add(polySegm.Last().End);
                //}
            }
            
            return poList;
        }

        // Пересечение круга и круга
        public static bool IsIntersected(Circle circlef, Circle circlel)
        {
            if (Point.Length(circlef.Center, circlel.Center) <= circlef.Radius + circlel.Radius)
                return true;
            else return false;

        }

        #endregion

        #region Вспомогательные функции


        // Находится ли точка слева по напр. левой нормали (для intersect(ConvexPolygon polygon, params Point[] arr) )
        private static bool isLeft(Segment segment, Point normal, Point point)
        {
            var vector = new Point(point.X - segment.Begin.X, point.Y - segment.Begin.Y);
            var scalar = vector.X * normal.X + vector.Y * normal.Y;
            return scalar >= 0;
        }

        private static Point LeftNormal(Segment segment)
        {
            double x = segment.End.X - segment.Begin.X;
            double y = segment.End.Y - segment.Begin.Y;
            return new Point(-y, x);
        }

        #endregion

        // Функция для поиска пересечений ломанной с фигурами
        // Общего вида для пути
        public static List<Point> GetIntersection(Figure figure, Segment segment)
        {
            if (figure is Circle)
            {
                return GetIntersection(figure as Circle, segment);
            }
            else if (figure is ConvexPolygon)
            {
                return GetIntersection(figure as ConvexPolygon, segment);
            }

            return new List<Point>();
        }

        // Функция для проверки наличия пересечения точки с фигурами
        public static bool IsIntersected(Point point, Figure figure)
        {
            if (figure is Circle) return IsIntersected(point, figure as Circle);
            else if (figure is ConvexPolygon) return IsIntersected(point, figure as ConvexPolygon);
            else if (figure is Point) return IsIntersected(point, figure as Point);
            else if (figure is Polyline) return IsIntersected(point, figure as Polyline);
            else if (figure is Segment) return IsIntersected(point, figure as Segment);
            else return false;
        }

        public static List<Point> IsIntersected(ConvexPolygon curPolygon, ConvexPolygon movPolygon)
        {
            return GetIntersection(curPolygon, movPolygon);
        }
    }
}
