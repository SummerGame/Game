using System.Collections.Generic;

namespace GameEngine.StoryTelling
{
    /// <summary>
    /// Класс "История"
    /// Является контейнером для "Состояний" предмета или какого-либо объекта.
    /// </summary>
    public class Story
    {
        #region Fields

        private List<State> states;

        #endregion

        #region Properties

        public List<State> States
        {
            get { return states; }
            set { states = value; }
        }

        #endregion

        #region Constructors

        public Story()
        {
            states = new List<State>();
        }
        #endregion
    }
}
