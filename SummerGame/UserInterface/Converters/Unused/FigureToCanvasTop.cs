using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using Geometry.Figures;
using System.Windows.Data;
using System.Windows.Media;
using Geometry;
using Polyline = Geometry.Figures.Polyline;


namespace UserInterface
{
    [ValueConversion(typeof(System.Windows.Shapes.Shape), typeof(Double))]
    public class FigureToCanvasTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                Point p = new Point(0,Transformer.CurrCanvasY((Figure)value));
                return (Transformer.ConvertToScreen(p).Y);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
