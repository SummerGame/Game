using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Geometry.Figures;
using System.Windows.Media;

namespace UserInterface
{
    [ValueConversion(typeof(Polyline), typeof(PointCollection))]
    public class PolylineToPolyline: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<System.Windows.Point> Points = new List<System.Windows.Point>();
            if (value != null)
            {
                Polyline poline = value as Polyline;
                var centerP = Transformer.ConvertToScreen(poline.Center());
                foreach (Point p in poline.Points)
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
            throw new Exception();

        }
    }
}
