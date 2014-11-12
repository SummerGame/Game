using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Водоем - Местность
    /// Низкая проходимость, высокая открытость, средняя видимость.
    /// </summary>
    public class Water : Landscape
    {
        #region Constructors
        public Water(List<Figure> figures)
            : base(figures)
        {
            passability = 0.6;
            openness = 1;
            visibility = 0.8;
        }
        #endregion
    }
}
