using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Lands;
using GameEngine.StoryTelling;
using Geometry;
using Geometry.Figures;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс "Движение" - Действие
    /// Предполагает движение объекта Юнита по ломаной
    /// </summary>
    public class MoveAction : AbstractGameEngine.Action<Unit>
    {
        #region Fields

        private List<Way> ways = new List<Way>();
        
        private Polyline poly;
        
        private int turnsToComplete;//количество ходов до конечной цели движения

        private Unit current;


        #endregion

        #region Constructors

        public MoveAction()
        {
        }

        public MoveAction(Unit unit, Polyline poly)
        {
            // TODO: Комментарий для самолетов, они не подтвержены коллизии
            // if (unit is FlyingVehicle )
            //ways = WayPolyline(unit, poly);
            // else
            current = unit;
            // ways = Collision(current, WayPolyline(current, poly));
            //ways = WaySegment(current, (Polyline.PointsToSegments(poly.Points))[0].Begin, (Polyline.PointsToSegments(poly.Points))[0].End);
            //Time = ways.ConvertAll(x => x.Time).Sum() + Game.GameTime;
            //ways = WaySegment(current, new Point(0, 0), new Point(4.3, 1));
            //4.3,1
            this.poly = poly;

            CalculateETA(unit.Position);
        }


        #endregion

        #region Properties

        public List<Way> Ways
        {
            get { return ways; }
            set { ways = value; }
        }

        public Point Origin
        {
            get { return poly.Points[0]; }
        }

        public Point Destination//возращает конечную точку пути отряда
        {
            get { return poly.LastPoint(); }
        }

        public Polyline LineWay//возвращает линию пути отряда
        {
            get { return poly; }
        }
        
        /// <summary>
        /// возвращает количество ходов до конечной цели движения
        /// </summary>
        public Int32 TurnsToComplete
        {
            get { return turnsToComplete; }
        }

        #endregion

        #region Methods

        public override object Do(Unit unit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Рассчитывает путь для войска, сколько ходов ему нужно, чтобы добраться до цели
        /// идёт расчёт времени по местностям, через которые войско проходит
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="time"></param>
        /// <returns>Траектория</returns>
        public override object Now(Unit unit, double time)
        {
            var states = new List<UnitState>();//концы отрезков пути по пересекаемым местностям
            states.Add(new UnitState(unit.Position, 0, StateAction.Move));
            CalculateETA(unit.Position);//подсчёт количества ходов до достижения цели перемещения

            var timeInterval = 0.0;
            int i = 0;
            while (timeInterval < Game.GameTimeInterval - 0.00001) // timeNow не используется сейчас 0.00001 - точность вычеслений
            {
                if (i < ways.Count)
                {
                    if (timeInterval + ways[i].Time < Game.GameTimeInterval)
                    {
                        timeInterval += ways[i].Time;
                        states.Add(new UnitState(ways[i].WayPoint, ways[i].Time, StateAction.Move));
                        unit.Position = ways.Last().WayPoint;
                        Completed = true;
                    }
                    else
                    {
                        var dif = timeInterval + ways[i].Time - Game.GameTimeInterval;
                        var kof = 1 - dif/ways[i].Time;
                        if (i > 0) //TODO сделать по человечески
                        {
                            var position = (new Segment(ways[i - 1].WayPoint, ways[i].WayPoint)).GetCurentPosition(kof);
                            states.Add(new UnitState(position, kof*ways[i].Time, StateAction.Move));
                            unit.Position = position;
                        }
                        else
                        {
                            var position = (new Segment(unit.Position, ways[i].WayPoint)).GetCurentPosition(kof);
                            states.Add(new UnitState(position, kof*ways[i].Time, StateAction.Move));
                            unit.Position = position;
                        }
                        Completed = false;
                        timeInterval += kof*ways[i].Time;
                    }
                    i++;
                }
                else
                {
                    Completed = true;
                    timeInterval = 1;
                    unit.Position = ways.Last().WayPoint;
                }
            }

            return states;//вернуть концы отрезков пути по пересекаемым местностям
        }

        /// <summary>
        /// Подсчитывает количество ходов до достижения цели перемещения.
        /// Изменяет значение поля turnsToComplete
        /// </summary>
        private void CalculateETA(Point position)
        {
            ways = WaySegment(current, position, (Polyline.PointsToSegments(poly.Points))[0].End);

            double allWayTime = 0;
            for (int j = 0; j < ways.Count; j++)
            {
                allWayTime += ways[j].Time;
            }
            turnsToComplete = (int)allWayTime;
        }

        /// <summary>
        /// Метод, который получает список "Путей" (Вспомогательных) и если будет обнаружена коллизия с одним из таких же путей других Юнитов, то путь текущего будет прекращен.
        /// </summary>
        /// <param name="unit">Текущий Юнит</param>
        /// <param name="curUnitWays">Текущие "Пути" Юнита</param>
        /// <returns>Список актуальныъх "Путей"</returns>
        private List<Way> Collision(Unit unit, List<Way> curUnitWays)
        {
            var movingUnits = Game.mainMap.Units.Where(x => x.CurrentAction != null && x.CurrentAction is MoveAction && x != unit);
            var curWays = curUnitWays;
            foreach (var movingUnit in movingUnits)
            {
                var action = (MoveAction)movingUnit.CurrentAction;
                var movingUnitWays = action.Ways;

                var curUnitPoints = unit.Polygon.Points;
                var movingUnitPoints = movingUnit.Polygon.Points;

                var collisionWayNumber = CheckWaysCollision(unit, movingUnit, curWays, movingUnitWays);
                if (collisionWayNumber != -1)
                {
                    curWays = curWays.GetRange(0, collisionWayNumber);
                }
            }
            return curWays;
        }

        /// <summary>
        /// Метод считает относительно Полигонов 2 Юнитов и их наборов Way коллизию, если находит, то возвращает индекс Way в котором врезается во второго Юнита.
        /// </summary>
        /// <param name="curUnit">Текущий Юнит</param>
        /// <param name="movingUnit">Другой Юнит</param>
        /// <param name="curUnitWays">"Пути" текущего Юнита</param>
        /// <param name="movingUnitWays">"Пути" другого Юнита</param>
        /// <returns>Индекс элемента "Пути" коллизии</returns>
        private int CheckWaysCollision(Unit curUnit, Unit movingUnit, List<Way> curUnitWays, List<Way> movingUnitWays)
        {
            var pointsCur = new List<Point>(curUnit.Polygon.Points);
            var pointsMoving = new List<Point>(movingUnit.Polygon.Points);

            var curUnitSegments = Polyline.PointsToSegments(curUnitWays.ConvertAll(x => x.WayPoint));
            var movingUnitSegments = Polyline.PointsToSegments(movingUnitWays.ConvertAll(x => x.WayPoint));

            // Цикл по всем отрезкам пути текущего Юнита
            for (int i = 0; i < curUnitSegments.Count; i++)
            {
                var curSegment = curUnitSegments[i];

                var curUnitVector = new Point(curSegment.End.X - curSegment.Begin.X,
                              curSegment.End.Y - curSegment.Begin.Y);

                // Перенесем все отрезки полигона текущего и рассматриваемого Юнитов на вектора их путей
                var curUnitPolySegments = Polyline.PointsToSegments(pointsCur);
                var curUnitPolyLastSegments = new List<Segment>();
                var curAnotherPolySegments = new List<Segment>();
                foreach (var cursegment in curUnitSegments)
                {
                    curUnitPolyLastSegments.Add(new Segment(new Point(cursegment.Begin.X + curUnitVector.X, cursegment.Begin.Y + curUnitVector.Y), new Point(cursegment.End.X + curUnitVector.X, cursegment.End.Y + curUnitVector.Y)));
                    // Достраиваем боковые отрезки
                    curAnotherPolySegments.Add(new Segment(cursegment.Begin, new Point(cursegment.Begin.X + curUnitVector.X, cursegment.Begin.Y + curUnitVector.Y)));
                }
                curUnitPolySegments.AddRange(curUnitPolyLastSegments);
                curUnitPolySegments.AddRange(curAnotherPolySegments);

                // Цикл по всем отрезкам пути движемого Юнита
                for (int j = 0; j < movingUnitSegments.Count; j++)
                {
                    var movingUnitSegment = movingUnitSegments[j];


                    var movingUnitVector = new Point(movingUnitSegment.End.X - movingUnitSegment.Begin.X,
                                                     movingUnitSegment.End.Y - movingUnitSegment.Begin.Y);

                    // Перенесем все отрезки полигона текущего и рассматриваемого Юнитов на вектора их путей
                    var movingUnitPolySegments = Polyline.PointsToSegments(pointsMoving);
                    var movingAnotherPolySegments = new List<Segment>();
                    var movingUnitPolyLastSegments = new List<Segment>();
                    foreach (var movingsegment in movingUnitSegments)
                    {
                        movingUnitPolyLastSegments.Add(new Segment(new Point(movingsegment.Begin.X + movingUnitVector.X, movingsegment.Begin.Y + movingUnitVector.Y), new Point(movingsegment.End.X + movingUnitVector.X, movingsegment.End.Y + movingUnitVector.Y)));
                        // Достраиваем боковые отрезки
                        movingAnotherPolySegments.Add(new Segment(movingsegment.Begin, new Point(movingsegment.Begin.X + movingUnitVector.X, movingsegment.Begin.Y + movingUnitVector.Y)));
                    }
                    movingUnitPolySegments.AddRange(movingUnitPolyLastSegments);
                    movingUnitPolySegments.AddRange(movingAnotherPolySegments);

                    var intersections = new List<Point>();
                    // Для всех отрезков движемого и текущего Юнитов, а также отрезков переноса по пути ищем пересечения

                    int movingIntersect = -1, curIntersect = -1;
                    for (int index = 0; index < movingUnitPolySegments.Count; index++)
                    {
                        var movingSegment = movingUnitPolySegments[index];
                        // Нашли пересечение - Выйдем
                        if (intersections.Count > 0)
                        {
                            movingIntersect = index;
                            break;
                        }
                        for (int index1 = 0; index1 < curUnitPolySegments.Count; index1++)
                        {
                            var currentSegment = curUnitPolySegments[index1];
                            intersections.AddRange(Intersect.GetIntersection(currentSegment, movingSegment));
                            // Нашли пересечение - Выйдем
                            if (intersections.Count > 0)
                            {
                                movingIntersect = index;
                                curIntersect = index1;
                                break;
                            }
                        }
                    }


                    var pathCurSegment = new Segment(curUnitPolySegments[curIntersect].Begin, curUnitPolySegments[curIntersect].End);
                    var pathMovingSegment = new Segment(movingUnitPolySegments[curIntersect].Begin, movingUnitPolySegments[curIntersect].End);
                    var pathCurVectorLen = Point.Length(pathCurSegment.Begin, pathCurSegment.End);
                    var pathMovingVectorLen = Point.Length(pathMovingSegment.Begin, pathMovingSegment.End);

                    // Посчитаем время
                    var currentTime = pathCurVectorLen / curUnit.Properties.CurSpeed;
                    var movingTime = pathMovingVectorLen / movingUnit.Properties.CurSpeed;

                    currentTime = currentTime < movingTime ? currentTime : movingTime;

                    // Получим точки на отрезке полигона по времени
                    var curTimedPoints = new List<Point>();
                    foreach (var segment in curAnotherPolySegments)
                    {
                        curTimedPoints.Add(segment.Position(currentTime, curUnitWays[i].Time));
                    }

                    var movingTimedPoints = new List<Point>();
                    foreach (var segment in movingAnotherPolySegments)
                    {
                        movingTimedPoints.Add(segment.Position(currentTime, movingUnitWays[i].Time));
                    }

                    // Создаем полигоны
                    ConvexPolygon curPolygon = new ConvexPolygon(curTimedPoints), movPolygon = new ConvexPolygon(movingTimedPoints);
                    intersections.Clear();
                    intersections.AddRange(Intersect.GetIntersection(curPolygon, movPolygon));

                    if (intersections.Count > 0 && currentTime > movingTime)
                    {
                        return i;
                    }

                    // Применяем параллельно перенесенный полигон, как текущий набор точек дял движемого Юнита
                    pointsMoving = Polyline.SegmentsToPoints(movingUnitPolyLastSegments);

                }

                // Применяем параллельно перенесенный полигон, как текущий набор точек дял текущего Юнита
                pointsCur = Polyline.SegmentsToPoints(curUnitPolyLastSegments);

            }

            return -1;
        }

        /// <summary>
        /// Метод, находит все "Пути" пересеченные ломанной с местностями.
        /// </summary>
        /// <param name="unit">Текущий Юнит</param>
        /// <param name="polyline">Ломанная линия</param>
        /// <returns>Список "Путей"</returns>
        private List<Way> WayPolyline(Unit unit, Polyline polyline)
        {
            var ways = new List<Way>();
            foreach (var segment in Polyline.PointsToSegments(polyline.Points))
            {
                ways.AddRange(WaySegment(unit, segment.Begin, segment.End));
            }
            return ways;
        }

        /// <summary>
        /// Метод, находит все "Пути" пересеченные отрезком с местностями.
        /// </summary>
        /// <param name="unit">Текущий Юнит</param>
        /// <param name="pointFirst">Начальная точка</param>
        /// <param name="pointLast">Конечаная точка</param>
        /// <returns>Список "Путей"</returns>
        /// 
        

        public List<Way> WaySegment(Unit unit, Point pointFirst, Point pointLast)
        {
            var glIntersects = new List<Point>();
            var times = new List<double>();
            var way = new List<Way>();
            var curArea = unit.GetCurrentArea();
            var internalSegment = new Segment(pointFirst, pointLast);
            var segStrucr = new List<SegSpeed>();

            foreach (var area in Game.mainMap.Lands)
            {
                var intersections = new List<Point>();
                foreach (var figure in area.Figures)
                {
                    var ints = Intersect.GetIntersection(figure, internalSegment);
                    //var pointCenter = new Point((ints.First().X + ints.Last().X)/2, (ints.First().Y + ints.Last().Y)/2);
                    intersections.AddRange(ints);
                }
                if (intersections.Count > 1)
                {
                    var maxLen = new List<Point>(getMaxLen(intersections));
                    var speed = area.Passability();
                    glIntersects.AddRange(maxLen);
                    segStrucr.Add(new SegSpeed(maxLen[0], maxLen[1], speed));
                }
            }

            glIntersects.Add(pointFirst);
            for (int j = 0; j < glIntersects.Count; j++)
            {
                for (int k = j + 1; k < glIntersects.Count; k++)
                {
                    if ((Point.Length(pointFirst, glIntersects[j]) > (Point.Length(pointFirst, glIntersects[k]))))
                    {
                        Point tmp = glIntersects[j];
                        glIntersects[j] = glIntersects[k];
                        glIntersects[k] = tmp;
                    }
                }
            }

            for (int i = 0; i < glIntersects.Count - 1; i++)
            {
                var speedCof = SegSpeed.GetModSpeed(segStrucr, glIntersects[i], glIntersects[i + 1]);
                var length = Point.Length(glIntersects[i], glIntersects[i + 1]);
                var speed = unit.Properties.Speed * speedCof;
                times.Add(length / speed);
            }

            times.Add(Point.Length(glIntersects.Last(), pointLast) / (unit.Properties.Speed * curArea.Passability()));
            glIntersects.Add(pointLast);
            glIntersects.RemoveAt(0);
            //}
            way = glIntersects.Zip(times, ((x, y) => new Way(x, y))).ToList();

            return way;
        }

        /// <summary>
        /// Метод для получения двух точек прохождения по Landscape
        /// </summary>
        /// <param name="points">Список точек на Landscape</param>
        /// <returns>Пара точек на Landscape</returns>
        public static List<Point> getMaxLen(List<Point> points)
        {
            if (points.Count < 2)
                return points;

            double length = 0;
            int indexf = 0, indexl = 1;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    if (Point.Length(points[i], points[j]) > length)
                    {
                        length = Point.Length(points[i], points[j]);
                        indexf = i;
                        indexl = j;
                    }
                }
            }
            return new List<Point> { points[indexf], points[indexl] };
        }

        #endregion
    }

    /// <summary>
    /// Класс "Путь"
    /// Является вспомогательной структурой для хранения Времени + Точки
    /// </summary>
    public class Way
    {
        #region Properties

        public Point WayPoint { get; set; }
        
        public double Time { get; set; }
        
        #endregion

        #region Constructors
        public Way(Point point, double time)
        {
            WayPoint = point;
            Time = time;
        }

        #endregion
    }

    internal class SegSpeed
    {
        public Point First { get; set; }
        public Point Last { get; set; }
        public double ModSpeed { get; set; }

        public SegSpeed(Point f, Point last, double sp)
        {
            First = f;
            Last = last;
            ModSpeed = sp;
        }

        public SegSpeed exept(Point f, Point l)
        {

            return this;
        }

        public static double GetModSpeed(List<SegSpeed> speeds, Point f, Point l)
        {
            foreach (var segSpeed in speeds)
            {
                if (((f == segSpeed.First) && (l == segSpeed.Last)) || ((l == segSpeed.First) && (f == segSpeed.Last)))
                {
                    return segSpeed.ModSpeed;
                }
            }
            return 0.8; //TODO сделать по человечески
            // 0.8 - const скорости передвижения по полю
        }
    }
}
