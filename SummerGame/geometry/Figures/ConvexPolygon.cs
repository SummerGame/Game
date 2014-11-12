using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;
using Geometry.Resources;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Выпуклый Многоугольник. Для полигона установлены соглашение о заполенении полигона ПРОТИВ ЧАСОВОЙ стрелки.
    /// </summary>
    public class ConvexPolygon : Figure
    {
        #region Поля
        /// <summary>
        /// точки, по которым строится полигон
        /// </summary>
        private List<Point> points;
        /// <summary>
        /// точки имеют координаты векторов нормали к отрезкам, соединяющим точки из points
        /// </summary>
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

        /// <summary>
        /// Получает список отрезков, соединяющий точки из списка и нормали к ним
        /// 
        /// </summary>
        /// <param name="newPoints">Список точек, отмеченных в окне редактора карт</param>
        public ConvexPolygon(List<Point> newPoints)
        {
            var set = Polyline.PointsToSegments(newPoints);
            if (!CheckSelfIntersection(set))
            {
                this.points = newPoints;
                normals = new List<Point>(/*set.Count*/);
                for (int index = 0; index < set.Count; index++)
                {
                    var segment = set[index];
                    normals.Add(GetLeftNormal(segment));
                }
            }
            else
            {
                throw new Exception(Messages.SelfIntersection);
            }
        }

        public ConvexPolygon()
        {
        }

        /// <summary>
        /// Преобразует массив координат в список точек, получает нормали для отрезков, соединяющих эти точки
        /// </summary>
        /// <param name="coords">список координат</param>
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
                if (!CheckSelfIntersection(set))
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
                    throw new Exception(Messages.SelfIntersection);
                }
            }
        }

        /// <summary>
        /// Преобразует массив точек в список точек, получает нормали для отрезков, соединяющих эти точки
        /// </summary>
        /// <param name="point"></param>
        public ConvexPolygon(params Point[] point)
        {
            if (point.Count() > 0)
            {
                var newPoints = point.ToList();
                var set = Polyline.PointsToSegments(newPoints);
                if (!CheckSelfIntersection(set))
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
                    throw new Exception(Messages.SelfIntersection);
                }
            }
        }

        /// <summary>
        /// Устанавливает список точек и нормалей
        /// </summary>
        public ConvexPolygon(List<Point> newPoints, List<Point> normals)
        {
            var set = Polyline.PointsToSegments(newPoints);
            if (!CheckSelfIntersection(set))
            {
                this.points = newPoints;
                this.normals = normals;
            }
            else
            {
                throw new Exception(Messages.SelfIntersection);
            }
        }

        /// <summary>
        /// Возвращает список точек с вершинами квадрата, длина ребра которого edge, одна из точек - point
        /// </summary>
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

        /// <summary>
        /// Преобразует точки в точки с целыми координатами
        /// </summary>
        public ConvexPolygon toInt()
        {
            points.ForEach(x => x.toInt());
            return new ConvexPolygon(points);
        }

        /// <summary>
        /// Получает левую нормаль к данному отрезку
        /// </summary>
        private Point GetLeftNormal(Segment segment)
        {
            double x = segment.End.X - segment.Begin.X;
            double y = segment.End.Y - segment.Begin.Y;
            return new Point(-y,x);
        }

        /// <summary>
        /// Рассчитывает центр многоугольника
        /// </summary>
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
        /// Проверка самопересечения многоугольника
        /// </summary>
        public static bool CheckSelfIntersection(List<Segment> segments)
        {
            int start_j = 1;
            // нет проверки пересечения для отрезка last-0
            for (int i = 0; i < segments.Count - 1; i++)
            {
                for (int j = start_j; j < segments.Count; j++)
                {
                    if (i != j && i != j - 1 && j != i - 1 && !(i == 0 && j == segments.Count - 1))
                    {
                        if (Intersect.IsLinePartsIntersected(segments[i].Begin, segments[i].End, segments[j].Begin, segments[j].End))
                        // if (Intersect.IsIntersected(segments[i], segments[j]))
                        { return true; }
                    }
                }
                start_j++;
            }

            return false;
        }
        
        /// <summary>
        /// Разбиение невыпуклого многоугольника на выпуклые
        /// </summary>
        /// <param name="points">Принимает список точек, по которым строится многоугольник</param>
        /// <returns> Возвращает список, состоящий из список точек для многоугольников</returns>
        public List<List<Point>> ConvertToConvexList(List<Point> points)
         {
            var itog = new List<List<Point>>(); // список возвращаемых полигонов (выпуклых многоугольников)

            if (points.Count <= 3)
            {
                itog.Add(points);
                return itog;
            }

            var obol = BelongingConvexHull(points);  // список, в котором указано, принадлежит точка оболочке (1) или нет (0)
            var ossoben = 0; // количество особенностей
            //Особенность – это такие подряд идущие точки многоугольника, 
            //каждая из которых не принадлежит его выпуклой оболочке.  

            ossoben = featuresCount(obol);

            if (ossoben == 0) // ! особенностей нет => многоугольник уже выпуклый
            {
                itog.Add(points);
                return itog; // возвращаем многоугольник (все точки)
            }

            #region more than one ossoben

            else if (ossoben > 1) // если особенностей 2 и больше
            {
                Tuple<Point, int, int, Point, int, int> featurp = ClosestFeaturesPoints(points); 
                List<Point> firstPoligon = new List<Point>();
                List<Point> secondPoligon = new List<Point>();
                var firstPoNum = 0;
                var secPoNum = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    // если координаты текущей точки совпадают с координатами первой точки из тьюпла
                    if ((points[i].X == (featurp.Item1 as Point).X) && (points[i].Y == (featurp.Item1 as Point).Y))
                    {
                        firstPoNum = i; // следующие точки принадлежат второму полигону
                    }
                    if ((points[i].X == (featurp.Item4 as Point).X) && (points[i].Y == (featurp.Item4 as Point).Y))
                    {
                        secPoNum = i; // следующие точки принадлежат первому полигону
                    }
                }
                if (firstPoNum > secPoNum)
                {
                    var temp = secPoNum;
                    secPoNum = firstPoNum;
                    firstPoNum = temp;
                }

                // разбиение на два выпуклых многоугольника 
                for (int i = 0; i <= firstPoNum; i++)
                {
                    firstPoligon.Add(points[i]);
                }
                for (int i = firstPoNum; i <= secPoNum; i++)
                {
                    secondPoligon.Add(points[i]);
                }
                for (int i = secPoNum; i < points.Count; i++)
                {
                    firstPoligon.Add(points[i]);
                }

                var newPol = ConvertToConvexList(firstPoligon);

                foreach (var list in newPol) itog.Add(list);

                newPol = ConvertToConvexList(secondPoligon);

                foreach (var list in newPol) itog.Add(list);

                return itog;
            }
            #endregion

            // только одна особенность
            else
            {
                //var leng = 0.0; //наибольшее расстояние между точкой особенности и точкой не особенности
                var firPoint = new Point(0, 0);
                var secPoint = new Point(0, 0); // точки основания особенности
                var firstPoNum = 0;
                var secPoNum = 0;
                List<Point> firstPoligon = new List<Point>();
                List<Point> secondPoligon = new List<Point>();


                for (int i = 0; i < points.Count; i++)
                {
                    var p = i; //?
                    // рассматриваем точки выпуклой оболочки
                    if (obol[p])
                    {
                        if (p + 1 == points.Count) // проверка граничных точек
                        {
                            //если первая точка является точкой особенности, значит больше точек выпуклой оболочки нет
                            if (!obol[0])
                            {
                                firPoint = points[p];
                            }
                            //предыдущая точка - последняя точка из особенности
                            if (!obol[p - 1])
                            {
                                secPoint = points[p];
                            }
                        }
                        else if (p == 0) // проверка граничных точек
                        {
                            //если последняя точка принадлежала особенности, значит, на данной точке особенность оканчивается
                            //данная точка является одной из точек разбиения
                            if (!obol[points.Count - 1])
                            {
                                secPoint = points[p];
                            }
                            //предыдущая точка была последней из точек особенности
                            if (!obol[p + 1])
                            {
                                firPoint = points[p];

                            }
                        }
                        //следующая точка принадлежит особенности => данная точка - первая точка разбиения
                        else if (!obol[p + 1])
                        {
                            firPoint = points[p];

                        }
                        //предыдущая точка принадлежала особенности => данная точка - вторая точка разбиения
                        else if (!obol[p - 1])
                        {
                            secPoint = points[p];

                        }
                    }
                }

                var vectorOsn = new Point(secPoint.X - firPoint.X, secPoint.Y - firPoint.Y); // основание особенности
                var znam = Math.Sqrt((secPoint.X - firPoint.X) * (secPoint.X - firPoint.X) + (secPoint.Y - firPoint.Y) * (secPoint.Y - firPoint.Y));
                var constvalue = secPoint.Y * firPoint.X - secPoint.X * firPoint.Y;
                var height = 0.0;

                //отыскание первой точки, по которой будет вестись разбиение
                for (int i = 0; i < points.Count; i++)
                {
                    if (!obol[i])
                    {
                        var mul_vect = (firPoint.Y - secPoint.Y) * points[i].X + (secPoint.X - firPoint.X) * points[i].Y + constvalue;

                        mul_vect /= znam;
                        if (Math.Abs(mul_vect) > height)
                        {
                            height = Math.Abs(mul_vect);
                            firstPoNum = i;
                        }
                    }
                }
                height = 0;

                // отыскание второй точки, по которой будет вестись разбиение
                for (int i = 0; i < points.Count; i++)
                {
                    if (obol[i])
                    {
                        var mul_vect = (firPoint.Y - secPoint.Y) * points[i].X + (secPoint.X - firPoint.X) * points[i].Y + constvalue;
                        mul_vect /= znam;

                        if (Math.Abs(mul_vect) > height)
                        {
                            height = Math.Abs(mul_vect);
                            secPoNum = i;
                        }
                    }
                }

                if (firstPoNum > secPoNum)
                {
                    var temp = secPoNum;
                    secPoNum = firstPoNum;
                    firstPoNum = temp;
                }
                //разбиение на два полигона
                for (int i = 0; i < firstPoNum + 1; i++)
                {
                    firstPoligon.Add(points[i]);
                }
                for (int i = firstPoNum; i < secPoNum + 1; i++)
                {
                    secondPoligon.Add(points[i]);
                }

                for (int i = secPoNum; i < points.Count; i++)
                {
                    firstPoligon.Add(points[i]);
                }

                var newPol = ConvertToConvexList(firstPoligon); // рекурсия для первого полигона

                foreach (var list in newPol) itog.Add(list); // добавляем точки первого полигона в результат

                newPol = ConvertToConvexList(secondPoligon); // рекурсия для второго полигона

                foreach (var list in newPol) itog.Add(list); // добавляем точки второго полигона в результат

                return itog;
            }

        }

        ///<summary>
        ///Сортировка точек по углу поворота
        /// </summary>
        public List<Point> SortByAngle(List<Point> below, Point itogpoint)
        {
            for (int i = 0; i < below.Count - 1; i++)
            {
                Vector vector1, vector2, vector3;
                if (itogpoint.X < 0)
                    vector1 = new Vector(-itogpoint.X, 0);
                else
                    vector1 = new Vector(0, itogpoint.Y);
                double checkX = below[i].X - itogpoint.X;
                double checkX1 = below[i + 1].X - itogpoint.X;

                vector2 = new Vector(checkX, below[i].Y - itogpoint.Y);
                vector3 = new Vector(checkX1, below[i + 1].Y - itogpoint.Y);

                var first = Vector.get_angle(vector1,vector2);
                var second = Vector.get_angle(vector1,vector3);

                if ((first < second)
                    || // Меньше угол
                    ((first == second) && (vector2.X < vector3.X))) // Если углы одинаковые и точка ближе
                {
                    var tmp = below[i];
                    below[i] = below[i + 1];
                    below[i + 1] = tmp;
                    i = -1;
                }
            }
            return below;
        }

        ///<summary>
        ///Построение выпуклой оболочки
        /// </summary>        
        public List<Point> GetConvexHull(List<Point> points)
        {
            var newpoints = new List<Point>();

            foreach (var point in points)
            {
                newpoints.Add(point);
            }

            var itogpoints = new List<Point>();

            int len = points.Count;

            var firsObloPoint = points[0];

            //отыскание стартовой точки
            foreach (var point in points)
            {
                if (point.X < firsObloPoint.X)
                {
                    firsObloPoint = point;
                }
            }

            itogpoints.Add(firsObloPoint);
            newpoints.Remove(firsObloPoint);

            #region SortByAngle

            var below = new List<Point>();
            var above = new List<Point>();

            foreach (var point in newpoints)
            {
                if (point.Y <= itogpoints[0].Y)
                    below.Add(point);
                else
                    above.Add(point);
            }

            if (below.Count > 0)
            {

                // сортировка точек, располагающихся ниже самой левой точки
                SortByAngle(below, itogpoints[0]);

            }
            // сортировка точек, располагающихся выше самой левой точки 
            if (above.Count > 0)
            {

                SortByAngle(above, itogpoints[0]);
                above.Reverse();

            }

            newpoints.Clear();
            foreach (var point in below)
            {
                newpoints.Add(point);
            }
            foreach (var point in above)
            {
                newpoints.Add(point);
            }

            #endregion

            itogpoints.Add(newpoints[0]);
            newpoints.RemoveAt(0);

            for (int i = 0; i < newpoints.Count; i++)
            {
                var countItog = itogpoints.Count;
                if (itogpoints[countItog - 1] == newpoints[i])
                {
                    int q = 0;
                }
                var direct = Point.getDirect(itogpoints[countItog - 2], itogpoints[countItog - 1], newpoints[i]);

                if (direct) // TODO get out 6250
                    itogpoints.Add(newpoints[i]);
                else
                {
                    if (itogpoints.Count > 2)
                    {
                        itogpoints.Remove(itogpoints.Last());
                        i--;
                    }

                }
            }
            return itogpoints;
        }

        ///<summary>
        ///Возвращает список значение типа bool, где true означает, что точка принадлежит выпуклой оболочке
        ///false - что не принадлежит 
        /// </summary>
        public List<bool> BelongingConvexHull(List<Point> points)
        {

            var list = GetConvexHull(points);
            var itoglist = new List<bool>();
            foreach (var point in points)
            {
                if (list.Contains(point))
                {
                    itoglist.Add(true);

                }
                else itoglist.Add(false);
            }
            return itoglist;
        } 
        ///<summary>
        /// Находит ближайшие точки, принадлежащие разным особенностям
        /// </summary>        
        public Tuple<Point, int, int, Point, int, int> ClosestFeaturesPoints(List<Point> points)
        {
            var listobl = BelongingConvexHull(points);
            var features = featuresCount(listobl);

            List<Point> obol = new List<Point>();
            for (int i = 0; i < listobl.Count; i++)
            {
                if (listobl[i]) obol.Add(points[i]);
            }


            List<List<Point>> internalLists = new List<List<Point>>();
            for (int i = 0; i < features; i++)
            {
                internalLists.Add(new List<Point>());
            }
            if (!listobl[0] && !listobl[listobl.Count - 1])
                internalLists.Add(new List<Point>());
            //заполнение списков особенностей
            for (int i = 0, j = 0; i < listobl.Count; i++)
            {
                if (!listobl[i])
                {
                    if (i + 1 != listobl.Count)
                    {
                        if (listobl[i + 1])
                        {
                            internalLists[j].Add(points[i]);
                            j++;
                        }
                        else
                        {
                            internalLists[j].Add(points[i]);
                        }
                    }
                    else
                    {
                        if (listobl[0])
                        {
                            internalLists[j].Add(points[i]);

                        }
                        else
                        {
                            internalLists[0].Insert(0, points[i]);
                        }
                    }
                }
            }

            if (!listobl[0] && !listobl[listobl.Count - 1])
            {
                for (int i = internalLists.Last().Count - 1; i >= 0; i--)
                {
                    internalLists[0].Insert(0, (internalLists.Last())[i]);
                    internalLists.Last().RemoveAt(i);
                }
                    
            }

            for (int i = 0; i < internalLists.Count; i++)
            {
                if (internalLists[i].Count == 0)
                {
                    internalLists.RemoveAt(i);
                    i--;
                }
            }

            var prevPoint = new Point(0, 0); // Точка начала основания особенности
            var nexPoint = new Point(0, 0); // Точка конца основания оссобенности
            var FeaturePoint = new Point(0, 0); // Текущая рассматриваемая точка оссобенности
            var osnVector = new Point(0, 0); // Вектор основания особенности
            var osnNormVector = new Point(0, 0); // Вектор нормали основания особенности

            var length = 999999999999.0;
            var fPoint = new Point(0, 0);
            var sPoint = new Point(0, 0);
            var reti = 0; var retNumi = 0;
            var retk = 0; var retNumk = 0;
            for (int i = 0; i < internalLists.Count; i++)
            {
                for (int j = 0; j < internalLists[i].Count; j++)
                {
                    for (int k = i + 1; k < internalLists.Count; k++)
                    {
                        for (int p = 0; p < internalLists[k].Count; p++)
                        {
                            var len = Point.Length(internalLists[i][j], internalLists[k][p]);
                            
                            if (len < length)
                            {
                                length = len;
                                fPoint = internalLists[i][j]; reti = i; retNumi = j;
                                sPoint = internalLists[k][p]; retk = k; retNumk = p;
                            }
                        }
                    }
                }
            }

            return new Tuple<Point, int, int, Point, int, int>(fPoint, reti, retNumi, sPoint, retk, retNumk);
        }

        ///<summary>
        ///Подсчёт количества особенностей для метода convertToConvexList
        /// </summary>        
        public int featuresCount(List<bool> ListOblo)
        {
            int ossoben = 0;
            for (int i = 0; i < ListOblo.Count; i++)
            {
                if (!ListOblo[i])
                {
                    if (i + 1 != ListOblo.Count)
                    {
                        if (ListOblo[i + 1]) ossoben++;
                    }
                    else 
                    {
                        if (ListOblo[0])
                            ossoben++;
                    }
                }
            }

            return ossoben;
        }

    }
}
