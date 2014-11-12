using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;
using System.Windows.Data;
using System.Windows.Media;

namespace UserInterface
{
    [ValueConversion(typeof(Point), typeof(System.Windows.Point))]
    public class PointToPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Point p=new System.Windows.Point();

            if (value != null)
            {
                Point point = value as Point;
                var pp = Transformer.ConvertToScreen(point);
                p.X = pp.X;
                p.Y = pp.Y;
                return p;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}