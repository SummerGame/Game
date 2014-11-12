namespace GameEngine.StoryTelling
{
    /// <summary>
    /// Абстрактный Класс "Состояние"
    /// Предназначен для передачи состояния предмета или юнита UserInterface.
    /// </summary>
    public abstract class State
    {
        #region Fields
        private double time;

        #endregion

        #region Properties
        public double Time
        {
            get { return time; }
            protected set { time = value; }
        }
        #endregion

        #region Methods

        public abstract object GetState();

        #endregion
    }
}
