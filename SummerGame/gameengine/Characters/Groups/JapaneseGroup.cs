using System.Collections.Generic;
using AbstractGameEngine;
using GameEngine.Object;
using Geometry.Figures;

namespace GameEngine.Characters.Groups
{
    /// <summary>
    /// Группа Японцев
    /// Конкретный Юнит
    /// </summary>
    public class JapaneseGroup : Unit
    {
        #region Constructors

        public JapaneseGroup(ConvexPolygon poligon)
            : base(poligon, JapaneseGroupFea())
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Статический метод, который возвращает набор настроек для Группы Японцев
        /// </summary>
        /// <returns>Набор настроек</returns>
        public static UnitFeatures JapaneseGroupFea()
        {
            var groups = new List<Group>();
            var items = new List<Item>();
            groups.Add(new Group(350, Specialization.InfantryMan));
            groups.Add(new Group(10, Specialization.TankMan));
            items.Add(new Goods(ObjectType.Ammunition, 1000, new Circle(0, 0, 0)));

            //items.Add(NewSmallArms(300, 1)); // Карабин Мосина
            //items.Add(NewSmallArms(27, 2));
            //items.Add(NewSmallArms(12, 3));

            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 3, 1, 12));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 2, 1, 30));
            items.Add(new EquipmentMark(Caliber.smallArms, Caliber.smallArms, 0.35, 1, 1, 300));
            items.Add(new EquipmentMark(Caliber.small, Caliber.small, 0.35, 1, 2, 2));


            return new UnitFeatures(groups, items);
        }

        #endregion

    }
}

