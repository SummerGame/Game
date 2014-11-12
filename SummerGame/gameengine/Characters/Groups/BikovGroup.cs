using System.Collections.Generic;
using AbstractGameEngine;
using GameEngine.Object;
using Geometry.Figures;

namespace GameEngine.Characters.Groups
{
    /// <summary>
    /// Группа Быкова
    /// Конкретный Юнит
    /// </summary>
    public class BikovGroup: Unit
    {
        #region Constructors

        public BikovGroup(ConvexPolygon poligon)
            : base(poligon, BikovFea())
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Статический метод, который возвращает набор настроек для Группы Быкова
        /// </summary>
        /// <returns>Набор настроек</returns>
        public static UnitFeatures BikovFea()
        {
            var groups = new List<Group>();
            var items = new List<Item>();
            groups.Add(new Group(90, Specialization.TankMan));
            groups.Add(new Group(360, Specialization.InfantryMan));
            items.Add(new Goods(ObjectType.Ammunition, 1000, new Circle(0, 0, 0)));

            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 3, 1, 12));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 2, 1, 27));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 1, 1, 300));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.largeSmallArms, 5, 2, 2, 8));
            items.Add(new EquipmentMark(Caliber.small, Caliber.largeSmallArms, 5, 2, 4, 11));
            items.Add(new EquipmentMark(Caliber.medium, Caliber.largeSmallArms, 0.5, 1, 2, 4));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.largeSmallArms, 5, 1, 2, 10));
            return new UnitFeatures(groups, items);
        }

        #endregion
    }
}
