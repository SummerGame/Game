namespace AbstractGameEngine
{
    /// <summary>
    /// Абстрактный Класс "Вещь".
    /// Наследован от Area, т.е. может занимать определенную зону.
    /// Определяет минимальный набор свойств, которыми должен обладать каждый предмет в игре.
    /// </summary>
    public abstract class Item : Area
    {
        #region Properties

        public int Count{ get; set; }

        #endregion
    }
}
