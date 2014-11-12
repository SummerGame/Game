using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Figures;
using System.Windows.Data;
using System.Windows.Media;

namespace UserInterface
{
    [ValueConversion(typeof(Circle), typeof(Double))]
    public class CircleToY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Double y;

            if (value != null)
            {
                Circle circle = value as Circle;
                y = circle.Center.Y;
                return y;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
