using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using AbstractGameEngine;
using GameEngine.Characters;
using GameEngine.Lands;
using GameEngine.Object;
using Geometry;
using Geometry.Figures;

namespace GameEngine
{
    /// <summary>
    /// Статический Класс "Расширение"
    /// Является универсальным классом для применения серилизации/десерилизации объектов карты.
    /// </summary>
    public static class Extension
    {

        public delegate T ElementConverter<T>(XElement xelement);

        public delegate T ListConverter<T>(List<XElement> xelements, ElementConverter<T> converter);

        private static List<Unit> ReadUnits(List<XElement> xelems, ElementConverter<Unit> converter)
        {
            return xelems.ConvertAll(x => converter(x));
        }

        private static Unit ReadUnit(XElement xelem)
        {
            if (xelem.Name.ToString().Equals("Unit"))
            {

            }
            else
            {
                throw new XmlSyntaxException("Invalid tag name for ReadUnit.");
            }
            return null;
        }

        #region Methods

        /// <summary>
        /// Метод серилизации для Вещи
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Item item)
        {
            if (item is Goods)
            {
                var el = new XElement("Goods");
                var goods = (Goods)item;
                el.SetAttributeValue("Count", goods.Count);
                el.SetAttributeValue("ItemType", goods.ItemType);
                return el;
            }
            else if (item is EquipmentMark)
            {
                var el = new XElement("EquipmentMark");
                var equipment = (EquipmentMark)item;
                el.SetAttributeValue("Count", equipment.Count);
                el.SetAttributeValue("Crew", equipment.Crew);
                el.SetAttributeValue("FireRate", equipment.FireRate);
                el.SetAttributeValue("MoveSpeed", equipment.MoveSpeed);
                el.SetAttributeValue("Armor", equipment.Armor);
                el.SetAttributeValue("Weapon", equipment.Weapon);
                return el;
            }

            throw new Exception("Invalid itemType tried to be saved.");
        }

        /// <summary>
        /// Метод сериализации для группы вещей
        /// </summary>
        /// <param name="items">вещи</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this List<Item> items)
        {
            var el = new XElement("Items");
            foreach (var item in items)
            {
                el.Add(item.Serialize());
            }
            return el;
        }

        /// <summary>
        /// Метод серилизации для Группы
        /// </summary>
        /// <param name="group">Группа</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Group group)
        {
            var el = new XElement("Group");
            el.SetAttributeValue("Count", group.Count);
            el.SetAttributeValue("Rank", group.Rank);
            el.SetAttributeValue("Specialization", group.Specialization);
            el.SetAttributeValue("Qualification", group.Qualification);
            el.SetAttributeValue("Experience", group.Experience);
            el.SetAttributeValue("Vitality", group.Vitality);
            return el;
        }

        /// <summary>
        /// Метод серилизации Списка Юнитов
        /// Серилизует все Юниты.
        /// </summary>
        /// <param name="units">Список Юнитов</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this List<Unit> units)
        {
            var el = new XElement("Characters");
            foreach (var unit in units)
            {
                el.Add(unit.Serialize());
            }
            return el;
        }

        /// <summary>
        /// Метод серилизации Юнита
        /// Серилизует все группы Юнита и его свойства.
        /// </summary>
        /// <param name="unit">Юнит</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Unit unit)
        {
            var el = new XElement("Unit");
            el.SetAttributeValue("Side", unit.Side.TeamNumber);
            var props = unit.Properties;

            el.SetAttributeValue("Visible", props.Visible);
            el.SetAttributeValue("CurSpeed", props.CurSpeed);
            el.SetAttributeValue("CurVisible", props.CurVisible);
            el.SetAttributeValue("Speed", props.Speed);
            el.SetAttributeValue("UnitType", unit.UnitType);

            var groups = new XElement("Groups");
            foreach (var group in props.Groups)
            {
                groups.Add(group.Serialize());
            }

            var items = new XElement("Items");
            foreach (var item in props.Items)
            {
                items.Add(item.Serialize());
            }

            var polygon = unit.Polygon.Serialize();

            var area = new XElement("Area");
            foreach (var figure in unit.Figures)
            {
                area.Add(figure.Serialize());
            }

            el.Add(polygon);
            el.Add(area);
            el.Add(groups);
            el.Add(items);

            return el;
        }

        /// <summary>
        /// Метод серилизации для Круга
        /// </summary>
        /// <param name="circle">Круг</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Circle circle)
        {
            var el = new XElement("Circle");
            el.SetAttributeValue("Radius", circle.Radius);
            el.SetAttributeValue("X", circle.Center.X);
            el.SetAttributeValue("Y", circle.Center.Y);
            return el;
        }

        /// <summary>
        /// Метод серилизации для Полигона
        /// </summary>
        /// <param name="polygon">Полигон</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this ConvexPolygon polygon)
        {
            var el = new XElement("ConvexPolygon");

            var points = polygon.Points.Select(x => x.X.ToString() + " " + x.Y.ToString()).ToList().Aggregate((x, y) => x + " " + y);
            var normals = polygon.Normals.Select(x => x.X.ToString() + " " + x.Y.ToString()).ToList().Aggregate((x, y) => x + " " + y);
            el.SetAttributeValue("Points", points);
            el.SetAttributeValue("Normals", normals);
            return el;
        }

        /// <summary>
        /// Метод серилизации для Ломанной
        /// </summary>
        /// <param name="polyline">Ломанная</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Polyline polyline)
        {
            var el = new XElement("Polyline");
            var points = polyline.Points.Select(x => x.X.ToString() + " " + x.Y.ToString()).ToList().Aggregate((x, y) => x + " " + y);
            el.SetAttributeValue("Points", points);
            return el;
        }

        /// <summary>
        /// Метод серилизации для Точки
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Point point)
        {
            var el = new XElement("Point");
            el.SetAttributeValue("X", point.X);
            el.SetAttributeValue("Y", point.Y);
            return el;
        }

        /// <summary>
        /// Метод серилизации для Отрезка
        /// </summary>
        /// <param name="Segment">Отрезок</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Segment Segment)
        {
            var el = new XElement("Segment");
            el.SetAttributeValue("X1", Segment.Begin.X);
            el.SetAttributeValue("Y1", Segment.Begin.Y);
            el.SetAttributeValue("X2", Segment.End.X);
            el.SetAttributeValue("Y2", Segment.End.Y);
            return el;
        }

        /// <summary>
        /// Метод серилизации для Фигуры
        /// Определяет тип фигуры и вызывает для нее ее специальный метод серилизации.
        /// </summary>
        /// <param name="figure">Фигура</param>
        /// <returns>Xml Element</returns>
        private static XElement Serialize(this Figure figure)
        {
            if (figure is Circle)
            {
                return ((Circle)figure).Serialize();
            }
            else if (figure is ConvexPolygon)
            {
                return ((ConvexPolygon)figure).Serialize();
            }
            else if (figure is Segment)
            {
                return ((Segment)figure).Serialize();
            }
            else if (figure is Point)
            {
                return ((Point)figure).Serialize();
            }
            else if (figure is Polyline)
            {
                return ((Polyline)figure).Serialize();
            }
            throw new Exception("Unavailable figure tryed to be serialized");

        }

        /// <summary>
        /// Метод серилизации для Местности
        /// Определяет имя местности и возвращает xml елемент с таким именем, содержащий все фигуры местности.
        /// </summary>
        /// <param name="landscape">Местность</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this Landscape landscape)
        {
            var el = new XElement(landscape.GetType().Name);
            foreach (var figure in landscape.Figures)
            {
                el.Add(figure.Serialize());
            }
            return el;
        }

        /// <summary>
        /// Метод серилизации для Карты
        /// Серилизует корневой элемент и все его включающие.
        /// </summary>
        /// <param name="map">Карта</param>
        /// <returns>Xml Element</returns>
        public static XElement Serialize(this GameMap map)
        {
            var el = new XElement("Map");
            el.SetAttributeValue("Width", map.Width);
            el.SetAttributeValue("Height", map.Height);

            foreach (var land in map.Lands)
            {
                el.Add(land.Serialize());
            }
            return el;
        }

        /// <summary>
        /// Метод десерилизации для списка фигур
        /// Считывает xml элементы и для каждого из них сопоставляет нужную фигуру, после чего возвращает все фигуры списоком.
        /// Если фигура не может быть определена или не существует, то бросает исключительную ситуацию.
        /// </summary>
        /// <param name="xelements">Xml Elements</param>
        /// <returns>Список фигур</returns>
        public static List<Figure> Deserialize(this IEnumerable<XElement> xelements)
        {
            var figures = new List<Figure>(xelements.Count());
            if (xelements != null)
            {

                foreach (var xElement in xelements)
                {
                    switch (xElement.Name.ToString())
                    {
                        case "Circle":
                            figures.Add(new Circle((double)xElement.Attribute("X"), (double)xElement.Attribute("Y"), (double)xElement.Attribute("Radius")));
                            break;
                        case "ConvexPolygon":
                            var points = new List<Point>();
                            var normals = new List<Point>();
                            var strs = (((string)xElement.Attribute("Points")).Split(' '));
                            for (int index = 0; index < strs.Length; index += 2)
                            {
                                var f = strs[index];
                                var s = strs[index + 1];
                                points.Add(new Point(Convert.ToDouble(f.Replace(".", ",")), Convert.ToDouble(s.Replace(".", ","))));
                            }

                            strs = (((string)xElement.Attribute("Normals")).Split(' '));
                            if (strs.ToList().Count > 1)
                            {
                                for (int index = 0; index < strs.Length; index += 2)
                                {
                                    var f = strs[index];
                                    var s = strs[index + 1];
                                    normals.Add(new Point(Convert.ToDouble(f.Replace(".", ",")), Convert.ToDouble(s.Replace(".", ","))));
                                }
                                figures.Add(new ConvexPolygon(points, normals));
                            }
                            else
                            {
                                figures.Add(new ConvexPolygon(points));
                            }
                            break;
                        case "Segment":
                            figures.Add(new Segment((double)xElement.Attribute("X1"), (double)xElement.Attribute("Y1"), (double)xElement.Attribute("X2"), (double)xElement.Attribute("Y2")));
                            break;
                        case "Point":
                            figures.Add(new Point((double)xElement.Attribute("X"), (double)xElement.Attribute("Y")));
                            break;
                        case "Polyline":
                            var _points = new List<Point>();
                            var _strs = (((string)xElement.Attribute("Points")).Split(' '));
                            for (int index = 0; index < _strs.Length; index += 2)
                            {
                                var f = _strs[index];
                                var s = _strs[index + 1];
                                _points.Add(new Point(Convert.ToDouble(f), Convert.ToDouble(s)));
                            }
                            figures.Add(new Polyline(_points));
                            break;
                        default:
                            throw new Exception("Unavailable Figure used in deserialize function.");
                    }
                }
            }
            return figures;
        }

        /// <summary>
        /// Метод десерилизации для Местности
        /// Определяет местность по имени и считывает все фигуры этой местности.
        /// Если местность не может быть определена или не существует, то бросает исключительную ситуацию.
        /// </summary>
        /// <param name="xelement">Xml Element</param>
        /// <returns>Местность</returns>
        public static object Deserialize(this XElement xelement)
        {
            var name = xelement.Name.ToString();
            switch (name)
            {
                case "City":
                    return new City(xelement.Elements().Deserialize());
                case "Sand":
                    return new Sand(xelement.Elements().Deserialize());
                case "Road":
                    return new Road(xelement.Elements().Deserialize());
                case "Mountains":
                    return new Mountains(xelement.Elements().Deserialize());
                case "Water":
                    return new Water(xelement.Elements().Deserialize());
                case "Lowland":
                    return new Lowland(xelement.Elements().Deserialize());
                case "Field":
                    return new Field(xelement.Elements().Deserialize());
                case "Forest":
                    return new Forest(xelement.Elements().Deserialize());
                case "Unit":
                    var xpolygon = xelement.Element("ConvexPolygon");
                    var polygon = ((new List<XElement> { xpolygon }).Deserialize()[0]) as ConvexPolygon;

                    var xgroups = xelement.Element("Groups");
                    var groups = new List<Group>();
                    if (xgroups != null)
                    {
                        foreach (var xgroup in xgroups.Elements())
                        {
                            var rank = (Rank)((Enum.Parse(typeof(Rank), (string)xgroup.Attribute("Rank"))));
                            var specialization = (Specialization)((Enum.Parse(typeof(Specialization), (string)xgroup.Attribute("Specialization"))));
                            var qualification = (Qualification)((Enum.Parse(typeof(Qualification), (string)xgroup.Attribute("Qualification"))));
                            var experience = (Qualification)((Enum.Parse(typeof(Qualification), (string)xgroup.Attribute("Experience"))));
                            var vitality = (Vitality)((Enum.Parse(typeof(Vitality), (string)xgroup.Attribute("Vitality"))));
                            groups.Add(new Group((int)xgroup.Attribute("Count"), rank, specialization, qualification, experience, vitality));
                        }
                    }

                    var xitems = xelement.Element("Items");
                    var items = new List<Item>();
                    if (xitems != null)
                    {
                        foreach (var xitem in xitems.Elements())
                        {
                            switch (xitem.Name.ToString())
                            {
                                case "Goods":
                                    var objecttype =
                                        (ObjectType)((Enum.Parse(typeof(ObjectType), (string)xitem.Attribute("ItemType"))));
                                    items.Add(new Goods(objecttype, (int)xitem.Attribute("Count"), new Circle(0, 0, 0)));
                                    break;
                                case "EquipmentMark":
                                    var weapon = (Caliber)((Enum.Parse(typeof(Caliber), (string)xitem.Attribute("Weapon"))));
                                    var armor = (Caliber)((Enum.Parse(typeof(Caliber), (string)xitem.Attribute("Armor"))));
                                    items.Add(new EquipmentMark(weapon, armor, (double)(xitem.Attribute("MoveSpeed")), (double)(xitem.Attribute("FireRate")), (int)(xitem.Attribute("Crew")), (int)(xitem.Attribute("Count"))));
                                    break;
                            }
                        }
                    }

                    var unit = new Unit(polygon);
                    var features = new UnitFeatures(groups, items);
                    features.CurSpeed = (double)xelement.Attribute("CurSpeed");
                    features.Visible = (double)xelement.Attribute("Visible");
                    features.CurVisible = (double)xelement.Attribute("CurVisible");
                    features.Speed = (double)xelement.Attribute("Speed");
                    unit.Properties = features;
                    unit.SetSide((int)xelement.Attribute("Side"), Countries.USSR, null); // todo do this better

                    var unittype = xelement.Attribute("UnitType");
                    unit.UnitType = (UnitType)((Enum.Parse(typeof(UnitType), (string)unittype)));
                    // unit.


                    return unit;
                default:
                    throw new Exception("Not existing Landscape used as parameter in deserializing function. Exception name: " + name);
            }
        }

        #endregion
    }
}
