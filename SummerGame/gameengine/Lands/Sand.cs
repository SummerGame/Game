using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Песчаная местность
    /// Низкая проходимость, высокая открытость, высокая видимость.
    /// </summary>
    public class Sand: Landscape
    {
        #region Constructors
        public Sand(List<Figure> figures)
            : base(figures)
        {
            passability = 0.6;
            openness = 1;
            visibility = 1;
        }

        #endregion
    }
}




        