using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractGameEngine;
using Geometry;
using Geometry.Figures;

namespace GameEngine.Lands
{
    /// <summary>
    /// Город - Местность
    /// Средняя проходимость, низкая открытость, низкая видимость.
    /// </summary>
    public class City : Landscape
    {
        #region Constructors
        public City(List<Figure> figures)
            : base(figures)
        {
            passability = 0.8;
            openness = 0.6;
            visibility = 0.6;
        }

        #endregion
    }
}
