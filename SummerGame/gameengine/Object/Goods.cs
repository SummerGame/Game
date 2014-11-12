using System.Collections.Generic;
using AbstractGameEngine;
using Geometry;
using Geometry.Figures;

namespace GameEngine.Object
{
    #region Enums
    /// <summary>
    /// Перечислимый тип - тип предмета
    /// </summary>
    public enum ObjectType
    {
        BuildingMaterials,       // Материалы для строительства инженерных сооружений
        Structures,              // Инженерные строения
        Fuel,                    // Топливо
        Ammunition,              // Боезапас
        Provision,               // Провизия
        Medicines,               // Медикаменты
        Spares                   // ЗИП

    }
    #endregion

    /// <summary>
    /// Класс Предмет
    /// Характеризует небоевые предметы игры.
    /// </summary>
    public class Goods : Item
    {
        #region Fields

        public ObjectType ItemType;

        #endregion

        #region Contructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">тип небоевого предмеа</param>
        /// <param name="kol">количество</param>
        /// <param name="circle">размеры предмета</param>
        public Goods(ObjectType type, int kol, Circle circle)
        {
            figures = new List<Figure> { circle };
            Count = kol;
            ItemType = type;
            this.Polygon = new ConvexPolygon (new List<Point>(){new Point(circle.Center.X - circle.Radius,circle.Center.Y - circle.Radius),
                new Point(circle.Center.X - circle.Radius,circle.Center.Y + circle.Radius),
            new Point(circle.Center.X + circle.Radius,circle.Center.Y + circle.Radius),
            new Point(circle.Center.X + circle.Radius,circle.Center.Y - circle.Radius)});
        }

        #endregion
    }
}
