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
    /// Область - Дорога. Высокая проходимость, высокая открытость, высокая видимость.
    /// </summary>
    public class Road : Landscape
    {
        public Road(List<Figure> figures) : base(figures)
        {
            passability = 1;
            openness = 1;
            visibility = 1;
        }
    }
}
