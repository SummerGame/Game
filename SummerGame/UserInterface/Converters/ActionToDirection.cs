using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GameEngine.Characters;
using UserInterface.Presenters;

namespace UserInterface
{
    [ValueConversion(typeof(OrderPresenter), typeof(Double))]
    public class ActionToDirection : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is OrderPresenter)
            {
                var action = value as OrderPresenter;
                var p0 = action.OrderOrigin;
                var p1 = action.OrderTarget;
                Geometry.Figures.Point orig = new Geometry.Figures.Point(p0.X, p0.Y);
                Geometry.Figures.Point targ =  new Geometry.Figures.Point(p1.X, p1.Y);
                var v = Geometry.Vector.BetweenPoints(orig, targ);
                var radians = Geometry.Measures.AngleFromXOrto(v);
                var degrees = Geometry.Measures.ToDegrees(-radians); // Rotate transform eats angles CLOCKWISE and in degrees
                return degrees;
            }
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
