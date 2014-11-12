using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry
{
    /// <summary>
    /// Абстрактный Класс Фигура
    /// </summary>
    public abstract class Figure
    {
        public Type Type
        {
            get { return GetType(); }
        }

        // Метод точности
        public static bool Epsilon(double value, bool less)
        {
            double EPSILON = 0.0001;
            return less ? Math.Abs(value) < EPSILON : Math.Abs(value) > EPSILON;
        }
    }
}
