namespace AbstractGameEngine
{
    /// <summary>
    /// Aбстрактный Класс "Исполнитель"
    /// Применяется для объединения всех наследуемых классов, как конкретных предметов, которые могут исполнять Action (Действие).
    /// Содержит основные поля:
    /// Текущее действие;
    /// Side - сторона, принадлежность к какому-либо виду сообщества.
    /// </summary>
    public abstract class Administrant : Area
    {

        #region Fields
        /// <summary>
        /// Текущее действие.
        /// </summary>
        protected Action curAction;

        /// <summary>
        /// Сторона, которой принадлежит исполнитель.
        /// </summary>
        protected Side side;

        #endregion

        #region Properties

        public Action CurrentAction
        {
            get { return curAction; }
            set { curAction = value; }
        }

        public Side Side
        {
            get { return side; }
            //set { side = value; }
        }

        #endregion

    }
}
