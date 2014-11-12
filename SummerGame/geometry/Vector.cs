using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;

namespace Geometry
{
    /// <summary>
    /// Класс вектор
    /// </summary>
    public class Vector
    {
        #region Properties

        /// <summary>
        /// Координаты вектора
        /// </summary>
        public double X { get; private set; }

        public double Y { get; private set; }

        /// <summary>
        /// Длина вектора
        /// </summary>
        public double Length
        {
            get
            {
                return Geometry.Measures.Distance(0, 0, X, Y);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор вектора по паре координат. Бросает исключение если вектор окажется нулевой длины.
        /// </summary>
        /// <param name="x">первая координата вектора</param>
        /// <param name="y">вторая координата вектора</param>
        public Vector(double x, double y)
        {
            if (x == 0 && y == 0) throw new Exception("Geometry.Vector. Attempt to construct zero length vector.");
            this.X = x; this.Y = y;
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="v">копируемый вектор</param>
        public Vector(Vector v)
        {
            this.X = v.X; this.Y = v.Y;
        }

        #endregion

        #region FactoryMethods

        /// <summary>
        /// Строит вектор, направленный от точки a к точке b. Может бросить исключение при совпадении точек.
        /// </summary>
        /// <param name="a">исходная точка</param>
        /// <param name="b">конечная точка</param>
        /// <returns>новый вектор</returns>
        public static Vector BetweenPoints(Point a, Point b)
        {
            return new Vector(b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Строит радиус-вектор для заданной точки. Бросает исключение если точка - начало координат
        /// </summary>
        /// <param name="a">точка</param>
        /// <returns>новый вектор</returns>
        public static Vector RadiusVector(Point a)
        {
            return new Vector(a.X, a.Y);
        }

        /// <summary>
        /// Левая нормаль к вектору
        /// </summary>
        /// <param name="v">вектор</param>
        /// <returns>новый вектор</returns>
        public static Vector LeftNormal(Vector v)
        {
            return new Vector(-v.Y, v.X);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Сложение векторов. Бросает исключение если получен нулевой вектор
        /// </summary>
        /// <param name="a">первое слагаемое</param>
        /// <param name="b">второе слагаемое</param>
        /// <returns>новый вектор суммы</returns>
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Вычитание векторов. Бросает исключение если получен нулевой вектор
        /// </summary>
        /// <param name="a">первый вектор</param>
        /// <param name="b">второй вектор</param>
        /// <returns>вектор разности</returns>
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Скалярное произведение
        /// </summary>
        /// <param name="a">первый вектор</param>
        /// <param name="b">второй вектор</param>
        /// <returns>значение скалярного произведения</returns>
        public static double operator *(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Умножение на число. Бросает исключение если получен нулевой вектор
        /// </summary>
        /// <param name="a">число</param>
        /// <param name="v">вектор</param>
        /// <returns>новый вектор</returns>
        public static Vector operator *(double a, Vector v)
        {
            return new Vector(a * v.X, a * v.Y);
        }

        /// <summary>
        /// Умножение на число. Бросает исключение если получен нулевой вектор
        /// </summary>
        /// <param name="v">вектор</param>
        /// <param name="a">число</param>
        /// <returns>новый вектор</returns>
        public static Vector operator *(Vector v, double a)
        {
            return new Vector(a * v.X, a * v.Y);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Нормализация вектора.
        /// </summary>
        /// <returns>новый вектор единичной длины </returns>
        public Vector Normalise()
        {
            return (1 / this.Length) * this;
        }

        //?
        public static double get_angle(Vector vector1, Vector vector2)
        {
            var firstChisl = vector1 * vector2;
            var firstZnam = Math.Sqrt(vector1 * vector1) * Math.Sqrt(vector2 * vector2);
            var fir = firstChisl / firstZnam;
            return Math.Acos(fir);
        }

        #endregion
    }
}
