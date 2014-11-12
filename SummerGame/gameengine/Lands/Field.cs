using System.Collections.Generic;
using Geometry;

namespace GameEngine.Lands
{
    /// <summary>
    /// Поле - Местность
    /// Средняя проходимость, высокая открытость, высокая видимость.
    /// </summary>
    public class Field : Landscape
    {
        #region Constructors
        public Field(List<Figure> figures) //TODO поля не работают корректно
            : base(figures)
        {
            passability = 0.8;
            openness = 1;
            visibility = 1;
        }
        #endregion
    }
}
