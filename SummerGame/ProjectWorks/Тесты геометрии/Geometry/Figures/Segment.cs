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

        public Segment(Point firts,Point last)
       {
           Begin = firts;
           End = last;
       }

        public Segment (double x1,double y1,double x2,double y2)
        {
            Begin = new Point(x1,y1);
            End = new Point(x2, y2);
        }

        #endregion

        // Получаем позицию на отрезке в зависимости от времени прохождения отрезка и пройденного времени
        public Point Position(double time, double duration)
        {
            if (time > 0)
            {
                var T = duration/time;
                return new Point(((End.X - Begin.X)*T) + Begin.Y, ((End.Y - Begin.Y)*T) + Begin.Y);
            }
            else
            {
                return null;
            }
        }

        public Segment toInt()
        {
            return new Segment(Begin.toInt(),End.toInt());
        }
    }
}
