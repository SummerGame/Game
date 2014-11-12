using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;
using System.Windows.Data;
using System.Windows.Media;
using Geometry;


namespace UserInterface
{
    [ValueConversion(typeof(Figure), typeof(Double))]
    public class FigureToCanvasLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                Point p = new Point(Transformer.CurrCanvasX((Figure)value), 0);
                return (Transformer.ConvertToScreen(p).X);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
