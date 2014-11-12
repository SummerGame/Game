using AbstractGameEngine;

namespace GameEngine.Characters
{
    /// <summary>
    /// Класс Конкретизирующий принадлежность юнита
    /// </summary>
    public class Team : Side
    {
        #region Fields
        
        private int team;

        private Countries country;

        private Alliance alliance;

        #endregion

        #region Properties

        public int TeamNumber
        {
            get { return team; }
            set { team = value; }
        }

        public Countries Country
        {
            get { return country; }
            set { country = value; }
        }

        public Alliance Alliance
        {
            get { return alliance; }
            set { alliance = value; }
        }

        #endregion

        #region Constructors
        
        public Team(int team, Countries country, Alliance alliance )
        {
            this.team = team;
            this.country = country;
            this.alliance = alliance;
        }
        
        #endregion
    }
}
