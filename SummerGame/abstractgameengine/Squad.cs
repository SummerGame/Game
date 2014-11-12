using System.Collections.Generic;

namespace AbstractGameEngine
{
    /// <summary>
    /// Абстрактный Класс Отряд (Группа)
    /// Применим для создания групп из Групп, Вещей, Персонажей.
    /// Выступает как универсальный контейнер фигурирующих персонажей игры.
    /// </summary>
    abstract class Squad : Administrant
    {
        #region Fields
        /// <summary>
        /// Список героев игры, принадлежащих данной группе.
        /// </summary>
        private List<Character> units;

        /// <summary>
        /// Список всех групп, которые входят в состав данной группы.
        /// </summary>
        private List<Squad> squads; 

        /// <summary>
        /// Список предметов, которых принадлежат данно группе.
        /// </summary>
        private List<Item> items;

        #endregion
    }
}
