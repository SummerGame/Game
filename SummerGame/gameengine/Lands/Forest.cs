using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Лес - Местность
    /// Средняя проходимость, низкая открытость, низкая видимость.
    /// </summary>
    public class Forest : Landscape
    {
        #region Constructors
        public Forest(List<Figure> figures)
            : base(figures)
        {
            passability = 0.8;
            openness = 0.6;
            visibility = 0.6;
        }
        #endregion
    }
}
