using System;
using System.Collections.Generic;
using System.Linq;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Ломанная
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
                throw new Exception("Incorrect Polygon");
            }
        }

        #endregion

        #region Свойства

        public List<Point> Points
        {
            get { return points; }
        }

        #endregion

        // Список отрезков в точки
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

        // Список точек в отрезки
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

        // Получение позиции на ломанной в зависимости от времени на ее отрезках и пройденном времени
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

        // Получение позиции на ломанной в зависимости от времени на ее отрезках и пройденном времени + остаток ломанной
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

        public Point LastPoint()
        {
            return points.Count > 0 ? points[points.Count - 1] : null;
        }

        public Polyline toInt()
        {
            points.ForEach(x => x.toInt());
            return new Polyline(points);
        }
    }

    // Класс для возврата остатка ломанной и текущей точки нахождения на ней
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
