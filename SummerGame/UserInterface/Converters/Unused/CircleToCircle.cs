using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Geometry.Figures;
using System.Windows.Media;

namespace UserInterface
{
    [ValueConversion(typeof(Circle), typeof(Double))]
    public class CircleToCircle : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Double diameter;

            if (value != null)
            {
                Circle circle = value as Circle;
                diameter = Transformer.ConvertToScreenLength(circle.Radius * 2);

                return diameter;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
