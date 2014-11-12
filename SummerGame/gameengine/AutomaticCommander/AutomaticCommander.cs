using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Characters;
using GameEngine.StoryTelling;

namespace GameEngine.Characters
{
    // TODO Изменить названия
    // Тип боевого командира Unita.
    public enum CommanderType
    {
        Common,
        Br,
        Officer
    }

    
    public class AutomaticCommander
    {
        #region Fields

        public string name;

        private CommanderType commanderType;

        public Unit commandUnit;

        private int currentBattleLosses;

        private int firstComposition;

        public int FirstComposition
        {
            get { return firstComposition; }
            set { firstComposition = value; }
        }

        #endregion

        #region Constructors

        public void CurrentBattleLossesUpdate(int summ)
        {
            currentBattleLosses += summ;
        }

        public AutomaticCommander(string commanderName, CommanderType commanderNewType, Unit unit)
        {
            name = commanderName;
            commanderType = commanderNewType;
            commandUnit = unit;
            currentBattleLosses = 0;
            firstComposition = unit.Properties.ActiveCount();
        }

        public AutomaticCommander(string commanderName)
        {
            name = commanderName;
            commanderType = CommanderType.Common;
            //commandUnit = new Unit(new Geometry.Figures.ConvexPolygon());
            currentBattleLosses = 0;
            firstComposition = 0;
        }

        #endregion

        #region Methods

        //Получаем свойства текущего командира.
        /// <summary>
        /// Свойства текущего командира.
        /// </summary>
        /// <returns> 0 минимальный процент для отступления 
        /// 1 процент брошенного в бой личного состава
        /// </returns>
        public List<double> GetParameters()
        {
            var parameters = new List<double>();
            if (commanderType == CommanderType.Common)
            {
                parameters.Add(0.15);
                parameters.Add(0.67);
            }
            else if (commanderType == CommanderType.Br)
            {
                parameters.Add(1);
                parameters.Add(0);
            }
            return parameters;
        }

        /// <summary>
        /// Отступление
        /// </summary>
        /// <param name="enemy">Unit отступает по направлению, противоположную к направлению на противника.</param>
        public void Retreat(Unit enemy)
        {
            var point = new Geometry.Figures.Point(commandUnit.Position.X - enemy.Position.X, commandUnit.Position.Y - enemy.Position.Y);
            double t = 0;
            t = Math.Sqrt((commandUnit.Properties.CurSpeed * commandUnit.Properties.CurSpeed) / (point.X * point.X + point.Y * point.Y));
            Geometry.Figures.Polyline poly = new Geometry.Figures.Polyline(commandUnit.Position.X, commandUnit.Position.Y, commandUnit.Position.X + t * point.X, commandUnit.Position.Y + t * point.Y);
            if (this.commandUnit.CurrentAction != null)
            {
                this.commandUnit.CurrentAction.Completed = true;
                this.commandUnit.CurrentAction = null;
            }
            Reset();
            GameEngine.Game.SendOrder(commandUnit, new MoveAction(commandUnit, poly));
            
        }

        /// <summary>
        /// Отступление от нескольких войск противников
        /// </summary>
        /// <param name="enemy"></param>
        public void Retreat(List<Unit> enemy)
        {
            var ememyAveragePosition = new Geometry.Figures.Point(enemy[0].Position.X,enemy[0].Position.Y);
            for (int i = 1; i < enemy.Count; i++)
            {
                ememyAveragePosition = new Geometry.Figures.Point((ememyAveragePosition.X + enemy[i].Position.X) / 2, (ememyAveragePosition.Y + enemy[i].Position.Y) / 2);
            }
            double t = 0;
            var point = new Geometry.Figures.Point(commandUnit.Position.X - ememyAveragePosition.X, commandUnit.Position.Y - ememyAveragePosition.Y);
            t = Math.Sqrt((commandUnit.Properties.CurSpeed * commandUnit.Properties.CurSpeed) / (point.X * point.X + point.Y * point.Y));
            Geometry.Figures.Polyline poly = new Geometry.Figures.Polyline(commandUnit.Position.X, commandUnit.Position.Y, commandUnit.Position.X + t * point.X, commandUnit.Position.Y + t * point.Y);
            if (this.commandUnit.CurrentAction != null)
            {
                this.commandUnit.CurrentAction.Completed = true;
                this.commandUnit.CurrentAction = null;
            }
            Reset();
            GameEngine.Game.SendOrder(commandUnit, new MoveAction(commandUnit, poly));
        }


        public List<UnitState> Leaving(Unit enemy)
        {
            var point = new Geometry.Figures.Point(commandUnit.Position.X - enemy.Position.X, commandUnit.Position.Y - enemy.Position.Y);
            double t = 0;
            t = Math.Sqrt((commandUnit.Properties.CurSpeed * commandUnit.Properties.CurSpeed) / (point.X * point.X + point.Y * point.Y));
            Geometry.Figures.Polyline poly = new Geometry.Figures.Polyline(commandUnit.Position.X, commandUnit.Position.Y, commandUnit.Position.X + t * point.X, commandUnit.Position.Y + t * point.Y);

            //GameEngine.Game.SendOrder(adm, new MoveAction(adm, poly));
            //return new UnitState

            return new List<UnitState> { new UnitState(poly.Points.Last(), 0, StateAction.Move), new UnitState(poly.Points.Last(), 0, StateAction.Move) };
        }


        public void DoNothing()
        {

        }


        /// <summary>
        /// Начинает бой между Unit'ами
        /// </summary>
        /// <param name="enemy">Юнит, с которым будет начато сражение</param>
        public void Attack(Unit enemy)
        {
            var attack_action = new AttackAction(commandUnit, enemy);
            if (enemy.CurrentAction != null) enemy.CurrentAction = null;
            GameEngine.Game.SendOrder(commandUnit, attack_action);
            
        }

        /// <summary>
        /// Атака войска противника несколькими дружественными войсками
        /// </summary>
        /// <param name="allies"></param>
        /// <param name="enemy"></param>
        public void MassAttack(List<Unit> allies, Unit enemy)
        {
            allies.Add(commandUnit);
            var combAttack = new CombinedAttackAction(allies, enemy);
            GameEngine.Game.SendOrder(commandUnit, combAttack);
        }


        /// <summary>
        /// Делает выводы о целесообразности продолжения сражения.
        /// </summary>
        /// <param name="enemy"> Юнит, с которым идет сражение</param>
        /// <returns>true если сражение надо продолжить.</returns>
        public bool ContinueBattleDecision(Unit enemy)
        {
            if ((double)currentBattleLosses / (double)firstComposition > GetParameters()[0])
            {
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Делает выводы о целесообразности продолжения сражения.
        /// </summary>
        /// <param name="enemy"> Список юнитов, с которыми идет сражение</param>
        /// <returns>true если сражение надо продолжить.</returns>
        public bool ContinueBattleDecision(List<Unit> enemy)
        {
            if ((double)currentBattleLosses / (double)firstComposition > GetParameters()[0])
            {
                return false;
            }
            else return true;
        }

        //Обнуляет промежуточное состояние сражения
        public void Reset()
        {
            currentBattleLosses = 0;
            firstComposition = commandUnit.Properties.ActiveCount();
        }

        #endregion
    }
}
