using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Выпуклый Многоугольник. Для полигона установлены соглашение о заполенении полигона ПРОТИВ ЧАСОВОЙ стрелки.
    /// </summary>
    public class ConvexPolygon : Figure
    {
        #region Поля

        private List<Point> points;

        private List<Point> normals;

        #endregion

        #region Свойства

        public List<Point> Normals
        {
            get { return normals; }
        }

        public List<Point> Points
        {
            get { return points; }
        }

        #endregion

        #region Конструкторы

        public ConvexPolygon(List<Point> newPoints)
        {
            var set = Polyline.PointsToSegments(newPoints);
            if (convex(set))
            {
                this.points = newPoints;
                normals = new List<Point>(/*set.Count*/);
                foreach (var segment in set)
                {
                    normals.Add(GetLeftNormal(segment));
                }
            }
            else
            {
                throw new Exception("Incorrect Polygon");
            }
        }

        public ConvexPolygon(params double[] coords)
        {
            var list = coords.ToList();
            if (list.Count > 0 && list.Count % 2 == 0)
            {
                var newPoints = new List<Point>();
                for (int i = 0; i < list.Count; i += 2)
                {
                    newPoints.Add(new Point(list[i], list[i + 1]));
                }
                var set = Polyline.PointsToSegments(newPoints);
                if (convex(set))
                {
                    this.points = newPoints;
                    normals = new List<Point>(/*set.Count*/);
                    foreach (var segment in set)
                    {
                        normals.Add(GetLeftNormal(segment));
                    }
                }
                else
                {
                    throw new Exception("Incorrect Polygon");
                }
            }
        }

        public ConvexPolygon(params Point[] point)
        {
            if (point.Count() > 0)
            {
                var newPoints = point.ToList();
                var set = Polyline.PointsToSegments(newPoints);
                if (convex(set))
                {
                    this.points = newPoints;
                    normals = new List<Point>(/*set.Count*/);
                    foreach (var segment in set)
                    {
                        normals.Add(GetLeftNormal(segment));
                    }
                }
                else
                {
                    throw new Exception("Incorrect Polygon");
                }
            }
        }

        public ConvexPolygon(List<Point> newPoints, List<Point> normals)
        {
            var set = Polyline.PointsToSegments(newPoints);
            if (convex(set))
            {
                this.points = newPoints;
                this.normals = normals;
            }
            else
            {
                throw new Exception("Incorrect Polygon");
            }
        }

        public ConvexPolygon(Point point, double edge)
        {
            //var Mainpoints = new List<Point>();
            var points = new List<Point>();
            points.Add(point);
            points.Add(new Point(point.X + edge, point.Y));
            points.Add(new Point(point.X + edge, point.Y + edge));
            points.Add(new Point(point.X, point.Y + edge));

            var set = Polyline.PointsToSegments(points);
            normals = new List<Point>(/*set.Count*/);
            foreach (var segment in set)
            {
                normals.Add(GetLeftNormal(segment));
            }

        }

        #endregion

        private bool convex(List<Segment> set)
        {
            //for (int i = 0; i < set.Count; i++)
            //{
            //    for (int j = i + 1; j < set.Count; j++)
            //    {
            //        if (Intersect.IsIntersected(set[i], set[j]))
            //            return false;
            //    }
            //}
            return true;
        }

        public ConvexPolygon toInt()
        {
            points.ForEach(x => x.toInt());
            return new ConvexPolygon(points);
        }

        private Point GetLeftNormal(Segment segment)
        {
            double x = segment.End.X - segment.Begin.X;
            double y = segment.End.Y - segment.Begin.Y;
            return new Point(-y,x); // TODO!!!!!
        }

        public Point Center()
        {
            var point = new Point(0, 0);
            foreach (var point1 in Points)
            {
                point.X += point1.X;
                point.Y += point1.Y;
            }
            point.X /= Points.Count;
            point.Y /= Points.Count;
            return point;
        }



    }
}
