using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.StoryTelling;
using GameEngine.Object;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс реализует действие нападение нескольких дружественных юнитов на один вражеский
    /// </summary>
    public class CombinedAttackAction : AbstractGameEngine.Action<List<Unit>>
    {
        private Unit defender;
        private List<Unit> attackers;

        public override object Do(List<Unit> adm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменяет состояния нападающих войск
        /// </summary>
        /// <param name="units">Список атакующих войск</param>
        /// <param name="time"></param>
        /// <returns>Текущие состояния атакующих войск</returns>
        public override object Now(List<Unit> units, double time)
        {
            var defendDamage = defender.DamageDealt();
            var attackDamage = new Damage();
            var reflectedPercents = new List<double>();
            double activeAttackersCount = 0;
            var returnedList = new List<UnitState>();

            foreach (var adm in attackers)
            {
                var atackDamage = adm.DamageDealt();
                var itogAtackDamage = adm.AttackingAmount(atackDamage);
                var currentAttackersCount = adm.Properties.ActiveCount() * adm.UnitCommander.GetParameters()[1];
                attackDamage += itogAtackDamage;
                activeAttackersCount += currentAttackersCount;
                reflectedPercents.Add(currentAttackersCount);
            }

            for (int i = 0; i < reflectedPercents.Count; i++)
            {
                reflectedPercents[i] /= activeAttackersCount;
            }

            int defenderLosses = defender.Properties.TakeDamage(attackDamage.DamageList); //TODO ПЕРЕНЕСТИ из пропертис!!!
            var defenderItogDamage = defender.AttackingAmount(defendDamage);
            defender.UnitCommander.CurrentBattleLossesUpdate(defenderLosses);
            if (!defender.UnitCommander.ContinueBattleDecision(attackers)) defender.UnitCommander.Retreat(attackers);

            var index = 0;
            foreach (var adm in attackers)
            {
                int currentAttackerLosses = adm.Properties.TakeDamage(defenderItogDamage.DamagePercent(reflectedPercents[index]).DamageList);
                adm.UnitCommander.CurrentBattleLossesUpdate(currentAttackerLosses);
                //defender.UnitCommander.CurrentBattleLossesUpdate(defenderLosses);
                if ((!adm.UnitCommander.ContinueBattleDecision(defender)) || (defenderLosses == 0)) adm.UnitCommander.Retreat(defender);
                returnedList.Add(new UnitState(adm.Position, Game.GameTimeInterval, StateAction.Attack)); // ??????????
            }
            Completed = false;

            return returnedList;

        }

        /// <summary>
        /// Устанавливает список нападающих войск и обороняющееся войско
        /// </summary>
        /// <param name="attackers"></param>
        /// <param name="defender"></param>
        public CombinedAttackAction(List<Unit> attackers, Unit defender)
        {
            this.attackers = attackers;
            this.defender = defender;
        }
    }
}
