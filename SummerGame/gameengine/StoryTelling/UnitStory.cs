using System.Collections.Generic;
using GameEngine.Characters;

namespace GameEngine.StoryTelling
{
    /// <summary>
    /// Класс "История Юнита"
    /// Имеет привязку к Юниту и содержит историю его действий.
    /// </summary>
    public class UnitStory : Story
    {
        #region Fields
        private Unit unit;

        #endregion


        #region Properties
        public Unit Unit
        {
            get { return unit; }
        }
        #endregion

        #region Constructors

        public UnitStory(Unit unit)
        {
            this.unit = unit;
        }

        public UnitStory(Unit unit, List<State> now)
        {
            this.unit = unit;
            States = now;
        }

        #endregion

    }
}
