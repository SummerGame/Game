using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Низина - Местность 
    /// Низкая проходимость, средняя открытость, низкая видимость.
    /// </summary>
    public class Lowland : Landscape
    {
        #region Constructors
        public Lowland(List<Figure> figures)
            : base(figures)
        {
            passability = 0.6;
            openness = 0.8;
            visibility = 0.6;
        }
        #endregion
    }
}
