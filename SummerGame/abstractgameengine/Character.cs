namespace AbstractGameEngine
{
    /// <summary>
    /// Абстрактный Класс "Персонаж"
    /// Представляет собой потомка Абстрактного Класса "Исполнитель", то есть может исполнять Действия.
    /// Используется для обобщения всех героев игры, наделяя их свойствами, которые далее должны быть реализованы, как потомок Абстрактного Класса unitProperty.
    /// </summary>
    public abstract class Character : Administrant
    {

    #region Fields

        /// <summary>
        /// Основные свойства данного персонажа.
        /// </summary>
        public UnitProperties props; //todo implement interface UnitProperties???

    #endregion

    }
}
