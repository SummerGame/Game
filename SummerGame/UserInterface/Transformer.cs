using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry;
using Geometry.Figures;

namespace UserInterface
{
    public static class Transformer
    {
        // Коэффициент, обозначающий разницу между экранными и модельными коориданатами
        private static double ratio = 100;
        private static double translationX = System.Windows.SystemParameters.PrimaryScreenWidth/2;
        private static double translationY = System.Windows.SystemParameters.PrimaryScreenHeight/2;

        public static double Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }

        public static double TranslationX
        {
            get { return translationX; }
            set { translationX = value; }
        }

        public static double TranslationY
        {
            get { return translationY; }
            set { translationY = value; }
        }

        //Преобразование модельных координат в экранные
        public static double ConvertToScreenLength(double model)
        {
            return ratio * model;
        }

        public static Point ConvertToScreen(Point point)
        {
            return new Point(point.X * ratio + translationX, -point.Y * ratio + translationY);
        }

        // Преобразование экранных координат в модельные
        public static double ConvertToModelLength(double screen)
        {
            return screen / ratio;
        }

        public static Point ConvertToModel(Point point)
        {
            return new Point( ( point.X - translationX ) / ratio, (translationY - point.Y) / ratio);
        }

        public static Point ConvertToModel(double x, double y)
        {
            return new Point((x - translationX) / ratio * 12.5, (translationY - y) / ratio * 12.5);
        }

        public static int TimeConverterInt(double time)
        {
            return (int)(time * GameEngine.Game.GameTimeInterval);
        }

        public static double TimeConverterDouble(int time)
        {
            return (double)(time * GameEngine.Game.GameTimeInterval);
        }

        public static double CurrCanvasX(Figure value)
        {
            if (value is Circle)
            {
                var circle = (Circle)value;
                return (circle.Center.X - circle.Radius);
            }
            if (value is Point)
            {
                var point = (Point)value;
                return (point.X - 3);
            }
            else if (value is ConvexPolygon)
            {
                var poly = (ConvexPolygon)value;
                return poly.Center().X;
            }
            else if (value is Polyline)
            {
                var polyline = (Polyline)value;
                return polyline.Center().X;
            }
            else if (value is Segment)
            {
                var segment = (Segment)value;
                return (segment.Begin.X + segment.End.X) / 2;
            }
            return 0;
        }

        public static double CurrCanvasY(Figure value)
        {

            if (value is Circle)
            {
                var circle = (Circle)value;
                return (circle.Center.Y - circle.Radius);
            }
            if (value is Point)
            {
                var point = (Point)value;
                return (point.Y - 3);
            }
            else if (value is ConvexPolygon)
            {
                var poly = (ConvexPolygon)value;
                return poly.Center().Y;
            }
            else if (value is Polyline)
            {
                var polyline = (Polyline)value;
                return polyline.Center().Y;
            }
            else if (value is Segment)
            {
                var segment = (Segment)value;
                return (segment.Begin.Y + segment.End.Y) / 2;
            }
            return 0;
        }
    }
}
