using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Geometry.Figures;
using System.Windows.Media;

namespace UserInterface
{
    [ValueConversion(typeof(Segment), typeof(PointCollection))]
    public class SegmentToSegment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<System.Windows.Point> Points = new List<System.Windows.Point>();
            if (value != null)
            {
                Segment segment = value as Segment;
                var p = (Transformer.ConvertToScreen(segment.Begin));
                var p1 = (Transformer.ConvertToScreen(segment.End));

                var centerP = Transformer.ConvertToScreen(segment.Center());

                Points.Add(new System.Windows.Point(p.X - centerP.X, p.Y - centerP.Y));
                Points.Add(new System.Windows.Point(p1.X - centerP.X, p1.Y - centerP.Y));
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
