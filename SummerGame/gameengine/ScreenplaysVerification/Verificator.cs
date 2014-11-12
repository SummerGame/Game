using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Characters;
using GameEngine.Lands;
using Geometry.Figures;



namespace GameEngine.ScreenplaysVerification
{
    public static class KeyTerritory
    {
        #region Fields

        private static int roundTimer;

        private static Team controllingTeam;

        private static List<ConvexPolygon> territory;

        private static int maxScenarioDuration;

        #endregion

        #region Properties

        public static int MaxScenarioDuration
        {
            get { return KeyTerritory.maxScenarioDuration; }
            set { KeyTerritory.maxScenarioDuration = value; }
        }

        public static int RoundTimer
        {
            get { return KeyTerritory.roundTimer; }
            set { KeyTerritory.roundTimer = value; }
        }
        

        public static Team ControllingTeam
        {
            get { return KeyTerritory.controllingTeam; }
            set { KeyTerritory.controllingTeam = value; }
        }

        

        public static List<ConvexPolygon> Territory
        {
            get { return KeyTerritory.territory; }
            set { KeyTerritory.territory = value; }
        }

        #endregion

        #region Methods

        public static void captureReset()
        {
            roundTimer = 0;
            controllingTeam = new Team(-2, Countries.None, new Alliance());
        }



        public static void capture(Team unitTeam)
        {
            if (controllingTeam.TeamNumber < 0) controllingTeam = unitTeam;
            if (unitTeam.TeamNumber != controllingTeam.TeamNumber) captureReset();
        }

        #endregion
    }
    
    public class Verificator
    {
        
        /// <summary>
        /// Функция проверяет, удержала ли сторона территорию от захвата.
        /// </summary>
        /// <returns></returns>
        private bool IsSuccessfulCapture()
        {
            //var flag = true;
            //var currentInvader = KeyTerritory.ControllingTeam.TeamNumber;
            List<Unit> allUnits = Game.MainMap.Units;
            foreach (Unit unit in allUnits)
            {
                foreach (Geometry.Figure unitFugure in unit.Figures)
                {
                    foreach (ConvexPolygon TerritoryPolygon in KeyTerritory.Territory)
                    {
                        if (Intersect.IsIntersected(TerritoryPolygon, unitFugure))
                        {
                            KeyTerritory.capture(unit.Side);
                            //if (KeyTerritory.ControllingTeam.TeamNumber != currentInvader) flag = false;
                        }
                    }
                }
            }
            return (KeyTerritory.ControllingTeam.TeamNumber>0);
        }

       /// <summary>
       /// Проверка условия завершения сценария.
       /// </summary>
       /// <param name="requiredTime"></param>
       /// <returns></returns> 
       private bool IsCompleted(int requiredTime)
        {
            if (KeyTerritory.RoundTimer > requiredTime) return true;
            else return false;
        }
   
       /// <summary>
       /// Возвращает пару текущее состояния сценария
       /// </summary>
       /// <param name="requiredTime">Сколько требуется времени для захвата</param>
       /// <returns>0 - сценарий не завершен, победителя нет.
       /// 1 - сценарий завершен, победитель есть.
       /// 2 - сценарий завершен, победителя нет.
       /// </returns>
       public Tuple<double,Team> mainFunction(int requiredTime)
       {
           if (IsSuccessfulCapture()) KeyTerritory.RoundTimer += 1;
           if (IsCompleted(requiredTime)) return new Tuple<double, Team>(1, KeyTerritory.ControllingTeam);
           else if (KeyTerritory.MaxScenarioDuration - Game.GameTime - requiredTime < 0) return new Tuple<double, Team>(2, KeyTerritory.ControllingTeam);
           else return new Tuple<double, Team>(0, KeyTerritory.ControllingTeam);

       }

    }
}
