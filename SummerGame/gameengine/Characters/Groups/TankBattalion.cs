using System.Collections.Generic;
using AbstractGameEngine;
using GameEngine.Object;
using Geometry.Figures;

namespace GameEngine.Characters.Groups
{
    /// <summary>
    /// Танковый Батальон
    /// Конкретный Юнит
    /// </summary>
    public class TankBattalion : Unit
    {

        #region Constructors

        public TankBattalion(ConvexPolygon poligon)
            : base(poligon, TankBattalionFea())
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// Статический метод, который возвращает набор настроек для Танкового Батальона
        /// </summary>
        /// <returns>Набор настроек</returns>
        public static UnitFeatures TankBattalionFea()
        {
            var groups = new List<Group>();
            var items = new List<Item>();
            groups.Add(new Group(160, Specialization.TankMan));
            items.Add(new Goods(ObjectType.Ammunition, 1000, new Circle(0, 0, 0)));
            items.Add(new EquipmentMark(Caliber.small, Caliber.largeSmallArms, 5, 2, 4, 50));
            return new UnitFeatures(groups, items);
        }

        #endregion
    }
}
