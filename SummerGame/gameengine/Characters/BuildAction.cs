using System;
using System.Collections.Generic;
using GameEngine.StoryTelling;
using GameEngine.Object;
using GameEngine;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс "Построение"
    /// Позволяет создавать постройки (небоевые предметы)
    /// </summary>
    public class BuildAction : AbstractGameEngine.Action<Unit>
    {
        Unit builder;
        Goods construction;
        
        /// <summary>
        /// Добавление небоевого предмета войску
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="building"></param>
        public BuildAction(Unit builder, Goods building)
        {
            this.builder = builder;
            this.construction = building;
        }

        public override object Do(Unit adm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Войско "строит" небоевой предмет
        /// </summary>
        /// <param name="adm"></param>
        /// <param name="time"></param>
        /// <returns>Состояния войска </returns>
        public override object Now(Unit adm, double time)
        {
            if (adm.CanBuild(construction))
            {
                Game.mainMap.Items.Add(construction);
                return new List<UnitState> { new UnitState(adm.Position, Game.GameTimeInterval, StateAction.Build) };
            }
            else return null;
        }
    }
}
