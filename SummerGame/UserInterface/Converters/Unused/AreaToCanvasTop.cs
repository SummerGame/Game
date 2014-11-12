using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using Geometry.Figures;
using System.Windows.Data;
using System.Windows.Media;
using Geometry;
using AbstractGameEngine;


namespace UserInterface
{
    [ValueConversion(typeof(Area), typeof(Double))]
    public class AreaToCanvasTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                Area area = value as Area;
                Point p = new Point(0,area.Polygon.Center().Y);

                return Transformer.ConvertToScreen(p).Y;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
