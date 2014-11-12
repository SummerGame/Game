using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GameEngine.Characters;
using UserInterface.Presenters;

namespace UserInterface
{
    [ValueConversion(typeof(UserInterface.Presenters.OrderPresenter), typeof(Double))]
    public class ActionToLength : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is OrderPresenter)
            {
                var action = value as OrderPresenter;
                var p0 = action.OrderOrigin;
                var p1 = action.OrderTarget;

                double len = Math.Sqrt(Math.Pow((p0.X - p1.X), 2) + Math.Pow((p0.Y - p1.Y), 2));
                return len;
                //return Transformer.ConvertToScreenLength( Geometry.Measures.Distance(p0, p1) );
            }
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
