using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Горы - Местность 
    /// Низкая проходимость, низкая открытость, средняя видимость.
    /// </summary>
    public class Mountains : Landscape
    {
        #region Constructors
        public Mountains(List<Figure> figures)
            : base(figures)
        {
            passability = 0.6;
            openness = 0.6;
            visibility = 0.8;
        }
        #endregion

    }
}
