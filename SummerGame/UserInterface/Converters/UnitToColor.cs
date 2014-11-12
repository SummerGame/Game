using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using AbstractGameEngine;
using GameEngine.Characters;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using UserInterface.Presenters;

namespace UserInterface
{
    [ValueConversion(typeof(UnitPresenter), typeof(Brush))]
    public class UnitToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SolidColorBrush(Colors.AntiqueWhite);
            if ((value as UnitPresenter).IsMyUnit) return new SolidColorBrush(Colors.Red);
            else return new SolidColorBrush(Colors.Blue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
