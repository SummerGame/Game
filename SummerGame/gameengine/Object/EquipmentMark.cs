using AbstractGameEngine;

namespace GameEngine.Object
{
    #region Enums
    /// <summary>
    /// Перечислимый тип - Калибр
    /// </summary>
    public enum Caliber
    {
        smallArms,
        largeSmallArms,
        small,
        medium,
        large,
        extraLarge
    }

    #endregion

    /// <summary>
    /// Класс "Оружие"
    /// Является Вещью. Характеризует собой класс, описывающий все виды вооружений в игре.
    /// </summary>
    public class EquipmentMark : Item
    {
        #region Propreties

        public Caliber Weapon { get; set; } // Калибр бортового оружия
        public Caliber Armor { get; set; } // Минимальный калибр, необходимый для пробивания брони.
        public double MoveSpeed { get; set; } // Максимальная скорость передвижения.
        public double FireRate { get; set; } // Скорострельность бортового оружия.
        public int Crew { get; set; } // Необходимое количество экипажа для корректной работоспособности.

        #endregion

        #region Contructors

        /// <summary>
        /// Создание оружия по значениям его полей
        /// </summary>
        public EquipmentMark(Caliber weTyp, Caliber arTyp, double speed, double rate, int crewCount, int count)
        {
            Weapon = weTyp;
            Armor = arTyp;
            MoveSpeed = speed;
            FireRate = rate;
            Crew = crewCount;
            Count = count;
        }

        #endregion
    }
}
