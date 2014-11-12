using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractGameEngine;
using GameEngine.Characters.Groups;
using GameEngine.Lands;
using GameEngine.Object;
using Geometry;
using Geometry.Figures;
using System.ComponentModel; // todo remove


namespace GameEngine.Characters
{
    /// <summary>
    /// Класс "Юнит" является персонажем и отрядом минимального размера.
    /// Его свойства задаются классом "Особенности".
    /// </summary>
    public class Unit : Character
    {
        #region Properties
        
        public UnitFeatures Properties
        {
            get { return (UnitFeatures)props; }
            set { props = value; }
        }

        public UnitType UnitType { get; set; }

        public OrganizationLevel OrganizationLevel
        {
            get { return OrganizationLevel.Battalion; } //todo implement organization levels
        }

        /// <summary>
        /// Authorized unit strength - a maximum number of men allowed to this unit
        /// by War Ministry regulations
        /// </summary>
        public int AuthorizedStrength
        {
            get { return 700; } // TODO implement authorized strengths
        }

        /// <summary>
        /// Количество боеприпасов, которые полагается иметь данному типу войск
        /// </summary>
        public int AuthorizedAmmo
        {
            get { return 1000; } // TODO implement authorized ammo
        }

        /// <summary>
        /// Сторона, к которой принадлежит юнит
        /// </summary>
        public new Team Side 
        {
            get { return base.Side as Team; }
        }

        
        private AutomaticCommander unitCommander;

        /// <summary>
        /// Полководец
        /// </summary>
        public AutomaticCommander UnitCommander
        {
            get { return unitCommander; }
            set { unitCommander = value; }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// 
        /// Создаёт юнит по по области его расположения
        /// </summary>
        /// <param name="polygon"></param>
        public Unit(ConvexPolygon polygon)
        {
            figures = new List<Figure> { polygon };
            Polygon = polygon;
            unitCommander = new AutomaticCommander("");
        }

        /// <summary>
        /// Создаёт юнит по области его расположения и заданным особенностям
        /// </summary>
        /// <param name="polygon">Область расположения</param>
        /// <param name="features">Свойства юнита</param>
        public Unit(ConvexPolygon polygon, UnitFeatures features)
        {
            figures = new List<Figure> { polygon };
            Polygon = polygon;
            props = features;
            unitCommander = new AutomaticCommander("",CommanderType.Common,this);
        }

        /// <summary>
        /// создаёт юнит с заданными областью расположения, свойствами и указанным боевым командиром
        /// </summary>
        /// <param name="polygon">Область расположения</param>
        /// <param name="features">Свойства</param>
        /// <param name="commander">Боевой командир</param>
        public Unit(ConvexPolygon polygon, UnitFeatures features, AutomaticCommander commander)
        {
            figures = new List<Figure> { polygon };
            Polygon = polygon;
            props = features;
            unitCommander = commander;
        }

        /// <summary>
        /// Создание юнита с указанными областью расположения, свойствами и типом
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="features"></param>
        /// <param name="type"></param>
        public Unit(ConvexPolygon polygon, UnitFeatures features, UnitType type)
        {
            figures = new List<Figure> { polygon };
            Polygon = polygon;
            props = features;
            UnitType = type;
            unitCommander = new AutomaticCommander("", CommanderType.Common, this);
        }

        /// <summary>
        /// Специальный конструктор который по Номеру Типа автоматически создает конкретный заполненный Юнит
        /// 1 - Группа Быкова
        /// 2 - Японцы
        /// 3 - Танки
        /// </summary>
        /// <param name="poligon">Область Юнита</param>
        /// <param name="type">Номер Типа</param>
        public Unit(ConvexPolygon poligon, int type)
        {
            Polygon = poligon;
            if (type == 1) // Тип 1
            {
                props = BikovGroup.BikovFea();
                UnitType = UnitType.Infantry;
            }
            if (type == 2) // Тип 2
            {
                props = JapaneseGroup.JapaneseGroupFea();
                UnitType = UnitType.Infantry;
            }
            if (type == 3)  // Тип 3
            {
                props = TankBattalion.TankBattalionFea();
                UnitType = UnitType.Armor;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод, который возвращает список Юнитов, которых видит текущий.
        /// </summary>
        /// <returns>Список видимых юнитов</returns>
        public List<Unit> GetVisible()
        {
            var visible = GetArea();
            List<Unit> VisibleUnits = new List<Unit>();
            foreach (var unit in Game.MainMap.Units)
            {
                if (Intersect.IsIntersected(unit.GetArea(), visible))
                {
                    VisibleUnits.Add(unit);
                }
            }
            return VisibleUnits;
        }

        /// <summary>
        /// Метод возвращает видимую область Юнита
        /// </summary>
        /// <returns>Область круг</returns>
        private Circle GetArea()
        {
            var a = Properties.CurVisible;
            return new Circle(Position, a); // возвращает константную область видимости
        }


        /// <summary>
        /// Метод изменяет текущую скорость и область видимости Юнита в зависимости от текущего местоположения.
        /// </summary>
        internal void SetCurrent()
        {
            Landscape land = GetCurrentArea();
            Properties.CurSpeed = Properties.Speed * land.Passability();// *0.1; 
            Properties.CurVisible = Properties.Visible * land.Visibility();
        }

        /// <summary>
        /// Метод изменяет текущую скорость и область видимости Юнита в зависимости от текущего местоположения, переданного в метод.
        /// </summary>
        /// <param name="land">Местоположение (Местность)</param>
        internal void SetCurrent(Landscape land)
        {
            Properties.CurSpeed = Properties.Speed * land.Passability();
            Properties.CurVisible = Properties.Visible * land.Visibility();
        }

        /// <summary>
        /// Возвращает текущую местность Юнита (По умолчанию "Поле").
        /// </summary>
        /// <returns>Местоположение (Местность)</returns>
        public Landscape GetCurrentArea()
        {
            foreach (var land in Game.MainMap.Lands)
            {
                foreach (var figure in land.Figures)
                {
                    if (Intersect.IsIntersected(Polygon, figure))
                    {
                        return land;
                    }
                }
                break;
            }
            return new Field(new List<Figure>());
        }

        /// <summary>
        /// Возвращает время заданного пути по конкретной области.
        /// </summary>
        /// <param name="land">Местоположение (Местность)</param>
        /// <param name="WayBegin">Начальная точка</param>
        /// <param name="WayEnd">Конечная точка</param>
        /// <returns>Время</returns>
        public double GetPathTime(Landscape land, Point WayBegin, Point WayEnd)
        {
            //SetCurrent(land);
            //return WalkTime(WayBegin, WayEnd);
            double leng = Point.Length(WayBegin, WayEnd);
            return leng / (Properties.Speed * land.Passability());

        }

        /// <summary>
        /// Возвращает время заданного пути по конкретной области.
        /// </summary>
        /// <param name="WayBegin">Начальная точка</param>
        /// <param name="WayEnd">Конечная точка</param>
        /// <returns>Время</returns>
        private double WalkTime(Point WayBegin, Point WayEnd)
        {
            double leng = Point.Length(WayBegin, WayEnd);
            return leng / Properties.CurSpeed;
        }

        /// <summary>
        /// Метод возвращает оружие в зависимости от нужного количества и скорострельности по умолчанию.
        /// </summary>
        /// <param name="count">Количество</param>
        /// <param name="firerate">Скорострельность</param>
        /// <returns>Оружие (Предмет)</returns>
        public EquipmentMark NewSmallArms(int count, double firerate)
        {
            return new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, firerate, 1, count);
        }

        /// <summary>
        /// Метод задания стороны Юниту
        /// </summary>
        /// <param name="team">Номер стороны к которой принадлежит</param>
        /// <param name="country">Страна принадлежности</param>
        /// <param name="alliance">Союз, в который входит страна</param>
        public void SetSide(int team, Countries country, Alliance alliance)
        {
            base.side = new Team(team, country, alliance);
        }

       
        /// <summary>
        /// Проверка возможности начать атаку по противнику.
        /// </summary>
        /// <param name="defender">Цель нападения.</param>
        /// <returns>true если возможность напасть есть.</returns>
        public bool CanAttack(Unit defender)
        {
            var distance = 0.0;
            var positionAtUnit = Position;
            var positionDefUnit = defender.Position;
            distance = Point.Length(positionAtUnit, positionDefUnit);
            return Properties.Visible > distance;
        }


        /// <summary>
        /// Проверка возможности строительства здания
        /// </summary>
        /// <param name="structure">Здание, которое необходимо построить.</param>
        /// <returns>true если строительство доступно.</returns>
        public bool CanBuild(Goods structure)
        {
            //TODO необходимо разграничить различные здания, по требованиям к строительству.
            int engineers = this.Properties.ActiveEngineerCount();
            int buildingMaterials = 0;
            foreach (Goods goods in this.Properties.Items)
            {
                if (goods.ItemType == ObjectType.BuildingMaterials) buildingMaterials+=goods.Count;
            }
            bool a = buildingMaterials > 50;
            bool b = engineers > 10;
            bool c = Geometry.Figures.Point.Length(this.Position,structure.Position) < this.Properties.CurVisible;
            return (a && b && c);
        }

        /// <summary>
        /// Считает количество наносимого урона каждого типа.
        /// </summary>
        /// <returns>Список нанесенного урона</returns>
        public List<double> DamageDealt()
        {
            var accuracy = 0.05;
            var damage = new List<double>();
            var weapons = Properties.ShotsCount();
            damage.Add(weapons[0] * accuracy);
            damage.Add(weapons[1] * accuracy);
            damage.Add(weapons[2] * accuracy);
            damage.Add(weapons[3] * accuracy);
            damage.Add(weapons[4] * accuracy);
            damage.Add(weapons[5] * accuracy);
            damage.Add(weapons[6]);
            return damage;
        }

        /// <summary>
        /// Возвращает урон
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>Урон</returns>
        public Damage AttackingAmount(List<double> damage)
        {
            //attacking percent
            var overShots = damage[6];
            List<double> itogDamage = new List<double>();
            var attackingPercent = UnitCommander.GetParameters()[1];
            for (int i = 0; i < 6; i++)
            {
                itogDamage.Add(damage[i] * attackingPercent);
            }
            for (int i = 0; i < Properties.Items.Count; i++)
            {
                if ((Properties.Items[i] as Goods).ItemType == ObjectType.Ammunition)
                {
                    Properties.Items[i].Count += (int)(overShots * (1 - attackingPercent));
                    i = Properties.Items.Count;
                }
            }
            return new Damage(itogDamage);
        }

        #endregion
    }

    /// <summary>
    /// Класс, описывающих количество наносимого урона
    /// содержит количество урона для каждого типа оружия
    /// </summary>
    public class Damage
    {
        private List<double> damage;

        public List<double> DamageList
        {
            get { return damage; }
            private set { damage = value; }
        }

        /// <summary>
        /// Создание объекта по списку значений
        /// </summary>
        /// <param name="damage"></param>
        public Damage(List<double> damage)
        {
            this.damage = damage;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Damage()
        {
            this.damage = new List<double>(){0,0,0,0,0,0};
        }

        /// <summary>
        /// Суммирует наносимый урон
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Damage operator +(Damage a, Damage b)
        {
            List<double> damage = new List<double>();
            for (int i = 0; i < 6; i++)
            {
                damage.Add(a.damage[i]);
                damage[i] += b.damage[i];
            }
            return new Damage(damage);
        }

        /// <summary>
        /// Пересчитывает количество нанесённого урона из процентного соотношения в число
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public Damage DamagePercent(double percent)
        {
            var newDamage = new List<double>();
            for (int i = 0; i < 6; i++)
            {
                newDamage.Add(damage[i] * percent);
            }
            return new Damage(newDamage);
        }

    }
}
