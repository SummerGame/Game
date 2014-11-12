using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Resources;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Ломаная
    /// </summary>
    public class Polyline : Figure
    {
        #region Поля

        private List<Point> points;

        #endregion

        #region Конструкторы

        public Polyline(List<Point> points)
        {
            this.points = points;
        }

        public Polyline()
        {
            this.points = new List<Point>();
        }

        public Polyline(List<Segment> Segments)
        {
            points = SegmentsToPoints(Segments);
        }

        public Polyline(params double[] coords)
        {
            var list = coords.ToList();
            if (list.Count > 0 && list.Count % 2 == 0)
            {
                var newPoints = new List<Point>();
                for (int i = 0; i < list.Count; i += 2)
                {
                    newPoints.Add(new Point(list[i], list[i + 1]));
                }
                points = newPoints;
            }
            else
            {
                throw new Exception(Messages.IncorrectLand);
            }
        }

        #endregion

        #region Свойства

        public List<Point> Points
        {
            get { return points; }
        }

        #endregion

        ///<summary>
        /// Добавление новой точки в список 
        /// </summary>
        // TODO: найти другой способ(если он есть) REMOVE THAT!!!
        public void AddPoint(Point point)
        {
            this.points.Add(point);
        }

        ///<summary>
        /// Список отрезков в точки
        /// </summary>  
        public static List<Point> SegmentsToPoints(List<Segment> Segments)
        {
            var points = new List<Point>();
            foreach (var Segment in Segments)
            {
                if (!points.Contains(Segment.Begin))
                    points.Add(Segment.Begin);
                if (!points.Contains(Segment.End))
                    points.Add(Segment.End);
            }
            return points;
        }

        ///<summary>
        /// Список точек в отрезки
        /// Последняя и первая точки также образуют отрезок
        /// </summary>         
        public static List<Segment> PointsToSegments(List<Point> points)
        {
            var Segments = new List<Segment>();
            if (points.Count >= 2)
            {
                for (int index = 0; index < points.Count; index++)
                {
                    var fst = points[index];
                    Point snd;
                    if (index + 1 < points.Count)
                    {
                        snd = points[index + 1];
                    }
                    else
                    {
                        snd = points[0];
                    }
                    Segments.Add(new Segment(fst, snd));
                }
            }
            return Segments;
        }

        ///<summary>
        /// Получение позиции на ломанной в зависимости от времени на ее отрезках и пройденном времени
        /// </summary>          
        public Point Position(double time, List<double> durations)
        {
            var curTime = 0;
            var _time = 0.0;
            foreach (var duration in durations)
            {
                _time += duration;
                if (_time < time)
                {
                    curTime++;
                }
                else
                {
                    var subTime = 0.0;
                    for (int i = 0; i <= curTime; i++)
                    {
                        subTime += durations[i];
                    }
                    subTime = time - subTime;
                    return PointsToSegments(points)[curTime].Position(subTime, durations[curTime]);
                }
            }
            return null;
        }

        ///<summary>
        /// Получение позиции на ломанной в зависимости от времени на ее отрезках и пройденном времени + остаток ломанной
        /// </summary>         
        public LastPolyAndPosition PositionAndLastPolyline(double time, List<double> durations)
        {
            var curTime = 0;
            var _time = 0.0;
            foreach (var duration in durations)
            {
                _time += duration;
                if (_time < time)
                {
                    curTime++;
                }
                else
                {
                    var subTime = 0.0;
                    for (int i = 0; i <= curTime; i++)
                    {
                        subTime += durations[i];
                    }
                    subTime = time - subTime;

                    var lastPoint = PointsToSegments(points)[curTime].Position(subTime, durations[curTime]);
                    var lastPoints = new List<Point> {lastPoint};
                    for (int index = curTime+1; index < points.Count; index++)
                    {
                        var point = points[index];
                        lastPoints.Add(point);
                    }

                    var lastPoly = new Polyline(lastPoints);
                    return new LastPolyAndPosition(lastPoly, lastPoint);
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает последнюю точку из списка или null, если список пуст
        /// </summary>
        /// <returns></returns>
        public Point LastPoint()
        {
            return points.Count > 0 ? points[points.Count - 1] : null;
        }

        /// <summary>
        /// Преобразует координаты точек ломаной в целые координаты
        /// </summary>
        /// <returns>Ломаную со списком точек с целыми координатами</returns>
        public Polyline toInt()
        {
            points.ForEach(x => x.toInt());
            return new Polyline(points);
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

        ///<summary>
        /// Проверка самопересечения ломаной
        /// </summary>          
        public static bool CheckSelfIntersection(List<Segment> segments)
        {
            int start_j = 1;
         
            for (int i = 0; i < segments.Count-1; i++)
            {
                for (int j = start_j; j < segments.Count; j++)
                {
                    if (i != j && i != j - 1 && j != i - 1)
                    {
                        if (Intersect.IsLinePartsIntersected(segments[i].Begin,segments[i].End,segments[j].Begin,segments[j].End))
                       // if (Intersect.IsIntersected(segments[i], segments[j]))
                        { return true; }
                    }
                }
                start_j++;
            }
            return false;
        }
    }

    ///<summary>
    /// Класс для возврата остатка ломанной и текущей точки нахождения на ней
    /// </summary>      
    public class LastPolyAndPosition
    {
        private Polyline last;
        private Point position;

        public LastPolyAndPosition(Polyline polyline, Point pos)
        {
            Polyline = polyline;
            Position = pos;
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Polyline Polyline
        {
            get { return last; }
            set { last = value; }
        }
    }
}
