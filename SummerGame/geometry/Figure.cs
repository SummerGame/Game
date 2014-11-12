using System;

namespace Geometry
{
    /// <summary>
    /// Абстрактный Класс Фигура
    /// </summary>
    public abstract class Figure
    {
        // todo remove this
        private object parent;

        public object AParent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Type Type
        {
            get { return GetType(); }
        }


        /// <summary>
        ///  Метод точности
        /// </summary>
        /// <param name="value">проверяемое значение</param>
        public static bool Epsilon(double value, bool less)
        {
            double EPSILON = 0.0001;
            return less ? Math.Abs(value) < EPSILON : Math.Abs(value) > EPSILON;
        }
    }
}
