using System;
using System.Collections.Generic;
using GameEngine.StoryTelling;
using GameEngine.Object;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс "Атака" - Действие
    /// Расчет и выполнение боевого взаимодействия Юнита на другой Юнит
    /// TODO: Реализовать
    /// </summary>
    public class AttackAction : AbstractGameEngine.Action<Unit> 
    {
        private Unit defender;
        private Unit attaker;

        public Unit Attaker
        {
            get { return attaker; }
            set { attaker = value; }
        }

        public Unit Defender
        {
            get { return defender; }
        }

        //private double defenderActivePercent;
        private int attakerFirstComposition;
        private int defenderFirstComposition;

        public AttackAction(Unit atacker, Unit defender)
        {
            this.defender = defender;
            this.attaker = atacker;
            this.attakerFirstComposition = atacker.Properties.ActiveCount();
            this.defenderFirstComposition = defender.Properties.ActiveCount();
        }

        public override object Do(Unit adm)
        {
            //var damage1 = adm.Properties.DamageDealt();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменяет состояние войска
        /// </summary>
        /// <param name="adm"></param>
        /// <param name="time"></param>
        /// <returns>Возвращает новое состояние войска</returns>
        public override object Now(Unit adm, double time)
        {
            var atackDamage = adm.DamageDealt();//TODO перенести в unit
            var defendDamage = defender.DamageDealt();

            var itogAtackDamage = adm.AttackingAmount(atackDamage);
            //var itogDefendDamage = AttackingAmount(defender, defendDamage);

            int atackerLosses = adm.Properties.TakeDamage(defendDamage);
            int defenderLosses = defender.Properties.TakeDamage(itogAtackDamage.DamageList);
            
            if (adm.CanAttack(defender)) Completed = false;
            else Completed = true;

            adm.UnitCommander.CurrentBattleLossesUpdate(atackerLosses);
            defender.UnitCommander.CurrentBattleLossesUpdate(defenderLosses);
            if ((!adm.UnitCommander.ContinueBattleDecision(defender)) || (defenderLosses==0)) adm.UnitCommander.Retreat(defender);
            if ((!defender.UnitCommander.ContinueBattleDecision(adm)) || (atackerLosses==0)) defender.UnitCommander.Retreat(adm);
            
            return new List<UnitState> { new UnitState(adm.Position, Game.GameTimeInterval, StateAction.Attack) };
            //TODO уравнять время анимации атаки с её модельной продолжительностью.
        }
                
        /// Не используется
        public bool BattleOutcome(Unit attacker)
        {
            var atackDamage = attacker.DamageDealt();
            var defendDamage = defender.DamageDealt();

            var attakerInfCount = attacker.Properties.ActiveInfantryCount();
            var attakerTankManCount = attacker.Properties.ActiveTanksManCount();
            var attakerHeavyWeapCount = attacker.Properties.HeavyWeaponCount();


            var defenderInfCount = defender.Properties.ActiveInfantryCount();
            var defenderTankManCount = defender.Properties.ActiveTanksManCount();
            var defenderHeavyWeapCount = defender.Properties.HeavyWeaponCount();

            double attakerDeadPercent = 0;
            double defenderDeadPercent = 0;
            for (int i = 0; i < 6; i++)
            {
                defenderDeadPercent += atackDamage[i];
                attakerDeadPercent += defendDamage[i];
            }
            defenderDeadPercent = defenderDeadPercent / (defenderInfCount + defenderHeavyWeapCount);
            attakerDeadPercent = attakerDeadPercent / (attakerInfCount + attakerHeavyWeapCount);
            //TODO Решить проблему со смертностью при потере танка

            return false;
        }




    }
}
