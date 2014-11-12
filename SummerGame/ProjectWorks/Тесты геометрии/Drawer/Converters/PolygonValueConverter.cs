using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Drawer
{

    [ValueConversion(typeof(List<double>), typeof(PointCollection))]
    class PolygonValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                List<double> data = (List<double>)value;
                var points = new List<System.Windows.Point>();

                for (int i = 0; i + 1 < data.Count; i += 2)
                {
                    points.Add(new System.Windows.Point(data[i], data[i + 1]));
                }

                points.TrimExcess();
                return new PointCollection(points);
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
