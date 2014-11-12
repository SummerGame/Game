using System;
using Geometry.Figures;

namespace GameEngine.StoryTelling
{
    #region Enums
    /// <summary>
    /// Перечислимый тип - Действие состояния
    /// </summary>
    public enum StateAction
    {
        Move,
        Attack,
        Build,
        CombiendAttack
    }

    #endregion

    /// <summary>
    /// Класс "Состояние Юнита"
    /// Предполагает наличие текущего действия состояния, времени действия и позиции.
    /// </summary>
    public class UnitState : State
    {
        #region Fields
        private Point position;
        private StateAction _action;
        #endregion

        #region Properties

        public StateAction Action
        {
            get { return _action; }
        }

        public Point Position
        {
            get { return position; }
        }
        #endregion

        #region Constructors
        public UnitState(Point position, double time, StateAction stateAction)
        {
            this.position = position;
            this.Time = time;
            this._action = stateAction;
        }



        #endregion

        #region Methods
        public override object GetState()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
