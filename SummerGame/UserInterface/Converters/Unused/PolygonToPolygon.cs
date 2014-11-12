using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Geometry.Figures;
using System.Windows.Media;

namespace UserInterface
{

    [ValueConversion(typeof(ConvexPolygon), typeof(PointCollection))]
    public class PolygonToPolygon : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<System.Windows.Point> Points=new List<System.Windows.Point>();

            if (value != null)
            {
                ConvexPolygon polygon = value as ConvexPolygon;
                var centerP = Transformer.ConvertToScreen(polygon.Center());
                foreach (Point p in polygon.Points)
                {
                    var p1 = Transformer.ConvertToScreen(p);
                    Points.Add(new System.Windows.Point(p1.X - centerP.X, p1.Y - centerP.Y));
                }
                return new PointCollection(Points);
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
