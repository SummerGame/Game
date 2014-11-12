using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Figures
{
    /// <summary>
    /// Класс Отрезок
    /// </summary>
    public class Segment : Figure
    {
        #region Свойства

        public Point Begin { get; set; }

        public Point End { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Устанавливает значения свойств
        /// </summary>
        /// <param name="firts">начало отрезка</param>
        /// <param name="last">конец отрезка</param>
        public Segment(Point firts,Point last)
       {
           Begin = firts;
           End = last;
       }

        /// <summary>
        /// Принимает координаты точек начала и конца и устанавливает значения свойств
        /// </summary>
        public Segment (double x1,double y1,double x2,double y2)
        {
            Begin = new Point(x1,y1);
            End = new Point(x2, y2);
        }

        #endregion

        #region Операторы

        /// <summary>
        /// Перегрузка оператора неравенства
        /// </summary>
        public static bool operator !=(Segment first, Segment second)
        {
            if (first.Begin != second.Begin && first.End != second.End)
                return true;
            return false;
        }

        /// <summary>
        /// Проверка равенства двух отрезков
        /// </summary>
        public static bool operator ==(Segment first, Segment second)
        {
            if (first.Begin == second.Begin && first.End == second.End)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        ///  Получаем позицию на отрезке в зависимости от времени прохождения отрезка и пройденного времени
        /// </summary>
        /// <param name="time">время прохождения отрезка</param>
        /// <param name="duration">времени прошло</param>
        /// <returns>текущую позицию</returns>
        public Point Position(double time, double duration)
        {
            if (time > 0)
            {
                var T = duration/time;
                return new Point(((End.X - Begin.X)*T) + Begin.X, ((End.Y - Begin.Y)*T) + Begin.Y);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Отрезок с целыми значениями свойств
        /// </summary>
        public Segment toInt()
        {
            return new Segment(Begin.toInt(),End.toInt());
        }

        /// <summary>
        /// Возвращает точку, которая является серединой отрезка
        /// </summary>
        public Point Center()
        {
            return new Point((Begin.X + End.X) / 2, (Begin.Y + End.Y) / 2);
        }

        /// <summary>
        /// Вычисляет текущую позицию
        /// </summary>
        /// <param name="koef">пройденная часть отрезка</param>
        /// <returns>текущую позицию</returns>
        public Point GetCurentPosition(double koef)
        {
            var vect = new Point(End.X - Begin.X, End.Y - Begin.Y);
            vect.X = vect.X * koef;
            vect.Y = vect.Y * koef;
            vect.X += Begin.X;
            vect.Y += Begin.Y;
            return vect;
        }
    }
}
