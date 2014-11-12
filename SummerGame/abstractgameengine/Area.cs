using System.Collections.Generic;
using System.Linq;
using Geometry;
using Geometry.Figures;

namespace AbstractGameEngine
{
    /// <summary>
    /// Класс "Зона"
    /// Представляет собой Полигон, который содержит в себе набор геометрических фигур.
    /// Основной полигон охватывает все фигуры объекта Area.
    /// </summary>
    public class Area
    {
        #region Fields
        /// <summary>
        /// Многоугольник, который покрывает все фигуры, определяющие данной облости.
        /// </summary>
        private ConvexPolygon poly;

        /// <summary>
        /// Список фигур, которые определяют данную облость.
        /// </summary>
        protected List<Figure> figures = new List<Figure>();

        #endregion

        #region Properties
        /// <summary>
        /// Свойство возвращает текущую позицию
        /// При установлении позиции параллельно переносит все точки полигона
        /// </summary>
        public Point Position
        {
            get { return poly.Center(); }
            set { poly = SetPoly(value); }
        }

        /// <summary>
        /// Свойство возвращает покрывающий облость многоугольник.
        /// При установлении позиции параллельно переносит все точки полигона
        /// </summary>
        public ConvexPolygon Polygon
        {
            get { return poly; }
            protected set { poly = value; }
        }

        /// <summary>
        /// Получение списка фигур, из которых состоит данная облость.
        /// </summary>
        public List<Figure> Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        #endregion

        #region Constructors

        protected Area()
        {
        }

        public Area(ConvexPolygon polygon)
        {
            poly = polygon;
        }

        public Area(List<object> fig)
        {
            var figs = fig.ConvertAll(x => (Figure) x);
            figures = figs;
            var points = new List<Point>();
            foreach (var figure in figs)
            {
                if (figure is Point)
                    points.Add((Point)figure);
                else if (figure is ConvexPolygon)
                    points.AddRange(((ConvexPolygon)figure).Points);
                else if (figure is Circle)
                {
                    points.Add(new Point(((Circle)figure).Center.X + ((Circle)figure).Radius, ((Circle)figure).Center.Y));
                    points.Add(new Point(((Circle)figure).Center.X, ((Circle)figure).Center.Y + ((Circle)figure).Radius));
                    points.Add(new Point(((Circle)figure).Center.X - ((Circle)figure).Radius, ((Circle)figure).Center.Y));
                    points.Add(new Point(((Circle)figure).Center.X, ((Circle)figure).Center.Y - ((Circle)figure).Radius));

                }
            }
            double minX = points[0].X, maxX = points[0].X, minY = points[0].Y, maxY = points[0].Y;
            foreach (var point in points)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }
            var listpo = new List<Point>()
                             {
                                 new Point(minX, maxY),
                                 new Point(minX, minY),
                                 new Point(maxX, minY),
                                 new Point(maxX, maxY)
                             };
            poly = new ConvexPolygon(listpo);
        }

        public Area(List<Figure> figs)
        {
            figures = figs;
            var points = new List<Point>();
            if (figs.Count > 0)
            {
                foreach (var figure in figs)
                {
                    if (figure is Point)
                        points.Add((Point) figure);
                    else if (figure is ConvexPolygon)
                        points.AddRange(((ConvexPolygon) figure).Points);
                    else if (figure is Polyline)
                        points.AddRange(((Polyline)figure).Points);
                    else if (figure is Circle)
                    {
                        points.Add(new Point(((Circle) figure).Center.X + ((Circle) figure).Radius,
                                             ((Circle) figure).Center.Y));
                        points.Add(new Point(((Circle) figure).Center.X,
                                             ((Circle) figure).Center.Y + ((Circle) figure).Radius));
                        points.Add(new Point(((Circle) figure).Center.X - ((Circle) figure).Radius,
                                             ((Circle) figure).Center.Y));
                        points.Add(new Point(((Circle) figure).Center.X,
                                             ((Circle) figure).Center.Y - ((Circle) figure).Radius));

                    }
                }
                double minX = points[0].X, maxX = points[0].X, minY = points[0].Y, maxY = points[0].Y;
                foreach (var point in points)
                {
                    if (point.X < minX) minX = point.X;
                    if (point.X > maxX) maxX = point.X;
                    if (point.Y < minY) minY = point.Y;
                    if (point.Y > maxY) maxY = point.Y;
                }
                var listpo = new List<Point>()
                                 {
                                     new Point(minX, maxY),
                                     new Point(minX, minY),
                                     new Point(maxX, minY),
                                     new Point(maxX, maxY)
                                 };
                poly = new ConvexPolygon(listpo);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод параллельного переноса полигона Area к другой точке по вектору переноcа, от текущей точки центра к той, на которую переносим.
        /// </summary>
        /// <param name="point">Точка, к которой осуществляется параллельный перенос</param>
        /// <returns>Сдвинутый полигон на вектор переноса</returns>
        public ConvexPolygon SetPoly(Point point)
        {
            var newPoints = new List<Point>();
            var center = Position;
            var vector = new Point(point.X - center.X, point.Y - center.Y);
            foreach (var _point in poly.Points)
            {
                newPoints.Add(new Point(_point.X+vector.X, _point.Y+vector.Y));
            }
            return new ConvexPolygon(newPoints);
        }

        /// <summary>
        /// Добавление фигуры в список фигур Area
        /// </summary>
        /// <param name="f">Добавляемая фигура</param>
        public void AddFigure(Figure f)
        {
            figures.Add(f);
        }

        /// <summary>
        /// Удаление выбранной фигуры (Конкретной) из списка фигур Area
        /// </summary>
        /// <param name="f">Удаляемая фигура</param>
        public void DelFigure(Figure f)
        {
            figures.Remove(f);
        }

        /// <summary>
        /// Очищает список фигур Area
        /// </summary>
        public void ClrFigure()
        {
            figures = new List<Figure>();
        }

        #endregion

    }
}
