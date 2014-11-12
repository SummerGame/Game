using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GameEngine.Characters;

namespace UserInterface
{

    [ValueConversion(typeof(Unit), typeof(String))]
    public class UnitToStringsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "NULL";
            else if (!(value is Unit)) return "NULL";
            else
            {
                int index = 0;
                if (parameter != null && (parameter is int)) index = (int) parameter;
                var unit = value as Unit;
                return "---";//unit.Descr[index];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
