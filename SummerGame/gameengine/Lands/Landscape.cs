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
    /// Класс "Местность" - Зона
    /// Вводит понятия проходимость, открытость, видимость.
    /// </summary>
    public class Landscape : Area
    {
        #region Fields

        protected double passability;   // коэффициент проходимости.
        protected double openness;      // коэффициент открытости.
        protected double visibility;    // коэффициент видимости.

        #endregion

        #region Properties

        public double Passability()
        {
            return passability;
        }
        public double Openness()
        {
            return openness;
        }
        public double Visibility()
        {
            return visibility;
        }

        public String Type
        {
            get { return GetType().Name; }
        }

        public string Name
        {
            get
            {
                if (this is City)
                {
                    return "Город";
                }
                else if (this is Road)
                {
                    return "Дорога в ад";
                }
                else if (this is Field)
                {
                    return "Поле";
                }
                else if (this is Forest)
                {
                    return "Чудесный лес";
                }
                else if (this is Lowland)
                {
                    return "Низина";
                }
                else if (this is Mountains)
                {
                    return "Горы";
                }
                else if (this is Water)
                {
                    return "Водица";
                }
                return null;
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Задаёт местность по списку фигур
        /// </summary>
        /// <param name="figures"></param>
        public Landscape(List<Figure> figures) : base(figures)
        {
            figures.ForEach(x => x.AParent = this);
            this.figures = figures;
        }

        public Landscape(List<object> figures)
            : base(figures)
        {
            this.figures = figures.ConvertAll(x => (Figure)x);
            this.figures.ForEach(x => x.AParent = this);
        }

        #endregion
    }
}
